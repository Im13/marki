import { DatePipe } from '@angular/common';
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

    if(this.infoForm.value.orderCreatedDate === '') {
      this.infoForm.get('orderCreatedDate').patchValue(new Date());
    }
  }

  onDateChange(result: Date) {
  }
}
