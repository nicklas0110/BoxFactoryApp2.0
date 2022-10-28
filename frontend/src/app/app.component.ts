import {Component, OnInit} from '@angular/core';
import {HttpService} from "../services/http.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  BoxSize: string = "";
  CustomerName: string = "";
  Type: string = "";
  box: any;




  constructor(private http : HttpService) {

  }


 async ngOnInit(){
    const boxData = await this.http.getBox();
    this.box = boxData;
  }

  async createBox(){
    let dto = {
      size: this.BoxSize,
      customerName: this.CustomerName,
      type: this.Type,
    }
    const result = await this.http.createBox(dto);
    this.box.push(result);
    console.log(result.data);
    this.BoxSize = "";
    this.CustomerName = "";
    this.Type = "";
  }

  writeBoxSize(){
    console.log(this.BoxSize);
    console.log(this.CustomerName);
    console.log(this.Type);
  }


  async deleteBox(id:any) {
   const box = await this.http.deleteBox(id);
   this.box = this.box.filter((b: { id: any; }) => b.id != box.id)
  }

  async editBox(id: any) {
    console.log(this.BoxSize);
    console.log(this.CustomerName);
    console.log(this.Type);
    let dto = {
      id: id,
      size: this.BoxSize,
      customerName: this.CustomerName,
      type: this.Type,
    }
    const box = await this.http.editBox(id, dto);
    console.log("This is box", box);
    this.box = this.box.filter((b: { id: any; }) => b.id != box.id)
    this.box.push(box);
    this.BoxSize = "";
    this.CustomerName = "";
    this.Type = "";
  }
}
