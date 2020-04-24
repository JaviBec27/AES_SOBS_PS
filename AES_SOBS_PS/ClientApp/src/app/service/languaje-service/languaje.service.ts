import { Injectable } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { Constants } from "app/models/Constants";

/**
 *Service to manage languaje
 *
 * @export
 * @class LanguajeService
 */
@Injectable({
  providedIn: "root"
})
export class LanguajeService {
  constructor(private translate: TranslateService) {}

  /**
   *Method to get current languaje
   *
   * @returns {string}
   * @memberof LanguajeService
   */
  getLanguaje(): string {
    let vLanguaje = localStorage.getItem(Constants.Keys.Lang);
    if (vLanguaje === null || vLanguaje === undefined) {
      this.translate.addLangs(Constants.Languajes);
      this.translate.setDefaultLang(Constants.Keys.DefaultLang);
      const vBrowserLang = this.translate.getBrowserCultureLang();
      const vLangs = this.translate.getLangs();

      if (vLangs.indexOf(vBrowserLang) >= 0) {
        vLanguaje = vBrowserLang;
      } else {
        vLanguaje = Constants.Keys.DefaultLang;
      }
    }

    return vLanguaje;
  }
}
