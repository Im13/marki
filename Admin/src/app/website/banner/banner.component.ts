import { Component } from '@angular/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AddBannerModalComponent } from './add-banner-modal/add-banner-modal.component';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.css'],
})
export class BannerComponent {
  constructor(private modalServices: NzModalService) {
    this.addBanner();
  }

  addBanner() {
    const modal = this.modalServices.create<AddBannerModalComponent>({
      nzTitle: 'Thiết lập slide',
      nzContent: AddBannerModalComponent,
      nzCentered: true,
      nzWidth: '70vh',
    });

    modal.afterClose.subscribe(() => this.getBanners());
  }

  getBanners() {
    // Get banners
  }
}
