import { Component, Input, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/_models/order';
import { OrderParams } from 'src/app/shared/_models/order/orderParams';
import { OrderService } from '../../order.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { UpdateOrderComponent } from '../../update-order/update-order.component';
import { OrderStatus } from 'src/app/shared/_models/orderStatus';
import { UpdateStatusDTO } from 'src/app/shared/_models/order/updateStatusDTO';
import { OrderWithStatusParams } from 'src/app/shared/_models/order/orderWithStatusParams';
import { SearchService } from 'src/app/core/services/search.service';

@Component({
  selector: 'app-all-order',
  templateUrl: './all-order.component.html',
  styleUrls: ['./all-order.component.css']
})

export class AllOrderComponent implements OnInit {
  @Input() orderStatus: number;
  orderParams = new OrderParams();
  orders: readonly Order[] = [];
  totalItems = 0;
  allOrders: readonly Order[] = [];

  orderStatuses: OrderStatus[] = [
    { id: 1, status: 'Mới'},
    { id: 2, status: 'Chờ hàng'},
    { id: 3, status: 'Ưu tiên xuất đơn'},
    { id: 4, status: 'Đã xác nhận'},
    { id: 5, status: 'Gửi hàng đi'},
    { id: 6, status: 'Huỷ đơn'},
    { id: 7, status: 'Xoá đơn'}
  ];

  //Order selected
  current = 1;
  checked = false;
  loading = true;
  indeterminate = false;
  setOfCheckedId = new Set<number>();

  constructor(private orderService: OrderService, private modalServices: NzModalService, private searchService: SearchService) {}

  ngOnInit() {
    if(this.orderStatus == -1) {
      this.getOrders();
    } else {
      this.getOrderByStatus(this.orderStatus);
    }

    // Lắng nghe sự thay đổi từ search box
    this.searchService.searchQuery$.subscribe(query => {
      this.filterOrders(query);
    });
  }

  filterOrders(query: string) {
    if (!query) {
      this.orders = this.allOrders; // Hiển thị toàn bộ nếu không có tìm kiếm
    } else {
      this.orders = this.allOrders.filter(order =>
        order.customer.name.toLowerCase().includes(query.toLowerCase()) ||
        order.id.toString().includes(query) ||
        order.customer.phoneNumber.includes(query) ||
        order.customer.emailAddress.toLowerCase().includes(query.toLowerCase()) ||
        order.receiverName.toLowerCase().includes(query.toLowerCase()) ||
        order.receiverPhoneNumber.includes(query) ||
        order.address.toLowerCase().includes(query.toLowerCase())
      );
    }
  }

  updateCheckedSet(id: number, checked: boolean): void {
    if (checked) {
      this.setOfCheckedId.add(id);
    } else {
      this.setOfCheckedId.delete(id);
    }
  }

  onCurrentPageDataChange(listOfCurrentPageData: readonly Order[]): void {
    this.orders = listOfCurrentPageData;
    this.refreshCheckedStatus();
  }

  refreshCheckedStatus(): void {
    // const listOfEnabledData = this.listOfCurrentPageData.filter(({ disabled }) => !disabled);
    this.checked = this.orders.every(({ id }) => this.setOfCheckedId.has(id));
    this.indeterminate = this.orders.some(({ id }) => this.setOfCheckedId.has(id)) && !this.checked;
  }

  onItemChecked(id: number, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.refreshCheckedStatus();
  }

  onAllChecked(checked: boolean): void {
    this.orders
      .forEach(({ id }) => this.updateCheckedSet(id, checked));
    this.refreshCheckedStatus();
  }

  onPageChange(pageNumber: number) {
    this.orderParams.pageIndex = pageNumber;
    this.getOrders();
  }

  getOrders() {
    this.orderService.getOrders(this.orderParams).subscribe({
      next: response => {
        this.orders = response.data;
        this.allOrders = this.orders;
        this.orderParams.pageIndex = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
        this.totalItems = response.count;
        this.loading = false;
      },
      error: err => {
        console.log(err);
        this.loading = false;
      }
    });
  }

  onPageSizeChange(pageSize: number) {
    this.orderParams.pageSize = pageSize;
    this.getOrders();
  }

  onEditOrder(order: Order) {
    const modal = this.modalServices.create<UpdateOrderComponent, Order>({
      nzTitle: '#' + order.id.toString(),
      nzContent: UpdateOrderComponent,
      nzCentered: true,
      nzWidth: '160vh',
      nzData: order,
      nzBodyStyle: {overflowY : 'scroll', height: '85vh'}
    });
  }

  changeStatus(statusId: number, orderId: number) {
    var updateStatusDTO: UpdateStatusDTO = {
      orderId: orderId,
      statusId: statusId
    };

    this.orderService.updateStatus(updateStatusDTO).subscribe({
      next: (order) => {
        console.log(order);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  getOrderByStatus(statusId: number) {
    var params = new OrderWithStatusParams(statusId);

    this.orderService.getOrdersWithStatus(params).subscribe({
      next: response => {
        this.orders = response.data;
        this.orderParams.pageIndex = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
        this.totalItems = response.count;
        this.loading = false;
      },
      error: err => {
        console.log(err);
        this.loading = false;
      }
    });
  }
}
