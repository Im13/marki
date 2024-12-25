import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutComponent } from './checkout.component';
import { CoreModule } from '../_core/core.module';
import { CheckoutRoutingModule } from './checkout-routing.module';

@NgModule({
  declarations: [
    CheckoutComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    CheckoutRoutingModule
  ]
})
export class CheckoutModule { }
