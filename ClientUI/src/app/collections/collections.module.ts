import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CollectionsComponent } from './collections.component';
import { CollectionsRoutingModule } from './collections-routing.module';
import { CoreModule } from '../_core/core.module';
import { CollectionProductsComponent } from './collection-products/collection-products.component';

@NgModule({
  declarations: [
    CollectionsComponent,
    CollectionProductsComponent
  ],
  imports: [
    CommonModule,
    CollectionsRoutingModule,
    CoreModule
  ]
})
export class CollectionsModule { }
