import { Injectable } from '@angular/core';

import axios from "axios";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";

export const customAxios = axios.create({
  baseURL: 'https://localhost:7153'
})

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(private matSnackbar: MatSnackBar) {
    customAxios.interceptors.response.use(
      response => {
        if(response.status == 201){
          this.matSnackbar.open("Great Success")
        }
        return response
      },
      rejected => {
        if (rejected.response.status>= 400 && rejected.response.status <500){
          matSnackbar.open(rejected.response.data);
        }else if (rejected.response.status>499){
           this.matSnackbar.open("Something Went Wrong")
        }
        catchError(rejected)
      }
    )
  }

  async getBox(){
    const httpResponse = await customAxios.get('box/GetAll');
    return httpResponse.data;
  }


  async createBox(dto: { size: any; type: any; customerName: any }) {
   const httpResponse = await customAxios.post('box',dto)
    return httpResponse.data;
  }

  async deleteBox(id : any) {
    const httpResponse = await customAxios.delete('box' + id)
    return httpResponse.data;
  }


}
