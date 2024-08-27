import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';
import { OrderService } from 'src/app/order/order.service';
import { District } from 'src/app/shared/_models/address/district';
import { Province } from 'src/app/shared/_models/address/province';
import { Ward } from 'src/app/shared/_models/address/ward';

@Component({
  selector: 'app-receiver',
  templateUrl: './receiver.component.html',
  styleUrls: ['./receiver.component.css']
})
export class ReceiverComponent implements OnInit {
  receiverInfoForm!: FormGroup;
  @Input() provinces: Province[];
  districts: District[];
  wards: Ward[];

  constructor(private rootFormGroup: FormGroupDirective, private orderService: OrderService) {}

  ngOnInit(): void {
    this.receiverInfoForm = this.rootFormGroup.control.get('receiverInfo') as FormGroup;

    var provinceId = this.receiverInfoForm.get('provinceId').value;
    var districtId = this.receiverInfoForm.get('districtId').value;

    if(provinceId !== '' && districtId !== '') {
      this.provinceChange(provinceId);
      this.districtChange(districtId);
    }
  }

  provinceChange(provinceId: number): void {
    this.orderService.getDistricts(provinceId).subscribe({
      next: result => {
        this.districts = result;
      }
    });
  }

  districtChange(districtId: number): void {
    this.orderService.getWards(districtId).subscribe({
      next: result => {
        this.wards = result;
      }
    });
  }
}
