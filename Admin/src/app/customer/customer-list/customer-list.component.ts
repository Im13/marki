import { Component, ViewChild } from '@angular/core';
import { CustomerService } from '../customer.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AllCustomersComponent } from './all-customers/all-customers.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent {
  @ViewChild(AllCustomersComponent) allCustomerChildComp: AllCustomersComponent;
  checkedIds = new Set<number>();

  constructor(private customerService: CustomerService, private modalServices: NzModalService, private toastrService: ToastrService){}

  addCustomer() {}

  checkItem(checkedIds: Set<number>) {
    this.checkedIds = checkedIds;
  }

  deleteCustomers() {
    var ids: number[] = Array.from(this.checkedIds);

    console.log(ids)

    this.modalServices.confirm({
      nzTitle: 'Bạn muốn xoá các khách hàng đang chọn?',
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => {
        this.customerService.deleteCustomers(ids).subscribe({
          next: () => {
            this.toastrService.success('Xoá khách hàng thành công');
            this.allCustomerChildComp.getCustomers();
          },
          error: (err) => {
            console.log(err);
          }
        })
      },
      nzCancelText: 'No'
    });

    
  }
}
