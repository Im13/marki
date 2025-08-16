import { Component, OnInit } from '@angular/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { SettingsService } from '../settings.service';
import { User } from 'src/app/shared/_models/user';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';

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
  ]
  
  constructor(private settingsService: SettingsService, private modalService: NzModalService) {}

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.settingsService.getUsersWithRoles().subscribe({
      next: users => this.users = users
    })
  }

  showEditRolesModal(user: User) {
    const modal = this.modalService.create({
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
}
