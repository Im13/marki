import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-products-footer',
  templateUrl: './products-footer.component.html',
  styleUrls: ['./products-footer.component.css']
})
export class ProductsFooterComponent {
  @Output() atcClicked = new EventEmitter();
  constructor() {}

  addToCartClicked() {
    this.atcClicked.emit();
  }
}
