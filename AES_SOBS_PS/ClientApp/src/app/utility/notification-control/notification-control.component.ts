import { Component, OnInit } from "@angular/core";
import { Product } from "app/models/Product";
import { ISubscription } from "rxjs/Subscription";
import { CommunicationService } from "app/service/communication.service";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { PayOrderModuleComponent } from "../pay-order-module/pay-order-module.component";
@Component({
  selector: "app-notification-control",
  templateUrl: "./notification-control.component.html",
  styleUrls: ["./notification-control.component.scss"],
})
export class NotificationControlComponent implements OnInit {
  clicked: boolean = true;
  private subscriptionProductChange: ISubscription;
  carNotifications: Array<Product> = new Array<Product>();
  quantity:number =0;
  bsModalRef: BsModalRef;
  constructor(private commuService: CommunicationService,
    private bsModalService: BsModalService,
    ) {}

  ngOnInit() {
    localStorage.setItem("Kart", JSON.stringify(this.carNotifications));
    this.subscribeProductChange();
  }

  ngOnDestroy(): void {
    this.subscriptionProductChange.unsubscribe();
  }

  calculateQuantity():void{
    this.quantity=null;
    this.carNotifications.forEach(element => {
      if (element.cantidad>0) {
        this.quantity+=element.cantidad;
      }
      
    });
  }

  subscribeProductChange(): void {
    this.subscriptionProductChange = this.commuService.onKartProductChanged.subscribe(
      (pProduct) => {
        localStorage.getItem(JSON.stringify(this.carNotifications));
        const vIsNew = this.carNotifications.find(
          (x) => x.idProducto === pProduct.idProducto
        );
        if (vIsNew === undefined) {
          this.carNotifications.push(pProduct);
        }
        else{
          vIsNew.cantidad+=pProduct.cantidad;
        }
        this.calculateQuantity()
        localStorage.setItem("Kart", JSON.stringify(this.carNotifications));

      }
    );
  }

  openNotification(): void {
    this.clicked = true;
  }

  onNewOrder():void{
    const initialState = {
      lstProducts: this.carNotifications,
    };
    this.bsModalRef = this.bsModalService.show(PayOrderModuleComponent,
      {initialState,
      ignoreBackdropClick: true,
      keyboard: false});
  }
}
