import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";

/**
 *Service to manage http status
 *
 * @export
 * @class HttpStatusService
 */
@Injectable({
  providedIn: "root"
})
export class HttpStatusService {
  private requestInFlight$: BehaviorSubject<boolean>;

  /**
   *Creates an instance of HttpStatusService.
   * @memberof HttpStatusService
   */
  constructor() {
    this.requestInFlight$ = new BehaviorSubject(false);
  }

  /**
   *Method to set http status
   *
   * @param {boolean} inFlight
   * @memberof HttpStatusService
   */
  setHttpStatus(inFlight: boolean) {
    this.requestInFlight$.next(inFlight);
  }

  /**
   *Method to set http status
   *
   * @returns {Observable<boolean>}
   * @memberof HttpStatusService
   */
  getHttpStatus(): Observable<boolean> {
    return this.requestInFlight$.asObservable();
  }
}
