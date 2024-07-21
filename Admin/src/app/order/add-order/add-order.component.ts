import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.css']
})
export class AddOrderComponent implements OnInit {
  tabs = ['Tab 1', 'Tab 2'];
  selectedIndex = 0;
  addOrderForm: FormGroup;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.addOrderForm = this.formBuilder.group({
      skus: this.formBuilder.group({
        
      })
    });
  }

  closeTab({ index }: { index: number }): void {
    this.tabs.splice(index, 1);
  }

  newTab(): void {
    this.tabs.push('New Tab');
    this.selectedIndex = this.tabs.length;
  }

}
