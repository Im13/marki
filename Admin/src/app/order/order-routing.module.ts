import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopeeOrdersComponent } from './shopee-orders/shopee-orders.component';
import { AddOrderComponent } from './add-order/add-order.component';
import { WebOrdersComponent } from './web-orders/web-orders.component';

const routes: Routes = [
  { path: '', component: WebOrdersComponent, data: { title: 'Đơn hàng' } },
  { path: 'add', component: AddOrderComponent, data: { title: 'Tạo đơn' } },
  { path: 'shopee', component: ShopeeOrdersComponent, data: { title: 'Đơn Shopee' } }
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
