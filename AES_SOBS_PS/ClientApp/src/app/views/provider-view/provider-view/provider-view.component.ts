import { Component, OnInit, ViewChild } from "@angular/core";
import { ProviderViewTableComponent } from "../provider-view-table/provider-view-table.component";
import { CommunicationService } from "app/service/communication.service";
import { ISubscription } from "rxjs/Subscription";
import { Constants } from "app/models/Constants";
import { Category } from "app/models/Category";

@Component({
  selector: "app-provider-view",
  templateUrl: "./provider-view.component.html",
  styleUrls: ["./provider-view.component.scss"],
})
export class ProviderViewComponent implements OnInit {
  @ViewChild("productList") productList: ProviderViewTableComponent;
  private subscriptionProductChange: ISubscription;
  category: Category;
  idProvider = parseInt(localStorage.getItem(Constants.Keys.idUser));
  idCategory = parseInt(localStorage.getItem(Constants.Keys.idCategoria));
  constructor(private commuService: CommunicationService) {}

  ngOnInit() {
    this.subscribeCategoryChange();
  }

  ngOnDestroy(): void {
    this.subscriptionProductChange.unsubscribe();
  }

  subscribeCategoryChange(): void {
    this.subscriptionProductChange = this.commuService.onCategoryChanged.subscribe(
      (pCategory) => {
        this.category = pCategory;
        this.productList.getProductsByCategory(this.category, this.idProvider);
      }
    );
  }

  add(): void {
    this.productList.openModal(null);
  }
}
