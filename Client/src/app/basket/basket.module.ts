import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketComponent } from './basket.component';
import { BasketRoutingModule } from './basket-routing.module';
import { RouterModule } from '@angular/router';



@NgModule({
    imports: [
        CommonModule,
        BasketRoutingModule,
        RouterModule,
        BasketComponent
    ]
})
export class BasketModule { }
