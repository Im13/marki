import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderComponent } from './order.component';
import { ShopeeOrdersComponent } from './shopee-orders/shopee-orders.component';

const routes: Routes = [
  { path: '', component: OrderComponent },
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
