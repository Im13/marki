import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/shared/models/pagination';
import { Product } from 'src/app/shared/models/product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  menuOpen = false;
  products: Product[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<Pagination<Product[]>>('https://localhost:5001/api/products?sort=priceAsc').subscribe({
      next: val => this.products = val.data,
      error: err => console.log(err)
    });
  }

}
