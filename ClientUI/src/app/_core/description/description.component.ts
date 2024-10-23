import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-description',
  templateUrl: './description.component.html',
  styleUrls: ['./description.component.css']
})
export class DescriptionComponent {
  @Input() description: string;
  
  panels = [
    {
      active: true,
      name: 'Mô tả',
      disabled: false
    },
    {
      active: false,
      disabled: false,
      name: 'Chính sách giao hàng'
    },
    {
      active: false,
      disabled: true,
      name: 'Chính sách đổi hàng'
    }
  ];
}
