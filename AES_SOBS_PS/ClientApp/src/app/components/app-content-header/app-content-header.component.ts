import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-content-header",
  template: `
    <div>
      {{ header }}
    </div>
  `
})
export class AppContentHeaderComponent implements OnInit {
  header = "";

  constructor(private route: ActivatedRoute) {
    this.route.data.subscribe(dt => {
      this.header = dt.title;
    });
  }

  ngOnInit() {}
}
