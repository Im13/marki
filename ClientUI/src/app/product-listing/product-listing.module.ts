import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListingComponent } from './product-listing.component';
import { ProductListingRoutingModule } from './product-listing-routing.module';
import { CoreModule } from '../_core/core.module';
import { CollectionProductsComponent } from './collection-products/collection-products.component';

@NgModule({
  declarations: [
    ProductListingComponent,
    CollectionProductsComponent
  ],
  imports: [
    CommonModule,
    ProductListingRoutingModule,
    CoreModule
  ]
})
export class ProductListingModule { }
