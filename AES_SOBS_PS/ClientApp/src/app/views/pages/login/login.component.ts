import { Component, OnInit } from "@angular/core";
import { environment } from "environments/environment";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { trigger, transition, useAnimation } from "@angular/animations";
import { fadeIn, fadeOut } from "ng-animate";
import { Constants } from "app/models/Constants";
import { HttpErrorResponse } from "@angular/common/http";
import { SecurityService } from "app/service/security-service/security.service";
import { NgbProgressbarConfig } from "@ng-bootstrap/ng-bootstrap";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { RecoverLoginModalComponent } from "./recover-login-modal/recover-login-modal.component"; 
import { CreateUserModalComponent } from "app/utility/create-user-modal/create-user-modal.component"; 


@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
  animations: [
    trigger("bounce", [
      transition("* => *", useAnimation(fadeIn)),
      transition("* => *", useAnimation(fadeOut))
    ])
  ]
})
export class LoginComponent implements OnInit {
  bounce: any;
  version: string = environment.VERSION;
  releaseDate: string = environment.releaseDate;
  showAlert: number = 0;
  begin: any;
  end: any;
  bsModalRef: BsModalRef;
  isLoad = false;
  urlPageLogin = "/pages/login";
  languajes: Array<Object> = [
    { culture: "es-CO", name: "Español" },
    { culture: "en-US", name: "English" }
  ];
  languaje: Object;
  // loginResponse: LoginResponse;
  errorResponse: HttpErrorResponse;
  errorResponseLogin;

  constructor(
    private router: Router,
    private securityService: SecurityService,
    private translate: TranslateService,
    private bsModalService: BsModalService,
    config: NgbProgressbarConfig,

  ) {
    config.max = 1000;
    config.striped = true;
    config.animated = true;
    config.type = "info";
    config.height = "6px";
  }

  onSubmit(username, password) {
    this.isLoad = true;
    this.securityService.userAuthentication(username, password).subscribe(
      res => {
        this.showAlert = 1;
        const json_array = res;
        console.log(json_array);
        if (json_array.status !== 200) {
          console.log(this.showAlert);
          this.router.navigate([this.urlPageLogin]);
        } else {
          this.showAlert = 1;
          const response =json_array.body["usuario"]

          localStorage.setItem(
            Constants.Keys.UserName,
            response[0].nombre
          );
          localStorage.setItem(
            Constants.Keys.idUser,
            response[0].idUsuario
          );
          sessionStorage.setItem(Constants.Keys.Loggedin, "1");
          sessionStorage.setItem(
            Constants.Keys.Token,
            response[0].tokenMedio
          );

          this.router.navigate(["/dashboard"]);
        }
      },
      error => {
        this.errorResponse = error as HttpErrorResponse;
        if (this.errorResponse.status === 0) {
          this.errorResponseLogin = this.translate.instant(
            "lblCommunicationProblems"
          );
        } else {
          if (this.errorResponse.status === 500) {
            this.errorResponseLogin = this.translate.instant("lblServerError");
          } else {
            this.errorResponseLogin = this.translate.instant(
              "lblInvalidUsernamePassword"
            );
          }
        }
        this.showAlert = 2;
        console.log(this.showAlert);
        this.router.navigate([this.urlPageLogin]);
        this.isLoad = false;

      }
    );
  }

  onNewUser():void  {
    this.bsModalRef = this.bsModalService.show(CreateUserModalComponent,
      {ignoreBackdropClick: true,
      keyboard: false});
  }

  onChangeLanguaje(pLang) {
    this.translate.use(pLang);
    localStorage.setItem(Constants.Keys.Lang, pLang);
    console.log(this.languaje);
  }
  selectDefaultLanguaje() {
    const vLanguaje = localStorage.getItem(Constants.Keys.Lang);
    this.languaje = vLanguaje;
  }

  ngOnInit() {
    this.selectDefaultLanguaje();
  }

  recoverPassword() {
    this.bsModalRef = this.bsModalService.show(RecoverLoginModalComponent);
  }
}
