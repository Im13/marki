import { Component, OnInit } from '@angular/core';
import { Product } from '../shared/models/product';
import { HomeService } from './home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  products: Product[] = [];

  constructor(private homeService: HomeService) { }

  ngOnInit(): void {
    this.homeService.getProducts().subscribe({
      next: response => this.products.push(...response.data),
      error: err => console.log(err)
    });
  }

}
