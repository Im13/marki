import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  complexPassword = "(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$";
  
  registerForm = this.fb.group({
    displayname: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.pattern(this.complexPassword)]]
  })

  ngOnInit(): void {
  }

  onSubmit() {
    console.log(this.registerForm);
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => this.router.navigateByUrl('/')
    })
  }

}
