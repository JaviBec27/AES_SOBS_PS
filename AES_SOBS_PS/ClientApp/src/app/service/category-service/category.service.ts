import { Injectable } from '@angular/core';
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import "rxjs/add/observable/throw";
import { BaseApiService } from "../base-api.service";
import { Category } from "app/models/Category"

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private baseAPI: BaseApiService) { }

  
  public getAllCategories() {
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Categories`;
    return this.baseAPI.http
      .get(vUrl ,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }

  public getCategoryProvider(idProvider: string, pCategory:string) {
    const body = { idcategoria: pCategory, iduser: idProvider};
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Categories`;
    return this.baseAPI.http
      .put(vUrl,body,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }

  public getCategory(idCategory:number) {
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Categories/${idCategory}`;
    return this.baseAPI.http
      .get(vUrl ,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }
 
}
