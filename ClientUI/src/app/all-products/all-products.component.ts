import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AllProductsService } from './all-products.service';

@Component({
  selector: 'app-all-products',
  templateUrl: './all-products.component.html',
  styleUrls: ['./all-products.component.css']
})
export class AllProductsComponent implements OnInit {
  productSlug: string;
  product: any;

  constructor(private route: ActivatedRoute, private allProductService: AllProductsService) {}

  ngOnInit(): void {
    console.log('ge')
    // Lấy giá trị slug từ URL
    this.productSlug = this.route.snapshot.paramMap.get('slug');

    // Sử dụng slug để tìm sản phẩm
    this.allProductService.getProductBySlug(this.productSlug).subscribe({
      next: response => {
        this.product = response;
      },
      error: err => {
        if(err.status == 404) {
          console.log("Cannot find product");
        } else if (err.status == 400) {
          console.log(err);
        }
      }
    });
  }

}
