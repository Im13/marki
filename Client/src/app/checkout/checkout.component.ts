import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket/basket.service';
import { CheckoutService } from './checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from '../shared/models/basket';
import { Address } from '../shared/models/address';
import { NavigationExtras, Router } from '@angular/router';
import { Province } from '../shared/models/province';
import { District } from '../shared/models/district';
import { Ward } from '../shared/models/ward';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  checkoutFrm: FormGroup;
  isDisplayingOrder: boolean = true;
  provinces: Province[];
  districts: District[];
  wards: Ward[];
  selectedProvince = 0;
  selectedDistrict = 0;
  selectedWard = 0;

  constructor(public basketService: BasketService, private checkoutService: CheckoutService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.checkoutFrm = new FormGroup({
      fullName: new FormControl('', Validators.required),
      phone: new FormControl('', Validators.required),
      street: new FormControl('', Validators.required),
      province: new FormControl(0),
      district: new FormControl(0),
      ward: new FormControl(0)
    })

    this.checkoutService.getProvinces().subscribe({
      next: provinces => {
        this.provinces = provinces;
      }
    });
  }

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    if(!basket) return;

    const orderToCreate = this.getOrderToCreate(basket);
    console.log(orderToCreate);

    if(!orderToCreate) return;

    this.checkoutService.createOrder(orderToCreate).subscribe({
      next: order => {
        this.toastr.success('Order created successfully!');
        this.basketService.deleteLocalBasket();
        const navigationExtras: NavigationExtras = { state: order };
        this.router.navigate(['checkout/success'], navigationExtras);
      }
    });
  }

  private getOrderToCreate(basket: Basket) {
    const deliveryMethodId = 1;
    const shipToAddress: Address = {
      fullName: this.checkoutFrm.value.fullName,
      cityOrProvinceId: this.checkoutFrm.value.province,
      districtId: this.checkoutFrm.value.district,
      street: this.checkoutFrm.value.street,
      wardId: this.checkoutFrm.value.ward,
      phoneNumber: this.checkoutFrm.value.phone
    };

    if(!deliveryMethodId || !shipToAddress) return null;

    return {
      basketId: basket.id,
      deliveryMethodId: deliveryMethodId,
      shipToAddress: shipToAddress
    };
  }

  toggleDisplayOrder() {
    this.isDisplayingOrder = !this.isDisplayingOrder;
  }

  onProvinceChange(provinceId: number) {
    this.checkoutService.getDistrictsByProvinceId(provinceId).subscribe({
      next: districts => {
        this.districts = districts;
      }
    });
  }

  onDistrictChange(districtId: number) {
    this.checkoutService.getWardsByDistrictId(districtId).subscribe({
      next: wards => {
        this.wards = wards;
      }
    });
  }

  onSubmit() {
    const basket = this.basketService.getCurrentBasketValue();
    if(!basket) return;

    const orderToCreate = this.getOrderToCreate(basket);
    console.log(orderToCreate);

    if(!orderToCreate) return;

    this.checkoutService.createOrder(orderToCreate).subscribe({
      next: order => {
        this.toastr.success('Order created successfully!');
        this.basketService.deleteLocalBasket();
        const navigationExtras: NavigationExtras = { state: order };
        this.router.navigate(['checkout/success'], navigationExtras);
      }
    });
  }
}
