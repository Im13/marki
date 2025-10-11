import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '../_core/core.module';
import { ProductsFooterComponent } from './products-footer/products-footer.component';
import { ProductImagesComponent } from './product-images/product-images.component';
import { ProductInfoComponent } from './product-info/product-info.component';
import { ProductDetailRoutingModule } from './product-detail-routing.module';
import { ProductDetailComponent } from './product-detail.component';

@NgModule({
  declarations: [
    ProductDetailComponent,
    ProductsFooterComponent,
    ProductImagesComponent,
    ProductInfoComponent
  ],
  imports: [
    CommonModule,
    ProductDetailRoutingModule,
    CoreModule
  ]
})
export class ProductDetailModule { }
