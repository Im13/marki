import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderRoutingModule } from './order-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ShopeeOrdersComponent } from './shopee-orders/shopee-orders.component';
import { SharedModule } from '../shared/shared.module';
import { OrderListComponent } from './order-list/order-list.component';
import { CoreModule } from '../core/core.module';
import { AddOrderComponent } from './add-order/add-order.component';
import { OrderInformationComponent } from './add-order/order-information/order-information.component';
import { ProductModule } from '../product/product.module';

@NgModule({
  declarations: [
    ShopeeOrdersComponent,
    OrderListComponent,
    AddOrderComponent,
    OrderInformationComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    OrderRoutingModule,
    TabsModule.forRoot(),
    SharedModule,
    CoreModule,
    ProductModule
  ]
})
export class OrderModule { }
