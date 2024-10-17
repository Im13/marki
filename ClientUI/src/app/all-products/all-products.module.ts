import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllProductsComponent } from './all-products.component';
import { CoreModule } from '../_core/core.module';
import { AllProductsRoutingModule } from './all-products-routing.module';

@NgModule({
  declarations: [
    AllProductsComponent
  ],
  imports: [
    CommonModule,
    AllProductsRoutingModule,
    CoreModule
  ]
})
export class AllProductsModule { }
