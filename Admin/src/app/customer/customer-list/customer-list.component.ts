import { Component } from '@angular/core';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent {
  checkedIds = new Set<number>();

  addCustomer() {}

  checkItem(checkedIds: Set<number>) {
    this.checkedIds = checkedIds;
  }
}
