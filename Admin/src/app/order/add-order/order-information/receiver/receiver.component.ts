import { Component } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-receiver',
  templateUrl: './receiver.component.html',
  styleUrls: ['./receiver.component.css']
})
export class ReceiverComponent {
  receiverInfoForm!: FormGroup;

  constructor(private rootFormGroup: FormGroupDirective) {}

  ngOnInit(): void {
    this.receiverInfoForm = this.rootFormGroup.control.get('receiverInfo') as FormGroup;

    this.receiverInfoForm.setValue({
      receiverName: '',
      receiverPhoneNumber: '',
      receiverAddress: '',
      provinceId: '',
      districtId: '',
      wardId: ''
    });
  }
}
