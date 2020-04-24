import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LoginComponent } from "./login/login.component";
import { SecurityService } from "app/service/security-service/security.service";
import { AuthGuardService } from "app/service/auth-guard.service";
import { SharedModule } from "app/shared.module";
import { FormsModule } from "@angular/forms";
import { PagesRoutingModule } from "./pages-routing.module";
import { CoreModule } from "app/core.module";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { RecoverLoginModalComponent } from './login/recover-login-modal/recover-login-modal.component';

@NgModule({
  declarations: [LoginComponent, RecoverLoginModalComponent],
  imports: [CommonModule, SharedModule, FormsModule, PagesRoutingModule,NgbModule,CoreModule],
  providers: [SecurityService, AuthGuardService]
})
export class PagesModule {}
