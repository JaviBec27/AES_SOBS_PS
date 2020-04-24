import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "app/shared.module";
import { ProviderViewRoutingModule } from "./provider-view-routing.module";
import { ProviderViewComponent } from "./provider-view/provider-view.component";
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
import { ProviderViewTableComponent } from "./provider-view-table/provider-view-table.component";


@NgModule({
  declarations: [ProviderViewComponent, ProviderViewTableComponent],
  imports: [
    CommonModule,
    SharedModule,
    ProviderViewRoutingModule,
    CoreModule,
    DataViewModule,
    PanelModule,
    InputTextModule,
    ButtonModule,
    DialogModule,
    TabViewModule,
    DropdownModule,
    ConfirmDialogModule  ],
  providers: [ConfirmationService],
})
export class ProviderViewModule {}
