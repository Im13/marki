import { Component, OnInit } from '@angular/core';
import { Product } from '../shared/models/product';
import { HomeService } from '../home/home.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  
  constructor(private homeService: HomeService) { }

  ngOnInit(): void {
    this.homeService.getProducts().subscribe({
      next: response => this.products.push(...response.data),
      error: err => console.log(err)
    });
  }

}
