import { Injectable } from "@angular/core";
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Constants } from "app/models/Constants";

@Injectable({
  providedIn: "root"
})
export class AuthGuardService {
  constructor(private router: Router) {}

  /**
   * Validate user either logged in or not on all request
   */
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const loggedin = sessionStorage.getItem(Constants.Keys.Loggedin);
    if (loggedin === "1") {
      return true;
    } else {
      this.router.navigate(["/pages/login"]);
      return false;
    }
  }
}
