import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductDetailComponent } from './product-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full'  },
  { path: ':slug', component: ProductDetailComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class ProductDetailRoutingModule { }
