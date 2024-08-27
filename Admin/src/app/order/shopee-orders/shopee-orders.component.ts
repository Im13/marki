import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { ShopeeOrder } from 'src/app/shared/_models/shopeeOrder';
import { ShopeeOrderParams } from 'src/app/shared/_models/shopeeOrderParams';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-shopee-orders',
  templateUrl: './shopee-orders.component.html',
  styleUrls: ['./shopee-orders.component.css']
})
export class ShopeeOrdersComponent implements OnInit, OnDestroy {
  fileName = '';
  shopeeOrders: ShopeeOrder[] = [];
  orderParams = new ShopeeOrderParams();
  totalCount = 0;

  private orderSubscription: Subscription;
  
  constructor(private orderService: OrderService, private toastrService: ToastrService) {}

  ngOnInit(): void {
    this.getOrders();

    this.orderSubscription = this.orderService.addedOrders.subscribe(
      (shopeeOrders: ShopeeOrder[]) => {
        if(shopeeOrders !== null) {
          this.getOrders();
          this.toastrService.success('Cập nhật đơn Shopee thành công!');
        } else {
          this.toastrService.error('Lỗi! Không có đơn hàng nào được cập nhật.')
        }
      }
    );
  }

  ngOnDestroy(): void {
    this.orderSubscription.unsubscribe();
  }

  onFileSelected(event) {
    var orders: ShopeeOrder[];
    const file: File = event.target.files[0];

    if(file) {
      orders = this.orderService.readExcelFile(file);
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
