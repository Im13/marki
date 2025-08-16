import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SettingsRoutingModule } from './settings-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { RolesModalComponent } from './roles-modal/roles-modal.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';

@NgModule({
  declarations: [
    RolesModalComponent,
    EmployeeListComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    SettingsRoutingModule,
    SharedModule,
    CoreModule
  ]
})
export class SettingsModule { }
