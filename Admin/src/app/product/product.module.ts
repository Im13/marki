import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListComponent } from './product-list/product-list.component';
import { RouterModule } from '@angular/router';
import { ProductRoutingModule } from './product-routing.module';
import { AddProductComponent } from './product-list/add-product/add-product.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { AddProductModalComponent } from './product-list/add-product-modal/add-product-modal.component';

@NgModule({
  declarations: [
    ProductListComponent,
    AddProductComponent,
    AddProductModalComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ProductRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    PopoverModule.forRoot(),
    FormsModule
  ],
  exports: [
    ProductListComponent
  ]
})
export class ProductModule { }
