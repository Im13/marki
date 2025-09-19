import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { RouterModule } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { SharedModule } from '../_shared/shared.module';
import { CustomerRoutingModule } from './customer-routing.module';
import { AllCustomersComponent } from './customer-list/all-customers/all-customers.component';
import { CoreModule } from '../_core/core.module';

@NgModule({
  declarations: [
    CustomerListComponent,
    AllCustomersComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    CustomerRoutingModule,
    TabsModule.forRoot(),
    SharedModule,
    CoreModule
  ]
})
export class CustomerModule { }
