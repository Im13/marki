import { Component, Input, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { SwiperModule } from 'swiper/angular';
import { NgFor, DecimalPipe } from '@angular/common';

@Component({
    selector: 'app-category-section',
    templateUrl: './category-section.component.html',
    styleUrls: ['./category-section.component.css'],
    standalone: true,
    imports: [SwiperModule, NgFor, DecimalPipe]
})
export class CategorySectionComponent implements OnInit {
  @Input() products: Product[];

  constructor() { }

  ngOnInit(): void {
  }

}
