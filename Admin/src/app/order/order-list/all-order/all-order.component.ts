import { Component, Input, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/models/order';
import { OrderParams } from 'src/app/shared/models/orderParams';
import { OrderService } from '../../order.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { UpdateOrderComponent } from '../../update-order/update-order.component';

@Component({
  selector: 'app-all-order',
  templateUrl: './all-order.component.html',
  styleUrls: ['./all-order.component.css']
})

export class AllOrderComponent implements OnInit {
  orderParams = new OrderParams();
  orders: readonly Order[] = [];
  totalItems = 0;

  //Temp dropdown list
  dropdownList = ['Mới', 'Chờ hàng', 'Ưu tiên xuất đơn', 'Xác nhận đơn hàng', 'Gửi hàng đi', 'Huỷ đơn', 'Xoá đơn']

  //Order selected
  current = 1;
  checked = false;
  loading = false;
  indeterminate = false;
  setOfCheckedId = new Set<number>();

  constructor(private orderService: OrderService, private modalServices: NzModalService) {}

  ngOnInit() {
    this.getOrders();
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
    this.orderService.getProducts(this.orderParams).subscribe({
      next: response => {
        this.orders = response.data;
        this.orderParams.pageIndex = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
        this.totalItems = response.count;

        console.log(this.totalItems);
      },
      error: err => {
        console.log(err);
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
}
