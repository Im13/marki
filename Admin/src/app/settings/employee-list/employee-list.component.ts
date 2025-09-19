import { Component, OnInit } from '@angular/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { SettingsService } from '../settings.service';
import { User } from 'src/app/_shared/_models/user';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';
import { EditEmployeeModalComponent } from './edit-employee-modal/edit-employee-modal.component';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  users: User[] = [];
  availableRoles = [
    'SuperAdmin',
    'Admin',
    'Customer'
  ];
  loading: boolean = false;

  constructor(private settingsService: SettingsService, private modalServices: NzModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
    this.addEmployee();
  }

  getUsersWithRoles() {
    this.loading = true;
    this.settingsService.getUsersWithRoles().subscribe({
      next: users => {
        this.users = users;
        this.loading = false;
      },
      error: err => {
        console.log(err);
        this.loading = false;
      }
    });
  }

  showEditRolesModal(user: User) {
    const modal = this.modalServices.create({
      nzTitle: 'Edit Roles for ' + user.username,
      nzContent: RolesModalComponent,
      nzData: {
        username: user.username,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles],
        user: user
      }
    });

    modal.afterClose.subscribe(() => this.getUsersWithRoles());
  }

  addEmployee() {
    const modal = this.modalServices.create<EditEmployeeModalComponent>({
      nzTitle: 'Thêm nhân viên',
      nzContent: EditEmployeeModalComponent,
      nzCentered: true,
      nzWidth: '40vh'
    });

    modal.afterClose.subscribe(() => this.getUsersWithRoles());
  }
}
