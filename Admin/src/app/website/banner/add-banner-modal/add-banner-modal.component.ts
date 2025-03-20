import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { SlideImage } from 'src/app/shared/_models/slideImages';
import { WebsiteServiceService } from '../../website-service.service';

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
  isSubmitting: boolean = false;
  banner: SlideImage;

  desktopImage: NzUploadFile[] = [];
  mobileImage: NzUploadFile[] = [];
  previewImage: string | undefined = '';
  previewVisible = false;

  constructor(private websiteSettingServices: WebsiteServiceService) {}

  ngOnInit(): void {
    this.addBannerFrm = new FormGroup({
      link: new FormControl('', [Validators.required]),
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
      console.log(info);
      console.log(this.desktopImage);
    }
  }

  onSubmit() {
    this.isSubmitting = true;

    this.banner = {
      id: 0,
      orderNo: 0,
      desktopImageUrl: this.desktopImage[0].response?.url,
      link: this.addBannerFrm.value.link,
      mobileImageUrl: this.mobileImage[0].response?.url,
      altText: this.addBannerFrm.value.altText,
      status: this.addBannerFrm.value.status
    };

    this.websiteSettingServices.addBanner(this.banner).subscribe(
      (response) => {
        console.log(response);
        this.isSubmitting = false;
      },
      (error) => {    
        console.log(error);
        this.isSubmitting = false;
      } 
    );
  }
}
