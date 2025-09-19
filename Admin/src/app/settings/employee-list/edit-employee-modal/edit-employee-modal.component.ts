import { Component, inject, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_core/_services/account.service';
import { Account } from 'src/app/_shared/_models/account/account';

@Component({
  selector: 'app-edit-employee-modal',
  templateUrl: './edit-employee-modal.component.html',
  styleUrls: ['./edit-employee-modal.component.css']
})
export class EditEmployeeModalComponent implements OnInit {
  addForm: FormGroup;
  isSubmitting: boolean = false;
  isEdit: boolean = false; 
  @Input() account?: Account = inject(NZ_MODAL_DATA);
  
  ngOnInit(): void {
    this.addForm = new FormGroup({
      displayName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required),
      confirmPassword: new FormControl('', Validators.required)
    });

    if (this.account == null) {
      this.account = {
        id: null,
        displayName: '',
        email: '',
        password: '',
      };
    }
  }

  constructor(private accountService: AccountService, private toastrService: ToastrService, private modal: NzModalRef) {}

  onSubmit() {
    this.account.displayName = this.addForm.value?.displayName;
    this.account.email = this.addForm.value?.email;
    this.account.password = this.addForm.value?.password;

    if (this.addForm.valid) {
      this.isSubmitting = true;
      
      if (!this.isEdit) {
        this.accountService.createEmployee(this.account).subscribe({
          next: () => {
            this.toastrService.success('Thêm nhân viên thành công!');
            this.destroyModal();
          },
          error: (err) => {
            this.toastrService.error(err);
            this.isSubmitting = false;
          },
        });
      } else {
        this.accountService.editEmployee(this.account).subscribe({
          next: () => {
            this.toastrService.success('Sửa nhân viên thành công!');
            this.destroyModal();
          },
          error: (err) => {
            this.toastrService.error(err);
            this.isSubmitting = false;
          },
        });
      }
    }
  }

  destroyModal(): void {
    this.modal.destroy();
  }
}
