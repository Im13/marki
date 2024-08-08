import { Component, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';
import { OrderService } from 'src/app/order/order.service';
import { Province } from 'src/app/shared/models/address/province';

@Component({
  selector: 'app-receiver',
  templateUrl: './receiver.component.html',
  styleUrls: ['./receiver.component.css']
})
export class ReceiverComponent implements OnInit {
  receiverInfoForm!: FormGroup;
  provinces: Province[];

  constructor(private rootFormGroup: FormGroupDirective, private orderService: OrderService) {}

  ngOnInit(): void {
    this.receiverInfoForm = this.rootFormGroup.control.get('receiverInfo') as FormGroup;
    this.orderService.getProvinces().subscribe({
      next: provices => {
        this.provinces = provices;
      },
      error: err => {
        console.log(err);
      }
    })
  }
}
