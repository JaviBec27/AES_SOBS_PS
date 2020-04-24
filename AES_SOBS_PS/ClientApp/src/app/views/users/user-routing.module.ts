import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UsersComponent } from "./user/users.component"

const routes: Routes = [
  {
    path: "",
    data: {
      title: "User Form"
    },
    children: [
      {
        path: "create-user",
        component: UsersComponent,
        data: {
          title: "Create Page"
        }
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
