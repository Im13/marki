import { Component } from '@angular/core';
import { COAT, SHIRT, DRESS, PANTS } from '../_shared/_consts/productTypeConst';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  coatId = COAT;
  shirtId = SHIRT;
  dressId = DRESS;
  pantsId = PANTS;
}
