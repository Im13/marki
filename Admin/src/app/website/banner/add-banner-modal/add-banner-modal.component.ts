import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NzUploadFile } from 'ng-zorro-antd/upload';

const getBase64 = (file: File): Promise<string | ArrayBuffer | null> =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      resolve(reader.result);
    }
    reader.onerror = (error) => reject(error);
  });

@Component({
  selector: 'app-add-banner-modal',
  templateUrl: './add-banner-modal.component.html',
  styleUrls: ['./add-banner-modal.component.css']
})
export class AddBannerModalComponent implements OnInit {
  addBannerFrm: FormGroup;
  isEdit = false;
  desktopImage: NzUploadFile[] = [];
  previewImage: string | undefined = '';
  previewVisible = false;

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

  handlePreview = async (file: NzUploadFile): Promise<void> => {
      if (!file.url && !file.preview) {
        file.preview = await getBase64(file.originFileObj!);
      }
      this.previewImage = file.url || file.preview;
      this.previewVisible = true;
  };

  handleUploadProductChange(info: { file: NzUploadFile }, isMainPhoto: boolean): void {
    if (info.file.status === 'done') {
      console.log(info)
    }
  }
}
