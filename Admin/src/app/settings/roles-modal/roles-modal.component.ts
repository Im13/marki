import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { User } from 'src/app/_shared/_models/user';
import { ToastrService } from 'ngx-toastr';
import { SettingsService } from '../settings.service';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  username = '';
  availableRoles: any[] = [];
  selectedRoles: any[] = [];
  user: User;
  @Input() data?: any = inject(NZ_MODAL_DATA);
  @Output() updateSelectedRoles = new EventEmitter();

  constructor(private settingsService: SettingsService, private toastrService: ToastrService, private modal: NzModalRef) {}
  
  ngOnInit(): void {
    this.availableRoles = this.data.availableRoles;
    this.selectedRoles = this.data.selectedRoles;
    this.username = this.data.username;
    this.user = this.data.user;
  }

  updateChecked(checkedValue: string) {
    const index = this.selectedRoles.indexOf(checkedValue);
    index !== -1 ? this.selectedRoles.splice(index, 1) : this.selectedRoles.push(checkedValue);
  }

  updateRoles() {
    if(!this.arrayEqual(this.selectedRoles!, this.user.roles)) {
      this.settingsService.updateUserRoles(this.user.username, this.selectedRoles!).subscribe({
        next: () => {
          this.toastrService.success('Cập nhật thành công!');
          this.modal.destroy();
        }
      })
    }
  }

  private arrayEqual(arr1: any[], arr2: any[]) {
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }
}
