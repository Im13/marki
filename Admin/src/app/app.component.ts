import { Component } from '@angular/core';
import { User } from './_shared/_models/user';
import { AccountService } from './_core/_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = '';

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user : User = JSON.parse(localStorage.getItem('user'));
    if(user)
    {
      this.accountService.setCurrentUser(user);
    }
  }
}
