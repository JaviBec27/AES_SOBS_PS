import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Product } from "app/models/Product";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap";
import { ToastrService } from "ngx-toastr";
import { TranslateService } from "@ngx-translate/core";
import { ProductService } from "app/service/product-service/product.service";
import { Category } from "app/models/Category";

@Component({
  selector: "app-provider-view-form",
  templateUrl: "./provider-view-form.component.html",
  styleUrls: ["./provider-view-form.component.scss"],
})
export class ProviderViewFormComponent implements OnInit {
  product: Product;
  idUser:number;
  idCategory:number;
  frmTicket: FormGroup;
  isNew = false;
  submitted = false;
  @Output() close = new EventEmitter();
  isGroup: boolean;
  uploadedFiles: any[] = [];
  lstTypeProducts: Array<any> = [
    { name: this.translateService.instant("lblService"), value: false },
    { name: this.translateService.instant("lblProduct"), value: true },
  ];
  constructor(
    private formBuilder: FormBuilder,
    private translateService: TranslateService,
    public bsModalRef: BsModalRef,
    private toastr: ToastrService,
    private productService: ProductService
  ) {
    this.loadContent();
  }

  ngOnInit() {
    this.loadObject();
  }

  onUpload(event) {
    for (let file of event.files) {
      this.uploadedFiles.push(file);
    }
    this.frmTicket.get("txtImg").setValue("VALID");
  }

  loadContent(): void {
    this.frmTicket = this.formBuilder.group({
      txtName: [null, [Validators.required]],
      txtPrice: [null, [Validators.required]],
      txtImg: [null],
      txtQuantity: [null, [Validators.required]],
      txtTypeProduct: [null, [Validators.required]],
      txtDescription: [null, [Validators.required]],
      txtReference: [null, [Validators.required]],
    });
  }

  loadObject(): void {
    if (this.product) {
      this.setProduct(this.product);
      this.isNew = false;
    } else {
      this.isNew = true;
      this.product = new Product();
    }
  }

  setProduct(pProduct: Product) {
    this.frmTicket.get("txtName").setValue(pProduct.nombre);
    this.frmTicket.get("txtPrice").setValue(pProduct.precio);
    this.frmTicket.get("txtQuantity").setValue(pProduct.cantidad);
    this.frmTicket.get("txtImg").setValue(pProduct.imagen);
    this.frmTicket.get("txtTypeProduct").setValue(pProduct.tipoProducto);
    this.frmTicket.get("txtDescription").setValue(pProduct.descripcion);
    this.frmTicket.get("txtReference").setValue(pProduct.referenciaProductoProveedor);

  }

  cancel(): void {
    this.bsModalRef.hide();
    this.close.emit(null);
  }

  insert(): void {
    this.productService.insertProduct(this.product).subscribe(
      (result) => {
        this.toastr.success(this.translateService.instant("lblSuccess"));
        this.closeOnUpdateOrSave();
      },
      (err) => {
        this.toastr.error(err.error.Message, "Error");
      }
    );
  }

  validateProductCategory(): void {
    if (this.isNew) {
      this.isGroup = true;
    }
    else {
      this.frmTicket.removeControl("txtImg");
    }
  }

  public submit(): void {
    this.submitted = true;
    this.frmTicket.updateValueAndValidity();
    if (this.frmTicket.invalid) {
      return;
    }
    this.getForm();
    if (this.isNew) {
      this.insert();
    } else {
      this.update();
    }
    this.bsModalRef.hide();
  }

  closeOnUpdateOrSave(): void {
    this.bsModalRef.hide();
    this.close.emit(true);
  }

  update() {
    this.productService.UpdateProduct(this.product).subscribe(
      (result) => {
        this.toastr.success(this.translateService.instant("lblSuccess"));
        this.closeOnUpdateOrSave();
      },
      (err) => {
        this.toastr.error(err.error.Message, "Error");
      }
    );
  }

  getForm(): void {
    this.product.nombre = this.frmTicket.get("txtName").value;
    this.product.precio = this.frmTicket.get("txtPrice").value;
    this.product.cantidad = this.frmTicket.get("txtQuantity").value;
    this.product.imagen = this.frmTicket.get("txtImg").value;
    this.product.tipoProducto = this.frmTicket.get("txtTypeProduct").value;
    this.product.descripcion = this.frmTicket.get("txtDescription").value;
    this.product.referenciaProductoProveedor = this.frmTicket.get("txtReference").value;
    this.product.fechaCreacion = new Date();
    this.product.idCategoria = this.idCategory;
    this.product.idUsuario = this.idUser;
  }
}
