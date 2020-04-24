import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { CategoryService } from "app/service/category-service/category.service";
import { ToastrService } from "ngx-toastr";
import { Category } from "app/models/Category";
import { CommunicationService } from "app/service/communication.service";

@Component({
  selector: "app-header",
  templateUrl: "./app-header.component.html",
  styleUrls: ["./app-header.component.scss"],
})
export class AppHeaderComponent implements OnInit {
  categories: Array<Category>;
  selectedCategory: Category;

  constructor(
    private router: Router,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private commuService: CommunicationService
  ) {}

  ngOnInit() {
    this.getCategories();
  }

  getCategories() {
    this.categoryService.getAllCategories().subscribe(
      (result) => {
        this.categories = result as Array<Category>;
        this.selectedCategory=this.categories[0];
        localStorage.setItem("idCategoria", this.selectedCategory.idCategoria.toString())
      },
      (err) => {
        this.toastr.error(err.error.Message, "Error");
      }
    );
  }

  OnCategoryChange( ) {
    this.commuService.changeCategory(this.selectedCategory);
    localStorage.setItem("idCategoria", this.selectedCategory.idCategoria.toString())

  }

  logout($event) {
    this.router.navigate(["/pages/login"]);
    sessionStorage.clear();
  }
}
