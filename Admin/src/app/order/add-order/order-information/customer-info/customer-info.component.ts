import { Component, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-customer-info',
  templateUrl: './customer-info.component.html',
  styleUrls: ['./customer-info.component.css']
})
export class CustomerInfoComponent implements OnInit {
  customerName: string;
  customerPhoneNumber: string;
  customerEmail: string;
  customerDOB = new Date();
  customerInfoForm!: FormGroup;

  constructor(private rootFormGroup: FormGroupDirective) {}

  ngOnInit(): void {
    this.customerInfoForm = this.rootFormGroup.control.get('customerInfo') as FormGroup;

    this.customerInfoForm.setValue({
      customerName: '',
      customerPhoneNumber: '',
      customerEmailAddress: '',
      customerDOB: '',
    });
  }
  
  onDOBChange(result: Date) {
    console.log('onChange: ', result);
  }
}
