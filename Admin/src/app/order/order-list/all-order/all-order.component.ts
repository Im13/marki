import { Component, Input, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/models/order';
import { OrderParams } from 'src/app/shared/models/order/orderParams';
import { OrderService } from '../../order.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { UpdateOrderComponent } from '../../update-order/update-order.component';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { UpdateStatusDTO } from 'src/app/shared/models/order/updateStatusDTO';
import { OrderWithStatusParams } from 'src/app/shared/models/order/orderWithStatusParams';

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

  orderStatuses: OrderStatus[] = [
    { id: 0, status: 'Mới'},
    { id: 1, status: 'Chờ hàng'},
    { id: 2, status: 'Ưu tiên xuất đơn'},
    { id: 3, status: 'Đã xác nhận'},
    { id: 4, status: 'Gửi hàng đi'},
    { id: 5, status: 'Huỷ đơn'},
    { id: 6, status: 'Xoá đơn'}
  ];

  //Order selected
  current = 1;
  checked = false;
  loading = true;
  indeterminate = false;
  setOfCheckedId = new Set<number>();

  constructor(private orderService: OrderService, private modalServices: NzModalService) {}

  ngOnInit() {
    if(this.orderStatus == -1) {
      this.getOrders();
    } else {
      this.getOrderByStatus(this.orderStatus);
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
