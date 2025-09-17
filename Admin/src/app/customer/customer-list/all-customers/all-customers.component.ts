import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Customer } from 'src/app/shared/_models/customer';
import { CustomerService } from '../../customer.service';
import { CustomerParams } from 'src/app/shared/_models/customer/customerParams';
import { SearchService } from 'src/app/core/_services/search.service';

@Component({
  selector: 'app-all-customers',
  templateUrl: './all-customers.component.html',
  styleUrls: ['./all-customers.component.css']
})
export class AllCustomersComponent implements OnInit {
  @Output() checkId = new EventEmitter<Set<number>>();

  loading = true;
  customers: readonly Customer[] = [];
  allCustomers: readonly Customer[] = [];
  customerParams = new CustomerParams();
  totalItems = 0;

  //Order selected
  current = 1;
  checked = false;
  indeterminate = false;
  setOfCheckedId = new Set<number>();

  constructor(private customerService: CustomerService, private searchService: SearchService) {}

  public get selectedCustomers(): Customer[] {
      return this.allCustomers.filter(customer => this.setOfCheckedId.has(customer.id));
    }

  ngOnInit(): void {
    this.getCustomers();

    // Lắng nghe sự thay đổi từ search box
    this.searchService.searchQuery$.subscribe(query => {
      this.filterOrders(query);
    });
  }

  filterOrders(query: string) {
    if (!query) {
      this.customers = this.allCustomers; // Hiển thị toàn bộ nếu không có tìm kiếm
    } else {
      this.customers = this.allCustomers.filter(customer =>
        customer.name.toLowerCase().includes(query.toLowerCase()) ||
        customer.phoneNumber.includes(query) ||
        customer.emailAddress.toLowerCase().includes(query.toLowerCase())
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

  onCurrentPageDataChange(listOfCurrentPageData: readonly Customer[]): void {
    this.customers = listOfCurrentPageData;
    this.refreshCheckedStatus();
  }

  refreshCheckedStatus(): void {
    this.checked = this.customers.every(({ id }) => this.setOfCheckedId.has(id));
    this.indeterminate = this.customers.some(({ id }) => this.setOfCheckedId.has(id)) && !this.checked;
  }

  onItemChecked(id: number, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.checkId.emit(this.setOfCheckedId);
    this.refreshCheckedStatus();
  }

  onAllChecked(checked: boolean): void {
    this.customers
      .forEach(({ id }) => this.updateCheckedSet(id, checked));
    this.checkId.emit(this.setOfCheckedId);
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
    this.customerParams.pageIndex = pageNumber;
    this.getCustomers();
  }

  onPageSizeChange(pageSize: number) {
    this.customerParams.pageSize = pageSize;
    this.getCustomers();
  }

  getCustomers() {
    this.loading = true;

    this.customerService.getCustomers(this.customerParams).subscribe({
      next: response => {
        this.customers = response.data;
        this.allCustomers = this.customers;
        this.customerParams.pageIndex = response.pageIndex;
        this.customerParams.pageSize = response.pageSize;
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
