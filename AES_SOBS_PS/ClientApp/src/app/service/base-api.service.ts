import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders
} from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { environment } from "environments/environment";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import "rxjs/add/observable/throw";
import { Constants } from "app/models/Constants";

@Injectable({
  providedIn: "root"
})
export class BaseApiService {
  webserviceurl = environment.APIEndPoint;

  constructor(public http: HttpClient) {}

  getToken() {
    const headers = new HttpHeaders({
      Authorization:
        Constants.Keys.Bearer + sessionStorage.getItem("token")
    });
    return {
      headers: headers,
      "observe?": "body",
      "responseType?": "json"
    };
  }

  errorHandler(error: HttpErrorResponse) {
    const errMsg = error.message
      ? error.message
      : error.status
      ? `${error.status} - ${error.statusText}`
      : "Server error";
    return Observable.throw(error);
  }
}
