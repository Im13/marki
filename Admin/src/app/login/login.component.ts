import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AccountService } from '../_core/_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loginModel: any = {};

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private accountService: AccountService, 
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: this.formBuilder.control('', Validators.required),
      password: this.formBuilder.control('', Validators.required),
    });
  }

  onLogin() {
    if(this.loginForm.value.email.trim() == '') {
      this.toastr.error("Please input username!");
      return;
    }

    if(this.loginForm.value.password.trim() == '') {
      this.toastr.error("Please input password");
      return;
    }

    if (this.loginForm.valid) {
      this.loginModel.email = this.loginForm.value.email;
      this.loginModel.password = this.loginForm.value.password;

      this.accountService.login(this.loginModel).subscribe({
        next: () => {
          this.router.navigateByUrl('/');
        }
      })
    }
  }
}
