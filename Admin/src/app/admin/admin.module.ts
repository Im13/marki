import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { RouterModule } from '@angular/router';
import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { RolesModalComponent } from './roles-modal/roles-modal.component';



@NgModule({
  declarations: [
    AdminComponent,
    RolesModalComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    AdminRoutingModule,
    SharedModule,
    CoreModule
  ]
})
export class AdminModule { }
