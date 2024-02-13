import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order.component';
import { RouterModule } from '@angular/router';
import { OrderRoutingModule } from './order-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ShopeeOrdersComponent } from './shopee-orders/shopee-orders.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    OrderComponent,
    ShopeeOrdersComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    OrderRoutingModule,
    TabsModule.forRoot(),
    SharedModule
  ],
  exports: [
    OrderComponent
  ]
})
export class OrderModule { }
