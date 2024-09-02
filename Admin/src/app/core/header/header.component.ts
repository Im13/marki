import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/_service/account.service';
import { User } from 'src/app/shared/_models/user';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  username: string = '';

  constructor() {}

  ngOnInit(): void {
    const user : User = JSON.parse(localStorage.getItem('user'));
    this.username = user.displayName;
  }

  
}
