import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  HostListener,
  AfterViewInit
} from "@angular/core";
import { CommunicationService } from "app/service/communication.service";

@Component({
  selector: "app-full-layout",
  templateUrl: "./full-layout.component.html",
  styleUrls: ["./full-layout.component.scss"]
})
export class FullLayoutComponent implements OnInit, AfterViewInit {
  @ViewChild("main") main: ElementRef;
  isLoading = false;

  constructor(private commuService: CommunicationService) {}

  @HostListener("window:resize", ["$event"])
  onResize(event) {
    if (this.main) {
      this.sendDivInfo();
    }
  }

  sendDivInfo() {
    const hostElement = this.main.nativeElement;
    const HEIGHT = hostElement.offsetHeight;
    this.commuService.setMainDivInfo(HEIGHT);
  }
  ngOnInit() {}
  ngAfterViewInit() {
    this.sendDivInfo();
  }
}
