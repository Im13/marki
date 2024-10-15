import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CollectionMapConst } from '../_shared/_consts/collectionsMapConst';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css']
})
export class CollectionsComponent implements OnInit {
  collectionId: number;
  products: any[];

  constructor(
    private route: ActivatedRoute
    // private productService: ProductService
  ) { }

  ngOnInit(): void {
    // Truy cập collectionId từ route data
    this.collectionId = +this.route.snapshot.data['collectionId'];

    console.log(this.collectionId);
  }
}
