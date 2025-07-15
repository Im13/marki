import { Component, Input } from '@angular/core';
import { OrderService } from '../../order.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { OrderStatus } from 'src/app/shared/_models/orderStatus';
import { OrderParams } from 'src/app/shared/_models/order/orderParams';
import { EditOrderModalComponent } from '../edit-order-modal/edit-order-modal.component';
import { UpdateStatusDTO } from 'src/app/shared/_models/order/updateStatusDTO';
import { ToastrService } from 'ngx-toastr';
import { SearchService } from 'src/app/core/services/search.service';
import { WebsiteOrder } from 'src/app/shared/_models/website-order';
import { OrderWithStatusParams } from 'src/app/shared/_models/order/orderWithStatusParams';

@Component({
  selector: 'app-all-site-orders',
  templateUrl: './all-site-orders.component.html',
  styleUrls: ['./all-site-orders.component.css'],
})
export class AllSiteOrdersComponent {
  @Input() orderStatus: number;
  orderParams = new OrderParams();
  orders: readonly WebsiteOrder[] = [];
  allOrders: readonly WebsiteOrder[] = [];
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
    private modalServices: NzModalService,
    private toastrService: ToastrService,
    private searchService: SearchService
  ) {}

  ngOnInit() {
    if(this.orderStatus == -1) {
      this.getOrders();
    } else {
      this.getOrderByStatus(this.orderStatus);
    }

    this.searchService.searchQuery$.subscribe(query => {
      this.filterOrders(query);
    });
  }

  filterOrders(query: string) {
    if (!query) {
      this.orders = this.allOrders; // Hiển thị toàn bộ nếu không có tìm kiếm
    } else {
      this.orders = this.allOrders.filter(order =>
        order.fullname?.toLowerCase().includes(query.toLowerCase()) ||
        order.phoneNumber?.includes(query) ||
        order.buyerEmail?.toLowerCase().includes(query.toLowerCase())
      );
    }
  }

  getOrders() {
    this.orderService.getWebsiteOrders(this.orderParams).subscribe({
      next: (response) => {
        this.orders = response.data;
        this.allOrders = this.orders;
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
    const modalRef = this.modalServices.create<EditOrderModalComponent, WebsiteOrder>({
      nzTitle: '#' + order.id.toString(),
      nzContent: EditOrderModalComponent,
      nzCentered: true,
      nzWidth: '160vh',
      nzData: order,
      nzBodyStyle: { overflowY: 'scroll', height: '86vh', padding: '0px' },
    });

    modalRef.afterClose.subscribe(() => {
      this.getOrders();
    });
  }

  changeStatus(statusId: number, orderId: number) {
    var updateStatusDTO: UpdateStatusDTO = {
      orderId: orderId,
      statusId: statusId
    };

    this.orderService.updateWebsiteOrderStatus(updateStatusDTO).subscribe({
      next: (order) => {
        console.log(order);
        this.toastrService.success('Cập nhật trạng thái đơn hàng thành công');
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  getOrderByStatus(statusId: number) {
    var params = new OrderWithStatusParams(statusId);

    this.orderService.getWebsiteOrders(params).subscribe({
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
