import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProviderViewComponent } from "./provider-view/provider-view.component"

const routes: Routes = [
  {
    path: "",
    component: ProviderViewComponent,
    data: {
      title: "Provider View"
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ProviderViewRoutingModule { }
