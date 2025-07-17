import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { SettingsComponent } from './settings.component';

const routes: Routes = [
  { path: '', component: SettingsComponent },
  { path: 'employee-list', component: EmployeeListComponent }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
})
export class SettingsRoutingModule {}
