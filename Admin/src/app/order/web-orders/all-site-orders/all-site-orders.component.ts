import { Component, Input } from '@angular/core';
import { OrderService } from '../../order.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { Order } from 'src/app/shared/_models/order';
import { OrderStatus } from 'src/app/shared/_models/orderStatus';
import { OrderParams } from 'src/app/shared/_models/order/orderParams';
import { WebsiteOrder } from 'src/app/shared/_models/website-order';
import { UpdateOrderComponent } from '../../update-order/update-order.component';
import { EditOrderModalComponent } from '../edit-order-modal/edit-order-modal.component';

@Component({
  selector: 'app-all-site-orders',
  templateUrl: './all-site-orders.component.html',
  styleUrls: ['./all-site-orders.component.css'],
})
export class AllSiteOrdersComponent {
  @Input() orderStatus: number;
  orderParams = new OrderParams();
  orders: readonly WebsiteOrder[] = [];
  totalItems = 0;

  orderStatuses: OrderStatus[] = [
    { id: 1, status: 'Mới' },
    { id: 2, status: 'Chờ hàng' },
    { id: 3, status: 'Ưu tiên xuất đơn' },
    { id: 4, status: 'Đã xác nhận' },
    { id: 5, status: 'Gửi hàng đi' },
    { id: 6, status: 'Huỷ đơn' },
    { id: 7, status: 'Xoá đơn' },
  ];

  //Order selected
  current = 1;
  checked = false;
  loading = true;
  indeterminate = false;
  setOfCheckedId = new Set<number>();

  constructor(
    private orderService: OrderService,
    private modalServices: NzModalService
  ) {}

  ngOnInit() {
    this.getOrders();
  }

  getOrders() {
    this.orderService.getWebsiteOrders(this.orderParams).subscribe({
      next: (response) => {
        this.orders = response.data;
        this.orderParams.pageIndex = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
        this.totalItems = response.count;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
      },
    });
  }

  updateCheckedSet(id: number, checked: boolean): void {}

  onCurrentPageDataChange(
    listOfCurrentPageData: readonly WebsiteOrder[]
  ): void {
    // this.orders = listOfCurrentPageData;
    this.refreshCheckedStatus();
  }

  refreshCheckedStatus(): void {
    // const listOfEnabledData = this.listOfCurrentPageData.filter(({ disabled }) => !disabled);
    this.checked = this.orders.every(({ id }) => this.setOfCheckedId.has(id));
    this.indeterminate =
      this.orders.some(({ id }) => this.setOfCheckedId.has(id)) &&
      !this.checked;
  }

  onItemChecked(id: number, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.refreshCheckedStatus();
  }

  onAllChecked(checked: boolean): void {
    this.orders.forEach(({ id }) => this.updateCheckedSet(id, checked));
    this.refreshCheckedStatus();
  }

  onPageChange(pageNumber: number) {
    this.orderParams.pageIndex = pageNumber;
    this.getOrders();
  }

  onPageSizeChange(pageSize: number) {
    this.orderParams.pageSize = pageSize;
    this.getOrders();
  }

  onEditOrder(order: WebsiteOrder) {
    const modal = this.modalServices.create<EditOrderModalComponent, WebsiteOrder>({
      nzTitle: '#' + order.id.toString(),
      nzContent: EditOrderModalComponent,
      nzCentered: true,
      nzWidth: '160vh',
      nzData: order,
      nzBodyStyle: { overflowY: 'scroll', height: '85vh' },
    });
  }

  changeStatus(statusId: number, orderId: number) {}

  getOrderByStatus(statusId: number) {}
}
