import { Injectable } from "@angular/core";
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/filter";
import "rxjs/add/operator/map";
import { CommonService } from "./common.service";
import { Category } from "app/models/Category";
import { Product } from "app/models/Product";

@Injectable({
  providedIn: "root"
})
export class CommunicationService {
  private mainDiv = new Subject<number>();
  public refreshChar = new Subject<boolean>();
  public onCategoryChanged = new Subject<Category>();
  public onKartProductChanged = new Subject<Product>();


  constructor() {}

  public refreshChartEvent(pIsMax) {
    const vThis = this;
    setTimeout(function() {
      vThis.refreshChar.next(pIsMax);
    }, 200);
  }

  /**
   * for use height and width related info of parent main div
   * @param height height of main div
   */
  setMainDivInfo(height: number) {
    this.mainDiv.next(height);
  }

  public changeKartProduct(pProduct:Product):void{
    this.onKartProductChanged.next(pProduct);
  }
  public changeCategory(pCategory: Category): void {
    this.onCategoryChanged.next(pCategory);
  }
}
