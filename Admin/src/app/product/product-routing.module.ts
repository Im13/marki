import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductTypeComponent } from './product-type/product-type.component';

const routes: Routes = [
  { path: '', component: ProductListComponent },
  { path: 'types', component: ProductTypeComponent }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class ProductRoutingModule { }
