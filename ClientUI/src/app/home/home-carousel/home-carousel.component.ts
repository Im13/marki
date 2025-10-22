import { Component, OnInit } from '@angular/core';
import { HomeService } from '../home.service';
import { SlideImage } from 'src/app/_shared/_models/slideImages';

@Component({
  selector: 'app-home-carousel',
  templateUrl: './home-carousel.component.html',
  styleUrls: ['./home-carousel.component.css']
})
export class HomeCarouselComponent implements OnInit {
  slides: SlideImage[] = [];
  effect = 'scrollx';
  array = [1, 2, 3, 4];

  constructor(private homeServices: HomeService) { }

  ngOnInit(): void {
    this.homeServices.getSlides().subscribe({
      next: (slides: SlideImage[]) => {
        this.slides = slides;
      },
      error: (error: any) => {
        console.log(error);
      }
    });
  }

  onImageLoad(event: any): void {
    const img = event.target;
    console.log('Image natural size:', img.naturalWidth, 'x', img.naturalHeight);
    console.log('Image displayed size:', img.width, 'x', img.height);
    console.log('Image computed style:', window.getComputedStyle(img));
  }
}
