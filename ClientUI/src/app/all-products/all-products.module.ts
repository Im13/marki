import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllProductsComponent } from './all-products.component';
import { CoreModule } from '../_core/core.module';
import { AllProductsRoutingModule } from './all-products-routing.module';
import { ProductsFooterComponent } from './products-footer/products-footer.component';
import { ProductImagesComponent } from './product-images/product-images.component';
import { ProductInfoComponent } from './product-info/product-info.component';

@NgModule({
  declarations: [
    AllProductsComponent,
    ProductsFooterComponent,
    ProductImagesComponent,
    ProductInfoComponent
  ],
  imports: [
    CommonModule,
    AllProductsRoutingModule,
    CoreModule
  ]
})
export class AllProductsModule { }
