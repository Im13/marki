import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewArrivalsComponent } from './new-arrivals.component';
import { NewArrivalsRoutingModule } from './new-arrivals-routing.module';
import { CoreModule } from '../_core/core.module';
import { NewArrivalProductsComponent } from './new-arrival-products/new-arrival-products.component';



@NgModule({
  declarations: [
    NewArrivalsComponent,
    NewArrivalProductsComponent
  ],
  imports: [
    CommonModule,
    NewArrivalsRoutingModule,
    CoreModule
  ]
})
export class NewArrivalsModule { }
