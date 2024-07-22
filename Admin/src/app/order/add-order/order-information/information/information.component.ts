import { Component, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {
  infoForm!: FormGroup;
  orderCareStaff = null;
  customerCareStaff = null;

  constructor(private rootFormGroup: FormGroupDirective) {}

  ngOnInit(): void {
    this.infoForm = this.rootFormGroup.control.get('information') as FormGroup;

    this.infoForm.setValue({
      orderCreatedDate: new Date(),
      orderCareStaff: '',
      customerCareStaff: ''
    });
  }

  onDateChange(result: Date) {
    console.log('onChange: ', result);
  }
}
