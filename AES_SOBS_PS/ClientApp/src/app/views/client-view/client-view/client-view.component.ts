import { Component, OnInit, ViewChild } from '@angular/core';
import { ClientViewTableComponent} from '../client-view-table/client-view-table.component'
import { CommunicationService } from 'app/service/communication.service';
import { ISubscription } from 'rxjs/Subscription';
import { CategoryService } from 'app/service/category-service/category.service'
import { Category } from 'app/models/Category';

@Component({
  selector: 'app-client-view',
  templateUrl: './client-view.component.html',
  styleUrls: ['./client-view.component.scss']
})
export class ClientViewComponent implements OnInit {
  @ViewChild("productList") productList: ClientViewTableComponent;
  category: Category;
  private subscriptionProductChange: ISubscription;
  constructor(private commuService: CommunicationService,
    private categoryService: CategoryService) { }

  ngOnInit() {
    this.subscribeCategoryChange();
  }

  ngOnDestroy(): void {
    this.subscriptionProductChange.unsubscribe();
  }

  subscribeCategoryChange(): void {

    this.subscriptionProductChange = this.commuService.onCategoryChanged.subscribe(
      pCategory => {
        this.category = pCategory;
        this.productList.getProducts(this.category.idCategoria.toString());

      }
    );
  }

}
