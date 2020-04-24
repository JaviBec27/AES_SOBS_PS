import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { jqxGridComponent } from "jqwidgets-scripts/jqwidgets-ts/angular_jqxgrid";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ToastrModule } from "ngx-toastr";
import { TranslateModule, TranslatePipe } from "@ngx-translate/core";


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    ToastrModule.forRoot(),
  ],
  providers: [TranslatePipe],
  exports: [
    jqxGridComponent,
    TranslateModule,
    ReactiveFormsModule,
    TranslatePipe
  ],
  declarations: [
    jqxGridComponent
  ]
})
export class CoreModule {}
