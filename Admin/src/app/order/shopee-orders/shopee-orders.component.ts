import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { ShopeeOrder } from 'src/app/_shared/_models/shopeeOrder';
import { ShopeeOrderParams } from 'src/app/_shared/_models/shopeeOrderParams';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { SearchService } from 'src/app/_core/_services/search.service';

@Component({
  selector: 'app-shopee-orders',
  templateUrl: './shopee-orders.component.html',
  styleUrls: ['./shopee-orders.component.css']
})
export class ShopeeOrdersComponent implements OnInit, OnDestroy {
  fileName = '';
  shopeeOrders: ShopeeOrder[] = [];
  allOrders: ShopeeOrder[] = [];
  orderParams = new ShopeeOrderParams();
  totalCount = 0;
  loading = false;

  private orderSubscription: Subscription;

  constructor(private orderService: OrderService,
    private toastrService: ToastrService,
    private searchService: SearchService) {}

  ngOnInit(): void {
    this.getOrders();

    // Lắng nghe sự thay đổi từ search box
    this.searchService.searchQuery$.subscribe(query => {
      this.filterOrders(query);
    });

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

  filterOrders(query: string) {
    if (!query) {
      this.shopeeOrders = this.allOrders; // Hiển thị toàn bộ nếu không có tìm kiếm
    } else {
      this.shopeeOrders = this.allOrders.filter(order =>
        order.customerUsername.toLowerCase().includes(query.toLowerCase()) ||
        order.orderId.toString().includes(query) ||
        order.phoneNumber.includes(query) ||
        order.shipmentCode.toLowerCase().includes(query.toLowerCase()) 
      );
    }
  }

  onFileSelected(event) {
    this.loading = true;
    var orders: ShopeeOrder[];
    const file: File = event.target.files[0];

    if(file) {
      orders = this.orderService.readExcelFile(file);
      this.loading = false;
    }
  }

  onPageChanged(event: any) {
    if(this.orderParams.pageIndex !== event.page) {
      this.orderParams.pageIndex = event.page;
      this.getOrders();
    }
  }

  getOrders() {
    this.loading = true;
    this.orderService.getShopeeOrdersPagination(this.orderParams).subscribe({
      next: response => {
        this.shopeeOrders = response.data;
        this.allOrders = response.data;
        this.orderParams.pageIndex = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
        this.totalCount = response.count;
        this.loading = false;
      },
      error: err => {
        this.loading = false;
        console.log(err);
      }
    });
  }

  onPageChange(pageNumber: number) {
    this.orderParams.pageIndex = pageNumber;
    this.getOrders();
  }

  onPageSizeChange(pageSize: number) {
    this.orderParams.pageSize = pageSize;
    this.getOrders();
  }
}
