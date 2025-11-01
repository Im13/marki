import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListingComponent } from './product-listing.component';
import { ProductListingRoutingModule } from './product-listing-routing.module';
import { CoreModule } from '../_core/core.module';
import { ProductItemsComponent } from './product-items/product-items.component';

@NgModule({
  declarations: [
    ProductListingComponent,
    ProductItemsComponent
  ],
  imports: [
    CommonModule,
    ProductListingRoutingModule,
    CoreModule
  ]
})
export class ProductListingModule { }
