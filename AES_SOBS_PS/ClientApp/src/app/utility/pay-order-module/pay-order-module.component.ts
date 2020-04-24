import { Component, OnInit, EventEmitter, Output, ViewChild } from '@angular/core';
import { PayOrderService } from 'app/service/pay-order/pay-order.service';
import { Quotation } from 'app/models/Quotation';
import { BsModalRef } from "ngx-bootstrap";
import { ToastrService } from "ngx-toastr";
import { TranslateService } from "@ngx-translate/core";
import { Product } from 'app/models/Product';
import { Constants } from 'app/models/Constants';
import { Table } from 'primeng/table';
import { CommunicationService } from 'app/service/communication.service';

@Component({
  selector: 'app-pay-order-module',
  templateUrl: './pay-order-module.component.html',
  styleUrls: ['./pay-order-module.component.scss']
})
export class PayOrderModuleComponent implements OnInit {
  lstProducts: Array<Product>;
  cols: any[];
  @Output() close = new EventEmitter();
  quotation: Quotation;
  @ViewChild("dt") table: Table;
  submitted = false;
  lstQuotations: Array<Quotation>=new Array<Quotation>();
  constructor(public bsModalRef: BsModalRef,
              public payOrderService: PayOrderService,
              private toastr: ToastrService,
              private translateService: TranslateService,
              private commuService: CommunicationService,



    ) { }

  ngOnInit() {
    console.log(this.lstProducts)
       this.cols = [

      {
        field: "nombre",
        header: this.translateService.instant("lblName"),
        class: ""
      },
      {
        field: "precio",
        header: this.translateService.instant("lblPrice"),
        class: ""
      },
      {
        field: "cantidad",
        header: this.translateService.instant("lblQuantity"),
        class: "ui-p-5"
      },
      {
        field: "descripcion",
        header: this.translateService.instant("lblDescription"),
        class: ""
      },
      {
        field: "option",
        header: this.translateService.instant("lblOption"),
        class: "optionSelect"
      }

    ];
  
  }

  cancel(): void {
    this.bsModalRef.hide();
    this.close.emit(null);
  }

  insert(): void {
    this.payOrderService.insertQuotation(this.lstQuotations).subscribe(
      (result) => {
        localStorage.removeItem(Constants.Keys.Kart);
        this.commuService.changeKartProduct(null);
        this.toastr.success(this.translateService.instant("lblSuccessQuotation"));
      },
      (err) => {
        this.toastr.error(err.error.Message, "Error");
      }
    );
  }

  deleteProduct(pProduct:Product){

  }

  public submit(): void {
    this.submitted = true;
    this.lstProducts.forEach(element => {
      this.getForm(element);
    });
    this.insert();
    this.bsModalRef.hide();
    this.close.emit(true);
  }

  
  getForm(pProduct: Product): void {
    let quotation= new Quotation()
    quotation.fechaCotizacion=new Date();
    quotation.idUsuario=parseInt(localStorage.getItem(Constants.Keys.idUser));
    quotation.idProducto=pProduct.idProducto;
    quotation.referencia="Asawadada"
    quotation.cantidad=pProduct.cantidad;
    this.lstQuotations.push(quotation)
  }



}
