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
        console.log('=== SLIDES DATA FROM API ===');
        console.log('Total slides:', slides.length);
        slides.forEach((slide, index) => {
          console.log(`Slide ${index + 1}:`, {
            desktopUrl: slide.desktopImageUrl,
            mobileUrl: slide.mobileImageUrl,
            altText: slide.altText
          });
        });
        console.log('===========================');
      },
      error: (error: any) => {
        console.log('Error loading slides:', error);
      }
    });
  }

  onImageLoad(event: any): void {
    const img = event.target;
    console.log('=== IMAGE DEBUG INFO ===');
    console.log('Image natural size:', img.naturalWidth, 'x', img.naturalHeight);
    console.log('Image displayed size:', img.width, 'x', img.height);
    console.log('Image src:', img.src);
    console.log('Image currentSrc:', img.currentSrc);
    console.log('Image computed style:', window.getComputedStyle(img));
    console.log('Image quality check:');
    console.log('- object-fit:', window.getComputedStyle(img).objectFit);
    console.log('- object-position:', window.getComputedStyle(img).objectPosition);
    console.log('- width:', window.getComputedStyle(img).width);
    console.log('- height:', window.getComputedStyle(img).height);
    
    // Test Cloudinary quality parameters
    this.testCloudinaryQuality(img.src);
    console.log('========================');
  }

  private testCloudinaryQuality(imageUrl: string): void {
    console.log('=== CLOUDINARY QUALITY TEST ===');
    
    // Test different quality parameters
    const baseUrl = imageUrl.split('?')[0];
    const testUrls = [
      { name: 'Original', url: imageUrl },
      { name: 'Auto Good', url: `${baseUrl}?q_auto:good` },
      { name: 'Auto Best', url: `${baseUrl}?q_auto:best` },
      { name: 'Quality 80', url: `${baseUrl}?q_80` },
      { name: 'Quality 90', url: `${baseUrl}?q_90` },
      { name: 'Quality 100', url: `${baseUrl}?q_100` }
    ];
    
    console.log('Test URLs for quality comparison:');
    testUrls.forEach(test => {
      console.log(`${test.name}: ${test.url}`);
    });
    console.log('================================');
  }

  // Phương thức để force refresh ảnh với quality cao
  public forceHighQualityImages(): void {
    this.slides.forEach((slide, index) => {
      // Thêm quality parameter vào URL
      if (slide.desktopImageUrl && !slide.desktopImageUrl.includes('q_')) {
        slide.desktopImageUrl += '?q_auto:best';
      }
      if (slide.mobileImageUrl && !slide.mobileImageUrl.includes('q_')) {
        slide.mobileImageUrl += '?q_auto:best';
      }
    });
    console.log('Forced high quality images:', this.slides);
  }
}
