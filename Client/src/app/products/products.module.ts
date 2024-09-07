import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsComponent } from './products.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductsRoutingModule } from './products-routing.module';
import { GalleryModule } from '@daelmaak/ngx-gallery';
import { ProductSizeComponent } from './product-detail/product-size/product-size.component';
import { ProductColorComponent } from './product-detail/product-color/product-color.component';
import { ProductDescriptionComponent } from './product-detail/product-description/product-description.component';
import { ProductServicesComponent } from './product-detail/product-services/product-services.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    ProductsComponent,
    ProductDetailComponent,
    ProductSizeComponent,
    ProductColorComponent,
    ProductDescriptionComponent,
    ProductServicesComponent
  ],
  imports: [
    CommonModule,
    ProductsRoutingModule,
    GalleryModule,
    RouterModule
  ]
})
export class ProductsModule { }
