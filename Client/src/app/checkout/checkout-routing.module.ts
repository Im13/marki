import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CheckoutComponent } from './checkout.component';
import { CheckoutSuccessComponent } from './checkout-success/checkout-success.component';
import { ActivatedRoute } from '@angular/router';

const routes: Routes = [
  { path: '', component: CheckoutComponent },
  { path: 'success/:id', component: CheckoutSuccessComponent }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class CheckoutRoutingModule { }
