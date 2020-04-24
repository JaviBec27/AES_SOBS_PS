import { Component, OnInit } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CommunicationService } from "app/service/communication.service";
import { Category } from 'app/models/Category';
import { CategoryService } from "app/service/category-service/category.service";
import { Product } from 'app/models/Product';
import { Router } from '@angular/router';



@Component({
  selector: 'app-client-view-table',
  templateUrl: './client-view-table.component.html',
  styleUrls: ['./client-view-table.component.scss']
})
export class ClientViewTableComponent implements OnInit {
  lstProducts: any[];
  selectedProduct: any;
  subscribe: Subscription;
  displayDialog: boolean;
  sortOptions: SelectItem[];
  sortKey: string;
  sortField: string;
  sortOrder: number;
  acceptLabel:string;
  pCategory: Category;
  quantity: number = 1;



  /**
   *Creates an instance of ClientViewTableComponent.
   * @param {TranslateService} translateService
   * @param {ToastrService} toastr
   * @param {ConfirmationService} confirmationService
   * @param {ProductService} productService
   * @param {CategoryService} categoryService
   * @memberof ClientViewTableComponent
   */
  constructor(
    private translateService: TranslateService,
    private toastr: ToastrService,
    private categoryService: CategoryService,
    private commuService: CommunicationService,
    private router: Router
  ) { }



  ngOnInit() {

    this.getProducts(
      localStorage.getItem("idCategoria")
    );
      this.sortOptions = [
          {label: 'Newest First', value: '!year'},
          {label: 'Oldest First', value: 'year'},
          {label: 'Brand', value: 'brand'}
      ];
  }


  
  public submit(): void {
    this.selectedProduct.cantidad=this.quantity;
    this.commuService.changeKartProduct(this.selectedProduct);
    this.toastr.success(this.translateService.instant("lblSuccess"));
    this.router
    .navigateByUrl("/", { skipLocationChange: true })
    .then(() => this.router.navigate(["/client-view"]));
  }
  

  selectProduct(event: Event, car: any) {
      this.selectedProduct = car;
      this.displayDialog = true;
      event.preventDefault();
  }

  getProductsByCategory(pCategory: Category): void {
    this.pCategory = pCategory;
    this.getProducts( pCategory.idCategoria.toString());
  }
  getProducts(pIdCategory: string) {
    this.categoryService
    .getCategory(parseInt(pIdCategory))
    .subscribe((pProducts) => {
      let product = pProducts as any;
      this.lstProducts = product.producto;
    });  }

  onSortChange(event) {
      let value = event.value;

      if (value.indexOf('!') === 0) {
          this.sortOrder = -1;
          this.sortField = value.substring(1, value.length);
      }
      else {
          this.sortOrder = 1;
          this.sortField = value;
      }
  }

  

  onDialogHide() {
      this.selectedProduct = null;
  }


}
