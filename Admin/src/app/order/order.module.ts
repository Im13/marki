import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order.component';
import { RouterModule } from '@angular/router';
import { OrderRoutingModule } from './order-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ShopeeOrdersComponent } from './shopee-orders/shopee-orders.component';

@NgModule({
  declarations: [
    OrderComponent,
    ShopeeOrdersComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    OrderRoutingModule,
    TabsModule.forRoot()
  ],
  exports: [
    OrderComponent
  ]
})
export class OrderModule { }
