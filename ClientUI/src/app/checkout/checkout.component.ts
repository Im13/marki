import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket/basket.service';
import { CheckoutService } from './checkout.service';
import { NavigationExtras, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Province } from '../_shared/_models/address/province';
import { District } from '../_shared/_models/address/district';
import { Ward } from '../_shared/_models/address/ward';
import { Basket, BasketItem } from '../_shared/_models/basket';

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

  processedBasketItems: any[] = [];
  basketItems: BasketItem[] = [];
  
  constructor(public basketService: BasketService, private checkoutService: CheckoutService, private router: Router) {}

  ngOnInit(): void {
    this.checkoutFrm = new FormGroup({
      fullName: new FormControl('', Validators.required),
      phone: new FormControl('', Validators.required),
      street: new FormControl('', Validators.required),
      province: new FormControl(0, [Validators.required, this.forbiddenSelect.bind(this)]),
      district: new FormControl(0, [Validators.required, this.forbiddenSelect.bind(this)]),
      ward: new FormControl(0, [Validators.required, this.forbiddenSelect.bind(this)])
    })

    this.basketItems = this.basketService.getCurrentBasketValue().items;

    this.processedBasketItems = this.basketItems.map(item => ({
      ...item,
      formattedSKUValues: item.productSKUValues.map(skuValue => skuValue.productOptionValue.valueName).join(' / ')
    }));

    this.checkoutService.getProvinces().subscribe({
      next: result => {
        this.provinces = result;
      },
      error: err => {
        console.log(err);
      }
    });

    // this.basketService.basketSource$.
    // this.checkoutService.getProvinces().subscribe({
    //   next: provinces => {
    //     this.provinces = provinces;
    //   }
    // });
  }

  forbiddenSelect(control: FormControl): {[s: string] : boolean} {
    if(control.value == 0) {
      return {control: false};
    }

    return null;
  }

  toggleDisplayOrder() {
    this.isDisplayingOrder = !this.isDisplayingOrder;
  }

  onSubmit() {
    const basket = this.basketService.getCurrentBasketValue();
    if(!basket) return;

    const orderToCreate = this.getOrderToCreate(basket);

    if(!orderToCreate) return;

    this.checkoutService.createOrder(orderToCreate).subscribe({
      next: order => {
        this.basketService.deleteLocalBasket();
        const navigationExtras: NavigationExtras = { state: order };
        this.router.navigate(['checkout/success'], navigationExtras);
      }
    });
  }

  private getOrderToCreate(basket: Basket) {
    const deliveryMethodId = 1;
    const shipToAddress = {
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

  onProvinceChange(provinceId: number): void {
    this.checkoutService.getDistricts(provinceId).subscribe({
      next: result => {
        this.districts = result;
      }
    });
  }

  onDistrictChange(districtId: number) {
    this.checkoutService.getWards(districtId).subscribe({
      next: result => {
        this.wards = result;
      }
    });
  }
}
