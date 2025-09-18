import { Component } from '@angular/core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent {
  surname: string = 'Nguyễn';
  name: string = 'Bách';
  username: string = 'bachnguyen';
  email: string = 'bach.nguyenluongmsh@gmail.com';
  phoneNumber: string = '0345678901';
}
