import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { ShopeeOrder } from 'src/app/shared/models/shopeeOrder';
import { ShopeeOrderParams } from 'src/app/shared/models/shopeeOrderParams';

@Component({
  selector: 'app-shopee-orders',
  templateUrl: './shopee-orders.component.html',
  styleUrls: ['./shopee-orders.component.css']
})
export class ShopeeOrdersComponent implements OnInit {
  fileName = '';
  shopeeOrders: ShopeeOrder[] = [];
  orderParams = new ShopeeOrderParams();
  totalCount = 0;

  ngOnInit(): void {
    this.getOrders();
  }

  constructor(private orderService: OrderService) {}

  onFileSelected(event) {
    var orders: ShopeeOrder[];
    const file: File = event.target.files[0];

    if(file) {
      orders = this.orderService.readExcelFile(file);

      if(orders) {
        this.getOrders();
      }
    }
  }

  onPageChanged(event: any) {
    if(this.orderParams.pageIndex !== event.page) {
      this.orderParams.pageIndex = event.page;
      this.getOrders();
    }
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
