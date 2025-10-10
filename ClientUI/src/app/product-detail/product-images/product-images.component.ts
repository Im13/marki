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
}
