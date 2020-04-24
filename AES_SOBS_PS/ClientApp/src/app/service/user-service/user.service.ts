import { Injectable } from '@angular/core';
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import "rxjs/add/observable/throw";
import { BaseApiService } from "../base-api.service";
import { User } from 'app/models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {


  constructor(private baseAPI: BaseApiService) { 

  }

  public insertUser(pUser:User) {
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Account/Create`;
    return this.baseAPI.http
      .post(vUrl, pUser ,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }
}
