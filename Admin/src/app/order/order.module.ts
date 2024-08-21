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
import { ProductSkusComponent } from './add-order/order-information/product-skus/product-skus.component';
import { CheckoutComponent } from './add-order/order-information/checkout/checkout.component';
import { InformationComponent } from './add-order/order-information/information/information.component';
import { CustomerInfoComponent } from './add-order/order-information/customer-info/customer-info.component';
import { ReceiverComponent } from './add-order/order-information/receiver/receiver.component';
import { DeliveryComponent } from './add-order/order-information/delivery/delivery.component';
import { AllOrderComponent } from './order-list/all-order/all-order.component';
import { UpdateOrderComponent } from './update-order/update-order.component';

@NgModule({
  declarations: [
    ShopeeOrdersComponent,
    OrderListComponent,
    AddOrderComponent,
    OrderInformationComponent,
    ProductSkusComponent,
    CheckoutComponent,
    InformationComponent,
    CustomerInfoComponent,
    ReceiverComponent,
    DeliveryComponent,
    AllOrderComponent,
    UpdateOrderComponent
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
