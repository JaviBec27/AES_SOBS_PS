import { Component, OnInit } from "@angular/core";
import { HttpStatusService } from "app/service/interceptor-service/http-status.service";

@Component({
  selector: "app-loading",
  templateUrl: "./app-loading.component.html",
  styleUrls: ["./app-loading.component.scss"]
})
export class AppLoadingComponent implements OnInit {
  HTTPActivity: boolean;
  constructor(private httpStatus: HttpStatusService) {
    this.httpStatus.getHttpStatus().subscribe((status: boolean) => {
      this.HTTPActivity = status;
    });
  }

  ngOnInit() {}
}
