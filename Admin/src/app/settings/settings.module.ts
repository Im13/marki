import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SettingsRoutingModule } from './settings-routing.module';
import { SharedModule } from '../_shared/shared.module';
import { CoreModule } from '../_core/core.module';
import { RolesModalComponent } from './roles-modal/roles-modal.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { EditEmployeeModalComponent } from './employee-list/edit-employee-modal/edit-employee-modal.component';

@NgModule({
  declarations: [
    RolesModalComponent,
    EmployeeListComponent,
    EditEmployeeModalComponent
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
