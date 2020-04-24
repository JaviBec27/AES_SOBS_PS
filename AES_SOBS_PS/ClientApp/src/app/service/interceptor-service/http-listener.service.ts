
import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { HttpStatusService } from "./http-status.service";
import { Observable } from "rxjs/Observable";
import { catchError, finalize, map } from "rxjs/operators";

/**
 *Service to intercept http request
 *
 * @export
 * @class HttpListenerService
 * @implements {HttpInterceptor}
 */
@Injectable({
  providedIn: "root"
})
export class HttpListenerService implements HttpInterceptor {

  /**
   *Creates an instance of HttpListenerService.
   * @param {HttpStatusService} status
   * @memberof HttpListenerService
   */
  constructor(private status: HttpStatusService) {}

  /**
   *Method to intercep http request and show load icon
   *
   * @param {HttpRequest<any>} req :Request
   * @param {HttpHandler} next : Http handler
   * @returns {Observable<HttpEvent<any>>} : Observable class
   * @memberof HttpListenerService
   */
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.status.setHttpStatus(true);

    return next.handle(req).pipe(
      map(event => {
        return event;
      }),
      catchError(error => {
        return Observable.throw(error);
      }),
      finalize(() => {
        this.status.setHttpStatus(false);
      })
    );
  }
}
