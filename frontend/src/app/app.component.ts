import {Component, OnInit} from '@angular/core';
import {HttpService} from "../services/http.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit{
  formModel : Box = new Box(); // Sets formModel = to the Box class
  box: any;
  boxes: Array<Box> = []; // an array of boxes


  constructor(private http : HttpService) {

  }

  async ngOnInit(){
    const boxData = await this.http.getBox();
    this.boxes = boxData;
    this.box = boxData;
  }

  async postForm(){
    if(this.formModel.id === 0){
      await this.createBox(); // if the id = 0 no box needs to be editet so we know you want to crate
    } else {
      await this.editBox(this.formModel.id); // Sets the id of the box class to be edits
    }
  }

  async createBox(){
    const result = await this.http.createBox(this.formModel as BoxDto);
    this.boxes.push(result);
    this.formModel = new Box();
  }

  async deleteBox(id:any) {
   const box = await this.http.deleteBox(id);
   this.boxes = this.boxes.filter((b: { id: any; }) => b.id != box.id)
  }

  async editBox(id: any) {
    const box = await this.http.editBox(id, this.formModel);
    let indexToEdit = this.boxes.findIndex(b => b.id == id); // Sets the id of the box class for the url
    this.boxes[indexToEdit] = box;
    this.formModel = new Box();
  }

  selectCard(box : Box){
    this.formModel = {...box}; // creates a copy of box and sets it to the form - done to prevent 2 way binding
  }

  clearForm(){
    this.formModel = new Box(); // sets the info to the base value we have whits is blank for txt fields and id is 0
  }
}
// is in change of the 3 txt fields information storing when creating or edit
class BoxDto {
  size: string = "";
  customerName: string = "";
  type: string = "";
}
// Sets the id when it is needed
class Box extends BoxDto{
  id: number = 0;
}
