import { Component } from '@angular/core';

@Component({
  selector: 'app-customer-info',
  templateUrl: './customer-info.component.html',
  styleUrls: ['./customer-info.component.css']
})
export class CustomerInfoComponent {
  customerName: string;
  customerPhoneNumber: string;
  customerEmail: string;
  customerBirthday = new Date();
  
  onBirthdayChange(result: Date) {
    console.log('onChange: ', result);
  }
}
