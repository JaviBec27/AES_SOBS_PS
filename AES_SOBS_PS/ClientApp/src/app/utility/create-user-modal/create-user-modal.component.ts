import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { BsModalRef } from "ngx-bootstrap";
import { ToastrService } from "ngx-toastr";
import { UserService } from "app/service/user-service/user.service";
import { User } from "app/models/User";
import { SecurityService } from "app/service/security-service/security.service";
@Component({
  selector: "app-create-user-modal",
  templateUrl: "./create-user-modal.component.html",
  styleUrls: ["./create-user-modal.component.scss"],
})
export class CreateUserModalComponent implements OnInit {
  @Output() close = new EventEmitter();
  submitted = false;
  frmTicket: FormGroup;
  lstUser: Array<any> = [
    { name: this.translateService.instant("lblProvider"), value: 1 },
    { name: this.translateService.instant("lblClient"), value: 2 },
  ];
  user: User = new User();
  // user:User;
  constructor(
    private formBuilder: FormBuilder,
    public bsModalRef2: BsModalRef,
    private translateService: TranslateService,
    private toastr: ToastrService,
    private userService: UserService,
    private securityService: SecurityService
  ) {}

  ngOnInit() {
    this.loadContent();
  }

  /**
   *Load the initial form
   *
   * @memberof DataEntryMeasurementFormViewComponent
   */
  loadContent(): void {
    this.frmTicket = this.formBuilder.group({
      txtUser: [null, [Validators.required]],
      txtPassword: [null, [Validators.required]],
      txtUserType: [null, [Validators.required]],
      txtContact: [null, [Validators.required]],
      txtQuotationSystem: [false],
      txtMail: [
        null,
        [Validators.required, Validators.maxLength(60), Validators.email],
      ],
      txtCaptcha: [null, [Validators.required]],
    });

    // this.onFormChanges();
  }

  cancel(): void {
    this.bsModalRef2.hide();
    this.close.emit(null);
  }

  insert(): void {
    this.userService.insertUser(this.user).subscribe(
      (result) => {
        this.toastr.success(this.translateService.instant("lblSuccess"));
      },
      (err) => {
        this.toastr.error(err.error.Message, "Error");
      }
    );
  }

  public submit(): void {
    this.submitted = true;
    this.frmTicket.updateValueAndValidity();
    if (this.frmTicket.invalid) {
      return;
    }
    this.getForm();
    this.insert();
    this.bsModalRef2.hide();
    this.close.emit(true);
  }

  resolved(event: any): void {
    this.frmTicket.get("txtCaptcha").setValue("VALID");
  }

  getForm(): void {
    this.user.Nombre = this.frmTicket.get("txtUser").value;
    this.user.Email = this.frmTicket.get("txtMail").value;
    this.user.Identificacion = 123;
    this.user.Password = this.securityService.transformPassword(
      this.frmTicket.get("txtPassword").value
    );
    this.user.SistemaDeCotizacion = this.frmTicket.get("txtQuotationSystem").value;
    this.user.Contacto = this.frmTicket.get("txtContact").value;
    this.user.IdRol = this.frmTicket.get("txtUserType").value;
    this.user.Usuario = this.frmTicket.get("txtUser").value;
  }
}
