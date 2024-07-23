import { Component } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.css']
})
export class DeliveryComponent {
  deliveryForm!: FormGroup;

  constructor(private rootFormGroup: FormGroupDirective) {}

  ngOnInit(): void {
    this.deliveryForm = this.rootFormGroup.control.get('deliveryService') as FormGroup;

    this.deliveryForm.setValue({
      deliveryCompanyId: '',
      shipmentCode: '',
      shipmentCost: ''
    });
  }
}
