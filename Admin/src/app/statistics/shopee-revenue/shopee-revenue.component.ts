import { Component } from '@angular/core';
import { OrderService } from 'src/app/order/order.service';
import { ShopeeOrder } from 'src/app/shared/models/shopeeOrder';
import { ShopeeOrderParams } from 'src/app/shared/models/shopeeOrderParams';

@Component({
  selector: 'app-shopee-revenue',
  templateUrl: './shopee-revenue.component.html',
  styleUrls: ['./shopee-revenue.component.css']
})
export class ShopeeRevenueComponent {
  today: Date;
  shopeeOrders: ShopeeOrder[] = [];
  orderParams = new ShopeeOrderParams();
  totalCount = 0;

  constructor(private orderService: OrderService) {
    this.today = new Date();
    this.orderParams.date = this.today.toString();

    this.getOrders();
  }

  getOrders() {
    this.orderService.getShopeeOrdersPagination(this.orderParams).subscribe({
      next: response => {
        this.shopeeOrders = response.data;
        this.orderParams.pageIndex = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: err => {
        console.log(err);
      }
    });
  }
}
