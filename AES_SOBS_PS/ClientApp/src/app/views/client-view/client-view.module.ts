import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from "app/shared.module";
import { ClientViewTableComponent } from './client-view-table/client-view-table.component';
import { ClientViewRoutingModule } from "./client-view-routing.module";
import { ClientViewComponent } from "./client-view/client-view.component";
import { DropdownModule } from "primeng/primeng";
import { CoreModule } from "app/core.module";
import { DataViewModule } from "primeng/dataview";
import { PanelModule } from "primeng/panel";
import { InputTextModule } from "primeng/inputtext";
import { ButtonModule } from "primeng/button";
import { DialogModule } from "primeng/dialog";
import { TabViewModule } from "primeng/tabview";
import { ConfirmationService } from "primeng/api";
import { ConfirmDialogModule } from "primeng/primeng"
import { SpinnerModule } from 'primeng/spinner';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ ClientViewComponent,ClientViewTableComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    ClientViewRoutingModule,
    CoreModule,
    DataViewModule,
    PanelModule,
    InputTextModule,
    ButtonModule,
    DialogModule,
    TabViewModule,
    DropdownModule,
    SpinnerModule,
    ConfirmDialogModule
    
  ],
  providers: [ConfirmationService],

})
export class ClientViewModule { }
