import { Component, OnInit } from '@angular/core';
import { GalleryItem } from '@daelmaak/ngx-gallery';
import { Product } from 'src/app/shared/models/product';
import { ProductServicesService } from '../product-services.service';
import { ActivatedRoute } from '@angular/router';

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

  product: Product;

  constructor(private productServices: ProductServicesService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.productServices.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe({
      next: product => this.product = product,
      error: err => console.log(err)
    });
  }
}
