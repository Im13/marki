import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';
import { NgIf, AsyncPipe } from '@angular/common';

@Component({
    selector: 'app-account-info',
    templateUrl: './account-info.component.html',
    styleUrls: ['./account-info.component.css'],
    standalone: true,
    imports: [NgIf, AsyncPipe]
})
export class AccountInfoComponent implements OnInit {

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  logout() {
    this.accountService.logout();
  }
}
