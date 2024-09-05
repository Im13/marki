import { Component, OnInit } from '@angular/core';
import { User } from '../shared/_models/user';
import { AdminService } from './admin.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { RolesModalComponent } from './roles-modal/roles-modal.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  users: User[] = [];
  availableRoles = [
    'SuperAdmin',
    'Admin',
    'Customer'
  ]

  constructor(private adminService: AdminService, private modalService: NzModalService) {}

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe({
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
