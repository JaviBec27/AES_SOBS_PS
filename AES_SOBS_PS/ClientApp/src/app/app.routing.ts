import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

// Import Containers
import { FullLayoutComponent } from "./containers";
import { AuthGuardService } from "./service/auth-guard.service";
import { SimpleLayoutComponent } from "./containers/simple-layout";

export const routes: Routes = [
  {
    path: "pages",
    component: SimpleLayoutComponent,
    data: {
      title: "Pages"
    },
    children: [
      {
        path: "",
        loadChildren: "./views/pages/pages.module#PagesModule"
      }
    ]
  },
  {
    path: "",
    component: FullLayoutComponent,
    canActivate: [AuthGuardService],
    data: {
      title: "Home"
    },
    children: [
      {
        path: "",
        loadChildren: "./views/dashboard/dashboard.module#DashboardModule"
      },
      {
        path: "dashboard",
        loadChildren: "./views/dashboard/dashboard.module#DashboardModule"
      },
      {
        path: "provider-view",
        loadChildren: "./views/provider-view/provider-view.module#ProviderViewModule"
      },
      {
        path: "client-view",
        loadChildren:
          "./views/client-view/client-view.module#ClientViewModule"
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
