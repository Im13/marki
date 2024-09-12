import { AfterViewInit, Component, ViewChild } from '@angular/core';
import SwiperCore, { Autoplay, SwiperOptions } from 'swiper';
import { SwiperComponent } from 'swiper/angular';

SwiperCore.use([Autoplay]);

@Component({
  selector: 'app-home-carousel',
  templateUrl: './home-carousel.component.html',
  styleUrls: ['./home-carousel.component.css']
})
export class HomeCarouselComponent implements AfterViewInit {
  @ViewChild('swiperSlideShow') swiperSlideShow!: SwiperComponent;
  config: SwiperOptions;

  constructor() {
    this.config = {
      slidesPerView: 1,
      navigation: true,
      pagination: { clickable: true },
      scrollbar: { draggable: true },
      autoplay: {
        delay: 2000
      },
      speed: 300
    }
  }

  ngAfterViewInit(): void {
    this.swiperSlideShow.swiperRef.autoplay.start();
  }
}
