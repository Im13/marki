import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-banner-modal',
  templateUrl: './add-banner-modal.component.html',
  styleUrls: ['./add-banner-modal.component.css']
})
export class AddBannerModalComponent implements OnInit {
  addBannerFrm: FormGroup;
  isEdit = false;

  constructor() {}

  ngOnInit(): void {
    this.addBannerFrm = new FormGroup({
      desktopImageUrl: new FormControl('', [Validators.required]),
      link: new FormControl(''),
      mobileImageUrl: new FormControl('', [Validators.required]),
      altText: new FormControl(''),
      status: new FormControl(true),
    });
  }
}
