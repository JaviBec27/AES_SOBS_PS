import { Injectable } from '@angular/core';
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import "rxjs/add/observable/throw";
import { BaseApiService } from "../base-api.service";
import { Product } from "app/models/Product";

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private baseAPI: BaseApiService) { }
  
  
  public insertProduct(pProduct:Product) {
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Products`;
    return this.baseAPI.http
      .post(vUrl, pProduct ,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }

    
  public UpdateProduct(pProduct:Product) {
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Products/${pProduct.idProducto}`;
    return this.baseAPI.http
      .put(vUrl, pProduct ,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }

  public deleteProduct(pProduct:Product) {
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Products/${pProduct.idProducto}`;
    return this.baseAPI.http
      .delete(vUrl ,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }



}


