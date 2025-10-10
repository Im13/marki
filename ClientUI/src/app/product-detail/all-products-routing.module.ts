import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllProductsComponent } from './all-products.component';

const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full'  },
  { path: ':slug', component: AllProductsComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class AllProductsRoutingModule { }
