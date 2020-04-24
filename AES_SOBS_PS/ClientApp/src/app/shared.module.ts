import { NgModule } from "@angular/core";
import { TranslateModule, TranslateLoader, TranslatePipe, TranslateService } from "@ngx-translate/core";
import { HttpClient } from "@angular/common/http";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { LanguajeService } from "./service/languaje-service/languaje.service";
import { Constants } from "./models/Constants";
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}

@NgModule({
  exports: [TranslateModule],
  imports: [
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [TranslatePipe]
})
export class SharedModule {
  constructor(private translate: TranslateService) {
    this.setLanguaje();
  }

  setLanguaje() {
    const vLanguajeService = new LanguajeService(this.translate);
    const vLanguaje = vLanguajeService.getLanguaje();
    localStorage.setItem(Constants.Keys.Lang, vLanguaje);
    this.translate.use(vLanguaje);
  }
}
