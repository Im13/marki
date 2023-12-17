import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order.component';
import { RouterModule } from '@angular/router';
import { OrderRoutingModule } from './order-routing.module';

@NgModule({
  declarations: [
    OrderComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    OrderRoutingModule
  ],
  exports: [
    OrderComponent
  ]
})
export class OrderModule { }
