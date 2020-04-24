import { Component, OnInit, Input, ViewChild, ElementRef } from "@angular/core";

@Component({
  selector: "app-aside",
  templateUrl: "./app-aside.component.html",
  styleUrls: ["./app-aside.component.scss"]
})
export class AppAsideComponent implements OnInit {
  @Input() container: any;
  @Input() mainContainer: any;
  @ViewChild("aside") aside: ElementRef;

  constructor() {}

  ngOnInit() {}
}
