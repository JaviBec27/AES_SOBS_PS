import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientViewComponent } from "./client-view/client-view.component"


const routes: Routes = [
  {
    path: "",
    component: ClientViewComponent,
    data: {
      title: "Provider View"
    }
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientViewRoutingModule { }
