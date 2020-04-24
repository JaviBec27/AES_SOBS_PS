import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class CommonService {
  constructor() {}

  openSideNav() {
    document.querySelector("body").classList.remove("aside-menu-hidden");
  }

  closeSideNav() {
    document.querySelector("body").classList.add("aside-menu-hidden");
  }
}
