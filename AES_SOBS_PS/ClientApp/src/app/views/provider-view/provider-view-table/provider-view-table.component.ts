import { Component, OnInit } from "@angular/core";
import { SelectItem } from "primeng/api";
import { ConfirmationService } from "primeng/api";
import { Subscription } from "rxjs";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { ProviderViewFormComponent } from "../provider-view-form/provider-view-form.component";
import { Product } from "app/models/Product";
import { ProductService } from "app/service/product-service/product.service";
import { Category } from "app/models/Category";
import { CategoryService } from "app/service/category-service/category.service";

@Component({
  selector: "app-provider-view-table",
  templateUrl: "./provider-view-table.component.html",
  styleUrls: ["./provider-view-table.component.scss"],
})
export class ProviderViewTableComponent implements OnInit {
  lstProducts: any[];
  selectedProduct: any;
  bsModalRef: BsModalRef;
  subscribe: Subscription;
  displayDialog: boolean;
  sortOptions: SelectItem[];
  sortKey: string;
  sortField: string;
  sortOrder: number;
  acceptLabel: string;
  pIdProvider: number;
  pCategory: Category;

  /**
   *Creates an instance of ProviderViewTableComponent.
   * @param {TranslateService} translateService
   * @param {BsModalService} bsModalService
   * @param {ToastrService} toastr
   * @param {ConfirmationService} confirmationService
   * @param {ProductService} productService
   * @memberof ProviderViewTableComponent
   */
  constructor(
    private translateService: TranslateService,
    private bsModalService: BsModalService,
    private toastr: ToastrService,
    private confirmationService: ConfirmationService,
    private productService: ProductService,
    private categoryService: CategoryService
  ) {}

  getProducts(pIdProvider: string, pIdCategory: string) {
    this.categoryService
      .getCategoryProvider(pIdProvider, pIdCategory)
      .subscribe((pProducts) => {
        let product = pProducts as any;
        this.lstProducts = product.producto;
      });
  }

  getProductsByCategory(pCategory: Category, pIdProvider: number): void {
    this.pCategory = pCategory;
    this.pIdProvider = pIdProvider;
    this.getProducts(pIdProvider.toString(), pCategory.idCategoria.toString());
  }

  ngOnInit() {
    
    this.getProducts(
      localStorage.getItem("idUser"),
      localStorage.getItem("idCategoria")
    );

    this.sortOptions = [
      { label: "Newest First", value: "!year" },
      { label: "Oldest First", value: "year" },
      { label: "Brand", value: "brand" },
    ];
  }

  selectProduct(event: Event, product: Product) {
    this.selectedProduct = product;
    this.displayDialog = true;
    event.preventDefault();
  }

  /**
   *
   *
   * @param {Ticket} pTicket
   * @memberof DataEntryRunMeasurementListViewComponent
   */
  confirm(pProduct: Product): void {
    this.confirmationService.confirm({
      message: this.translateService.instant("lblConfirmDeleteMessage") + " ?",
      header: this.translateService.instant("lblConfirmDeleteHeader"),
      icon: "pi pi-info-circle",
      accept: () => {
        this.delete(pProduct);
      },
      reject: () => {},
    });
  }

  /**
   *Method to delete a ticket
   *
   * @param {Ticket} pTicket
   * @memberof DataEntryRunMeasurementListViewComponent
   */
  delete(pProduct: Product): void {
    this.productService.deleteProduct(pProduct).subscribe(
      (arg) => {
        this.getProductsByCategory(this.pCategory, this.pIdProvider);
        this.toastr.success(
          this.translateService.instant("lblSuccess"),
          this.translateService.instant("lblDeleteSuccess")
        );
      },
      (error) => {
        this.toastr.error(
          this.translateService.instant("lblError"),
          error.error.Message
        );
      }
    );
  }

  onSortChange(event) {
    let value = event.value;

    if (value.indexOf("!") === 0) {
      this.sortOrder = -1;
      this.sortField = value.substring(1, value.length);
    } else {
      this.sortOrder = 1;
      this.sortField = value;
    }
  }

  openModal(pProduct?: Product): void {
    const initialState = {
      idUser: this.pIdProvider,
      idCategory: this.pCategory.idCategoria,
      product: pProduct,
    };
    this.bsModalRef = this.bsModalService.show(ProviderViewFormComponent, {
      initialState,
      ignoreBackdropClick: true,
      keyboard: false,
      class: "modal-md",
    });
    this.bsModalRef.content.close.subscribe((pIsSaveOrUpdate) => {
      if (pIsSaveOrUpdate) {
        this.getProductsByCategory(this.pCategory, this.pIdProvider);
      }
    });
  }

  detail(pProduct: Product): void {
    this.openModal(pProduct);
  }

  onDialogHide() {
    this.selectedProduct = null;
  }
}
