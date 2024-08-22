import { Component, OnInit } from '@angular/core';
import { Customer } from 'src/app/shared/models/cutomer';

@Component({
  selector: 'app-all-customers',
  templateUrl: './all-customers.component.html',
  styleUrls: ['./all-customers.component.css']
})
export class AllCustomersComponent implements OnInit {
  loading: true;
  customers: readonly Customer[] = [];
  // orderParams = new OrderParams();
  totalItems = 0;

  //Order selected
  current = 1;
  checked = false;
  indeterminate = false;
  setOfCheckedId = new Set<number>();
  
  constructor() {}

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  updateCheckedSet(id: number, checked: boolean): void {
    if (checked) {
      this.setOfCheckedId.add(id);
    } else {
      this.setOfCheckedId.delete(id);
    }
  }

  onCurrentPageDataChange(listOfCurrentPageData: readonly Customer[]): void {
    this.customers = listOfCurrentPageData;
    this.refreshCheckedStatus();
  }

  refreshCheckedStatus(): void {
    // const listOfEnabledData = this.listOfCurrentPageData.filter(({ disabled }) => !disabled);
    this.checked = this.customers.every(({ id }) => this.setOfCheckedId.has(id));
    this.indeterminate = this.customers.some(({ id }) => this.setOfCheckedId.has(id)) && !this.checked;
  }

  onItemChecked(id: number, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.refreshCheckedStatus();
  }

  onAllChecked(checked: boolean): void {
    this.customers
      .forEach(({ id }) => this.updateCheckedSet(id, checked));
    this.refreshCheckedStatus();
  }

  onEditCustomer(customer: Customer) {
    // const modal = this.modalServices.create<UpdateOrderComponent, Order>({
    //   nzTitle: '#' + order.id.toString(),
    //   nzContent: UpdateOrderComponent,
    //   nzCentered: true,
    //   nzWidth: '160vh',
    //   nzData: order,
    //   nzBodyStyle: {overflowY : 'scroll', height: '85vh'}
    // });
  }

  onPageChange(pageNumber: number) {
    // this.orderParams.pageIndex = pageNumber;
    // this.getOrders();
  }

  onPageSizeChange(pageSize: number) {
    // this.orderParams.pageSize = pageSize;
    // this.getOrders();
  }
}
