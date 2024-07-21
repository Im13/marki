import { Component } from '@angular/core';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent {
  orderCreatedDate = new Date();
  orderCareStaff = null;
  customerCareStaff = null;

  onDateChange(result: Date) {
    console.log('onChange: ', result);
  }
}
