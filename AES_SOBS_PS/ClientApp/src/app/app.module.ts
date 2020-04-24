import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule, Injector, APP_INITIALIZER } from "@angular/core";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { RecaptchaModule } from 'ng-recaptcha';


import {CodeHighlighterModule} from 'primeng/codehighlighter';
import {
  LocationStrategy,
  HashLocationStrategy,
  LOCATION_INITIALIZED
} from "@angular/common";

import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";
import { PERFECT_SCROLLBAR_CONFIG } from "ngx-perfect-scrollbar";
import { PerfectScrollbarConfigInterface } from "ngx-perfect-scrollbar";
import {
  TranslateModule,
  TranslateLoader,
  TranslateService,
  TranslatePipe
} from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}
export function appInitializerFactory(
  translate: TranslateService,
  injector: Injector
) {
  return () =>
    new Promise<any>((resolve: any) => {
      const locationInitialized = injector.get(
        LOCATION_INITIALIZED,
        Promise.resolve(null)
      );
      locationInitialized.then(() => {
        const vLanguajeService = new LanguajeService(translate);
        const vLanguaje = vLanguajeService.getLanguaje();

        translate.use(vLanguaje).subscribe(() => {
          resolve(null);
        });
      });
    });
}
const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from "./app.component";
// Import components
import {
  AppAsideComponent,
  AppFooterComponent,
  AppHeaderComponent,
  AppSidebarComponent,
  AppSidebarFooterComponent,
  AppSidebarFormComponent,
  AppSidebarHeaderComponent,
  AppSidebarMinimizerComponent,
  AppContentHeaderComponent,
  AppLoadingComponent,
  APP_SIDEBAR_NAV
} from "./components";

// Import containers
import { FullLayoutComponent } from "./containers";

const APP_CONTAINERS = [FullLayoutComponent, SimpleLayoutComponent];

const APP_COMPONENTS = [
  AppAsideComponent,
  AppLoadingComponent,
  AppHeaderComponent,
  AppFooterComponent,
  AppSidebarComponent,
  AppSidebarHeaderComponent,
  AppSidebarFormComponent,
  AppSidebarFooterComponent,
  AppSidebarMinimizerComponent,
  AppContentHeaderComponent,
  APP_SIDEBAR_NAV,
  CreateUserModalComponent,
  ProviderViewFormComponent,
  PayOrderModuleComponent
];

// Import directives
import {
  AsideToggleDirective,
  NAV_DROPDOWN_DIRECTIVES,
  ReplaceDirective,
  SIDEBAR_TOGGLE_DIRECTIVES
} from "./directives";

const APP_DIRECTIVES = [
  AsideToggleDirective,
  NAV_DROPDOWN_DIRECTIVES,
  ReplaceDirective,
  SIDEBAR_TOGGLE_DIRECTIVES
];

// Import routing module
import { AppRoutingModule } from "./app.routing";

// Import 3rd party components
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { TabsModule } from "ngx-bootstrap/tabs";
import { CommunicationService } from "./service/communication.service";
import { CommonService } from "./service/common.service";
import { HttpStatusService } from "./service/interceptor-service/http-status.service";
import { HttpListenerService } from "./service/interceptor-service/http-listener.service";
import {
  HTTP_INTERCEPTORS,
  HttpClient,
  HttpClientModule
} from "@angular/common/http";
import { CoreModule } from "./core.module";
import { LanguajeService } from "./service/languaje-service/languaje.service";
import { TreeModule } from "angular-tree-component";
import { FormsModule } from "@angular/forms";
import { ModalModule } from "ngx-bootstrap";
import { ModalModule as bsModalModule, ModalService } from "ng-bootstrap-modal";
import { FileUploadModule } from 'primeng/primeng';
import { SimpleLayoutComponent } from "./containers/simple-layout";
import { CreateUserModalComponent } from './utility/create-user-modal/create-user-modal.component';
import { NotificationControlComponent } from './utility/notification-control/notification-control.component';
import { ProviderViewFormComponent } from 'app/views/provider-view/provider-view-form/provider-view-form.component';
import { PayOrderModuleComponent } from './utility/pay-order-module/pay-order-module.component';
import { TableModule } from "primeng/table";

const APP_ENTRY_COMPONENT = [
  CreateUserModalComponent,
  ProviderViewFormComponent,
  PayOrderModuleComponent

];

@NgModule({
  imports: [
    ModalModule.forRoot(),
    FormsModule,
    HttpClientModule,
    CoreModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    BrowserModule,
    RecaptchaModule,
    TableModule,
    FileUploadModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    NgbModule,
    TabsModule.forRoot(),
    TreeModule.forRoot()
  ],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    ...APP_COMPONENTS,
    ...APP_DIRECTIVES,
    NotificationControlComponent,
    
  ],
  entryComponents: APP_ENTRY_COMPONENT,
  providers: [
    TranslatePipe,
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializerFactory,
      deps: [TranslateService, Injector],
      multi: true
    },
    CommunicationService,
    CommonService,
    HttpStatusService,
    HttpListenerService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpListenerService,
      multi: true
    },
    { provide: LocationStrategy, useClass: HashLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
