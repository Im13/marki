import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopeeOrdersComponent } from './shopee-orders/shopee-orders.component';
import { OrderListComponent } from './order-list/order-list.component';
import { AddOrderComponent } from './add-order/add-order.component';

const routes: Routes = [
  { path: '', component: OrderListComponent },
  { path: 'add', component: AddOrderComponent },
  { path: 'shopee', component: ShopeeOrdersComponent }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
  ]
})
export class OrderRoutingModule { }
