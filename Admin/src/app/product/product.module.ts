import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListComponent } from './product-list/product-list.component';
import { RouterModule } from '@angular/router';
import { ProductRoutingModule } from './product-routing.module';
import { AddProductComponent } from './product-list/add-product/add-product.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    ProductListComponent,
    AddProductComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ProductRoutingModule,
    ReactiveFormsModule,
  ],
  exports: [
    ProductListComponent
  ]
})
export class ProductModule { }
