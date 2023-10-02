import { Component, OnInit } from '@angular/core';
import { GalleryImage, GalleryItem } from '@daelmaak/ngx-gallery';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  images: GalleryItem[] = [
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: 'czxc', data: '' },
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: '', data: '' },
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: '', data: '' },
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: '', data: '' },
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: '', data: '' },
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: '', data: '' },
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: '', data: '' },
    { src: 'https://localhost:5001/images/products/sb-ang1.png', thumbSrc: 'https://localhost:5001/images/products/sb-ang1.png', alt: '', description: '', data: '' }
  ];


  constructor() { }

  ngOnInit(): void {
  }

}
