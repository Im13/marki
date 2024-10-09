import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewArrivalsComponent } from './new-arrivals.component';
import { NewArrivalsRoutingModule } from './new-arrivals-routing.module';
import { CoreModule } from '../_core/core.module';



@NgModule({
  declarations: [
    NewArrivalsComponent
  ],
  imports: [
    CommonModule,
    NewArrivalsRoutingModule,
    CoreModule
  ]
})
export class NewArrivalsModule { }
