import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AccountService } from 'src/app/account/account.service';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    standalone: true,
    imports: [ReactiveFormsModule, NgIf, RouterLink]
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    email: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe({
      next: user => console.log(user)
    })
  }
}
