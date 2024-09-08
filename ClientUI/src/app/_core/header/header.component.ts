import { Component, ElementRef } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  host: {
    '(document:click)' : 'onClick($event)',
  }
})
export class HeaderComponent {
  isMenuCollapsed = true;
  isLoginCollapsed = true;

  constructor(private _eref: ElementRef) {}

  // Remove menu items when click outside
  onClick(event: any) {
    if(!this._eref.nativeElement.contains(event.target)) {
      this.isMenuCollapsed = true;
      this.isLoginCollapsed = true;
    }
  }

  collapseLoginRegisterForm() {
    this.isLoginCollapsed = !this.isLoginCollapsed;
    this.isMenuCollapsed = true;
  }

  collapseMenuForm() {
    this.isMenuCollapsed = !this.isMenuCollapsed;
    this.isLoginCollapsed = true;
  }
}
