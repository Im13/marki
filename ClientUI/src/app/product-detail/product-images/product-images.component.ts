import { Component, Input, OnInit } from '@angular/core';
import { Photo } from 'src/app/_shared/_models/photo';
import { SwiperOptions } from 'swiper';

@Component({
  selector: 'app-product-images',
  templateUrl: './product-images.component.html',
  styleUrls: ['./product-images.component.css']
})
export class ProductImagesComponent implements OnInit {
  @Input() photos: Photo[] = [];
  mainPhoto: Photo;
  config: SwiperOptions;
  
  constructor() {
    this.config = {
      slidesPerView: 1,
      navigation: false,
      pagination: true,
      scrollbar: { draggable: true },
      autoplay: false
    }
  }

  ngOnInit(): void {
    // if(this.photos !== null)
    //   this.mainPhoto = this.photos.find(p => p.isMain == true);
  }

  get isEvenTotal(): boolean {
    return this.photos.length % 2 === 0;
  }

  get lastRowCount(): number {
    return this.isEvenTotal ? 4 : 3;
  }

  get topRows(): Photo[][] {
    const lastRowCount = this.lastRowCount;
    const topPhotosCount = this.photos.length - lastRowCount;
    const topPhotos = this.photos.slice(0, topPhotosCount);
    
    // Chia thành các hàng 2 ảnh
    const rows: Photo[][] = [];
    for (let i = 0; i < topPhotos.length; i += 2) {
      rows.push(topPhotos.slice(i, i + 2));
    }
    return rows;
  }

  get lastRow(): Photo[] {
    return this.photos.slice(-this.lastRowCount);
  }
}
