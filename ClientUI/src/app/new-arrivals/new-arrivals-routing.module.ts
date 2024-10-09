import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewArrivalsComponent } from './new-arrivals.component';

const routes: Routes = [
  { path: '', component: NewArrivalsComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class NewArrivalsRoutingModule { }
