import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  isRegistering = false;
  loginForm: FormGroup;
  registerForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: this.formBuilder.control('', Validators.required),
      password: this.formBuilder.control('', Validators.required),
    });
  }

  toggleForm() {
    this.isRegistering = !this.isRegistering;
  }

  onLogin() {
    // if (this.loginForm.valid) {
    //     const { username, password } = this.loginForm.value;
    //     console.log(`Username: ${username}, Password: ${password}`);
    // }
  }

  onRegister() {
    // if (this.registerForm.valid) {
    //     const { name, email, username, password, confirmPassword } = this.registerForm.value;
    //     if (password === confirmPassword) {
    //         console.log(`Name: ${name}, Email: ${email}, Username: ${username}, Password: ${password}`);
    //     } else {
    //         alert("Passwords do not match");
    //     }
    // }
  }
}
