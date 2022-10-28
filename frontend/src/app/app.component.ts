import {Component, OnInit} from '@angular/core';
import {HttpService} from "../services/http.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit{
  formModel : Box = new Box();
  box: any;
  boxes: Array<Box> = [];


  constructor(private http : HttpService) {

  }

  async ngOnInit(){
    const boxData = await this.http.getBox();
    this.boxes = boxData;
    this.box = boxData;
  }

  async postForm(){
    if(this.formModel.id === 0){
      await this.createBox();
    } else {
      await this.editBox(this.formModel.id);
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
    let indexToEdit = this.boxes.findIndex(b => b.id == id);
    this.boxes[indexToEdit] = box;
    this.formModel = new Box();
  }

  selectCard(box : Box){
    this.formModel = {...box}; // creates a copy of box and sets it to the form - done to prevent 2 way binding
  }

  clearForm(){
    this.formModel = new Box();
  }
}

class BoxDto {
  size: string = "";
  customerName: string = "";
  type: string = "";
}

class Box extends BoxDto{
  id: number = 0;
}
