import { Component, inject, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { SlideImage } from 'src/app/_shared/_models/slideImages';
import { WebsiteService } from '../../website.service';
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

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
  @Input() slide?: SlideImage = inject(NZ_MODAL_DATA);

  addBannerFrm: FormGroup;
  isEdit = false;
  isSubmitting: boolean = false;
  banner: SlideImage;
  apiUrl = environment.apiUrl;

  desktopImage: NzUploadFile[] = [];
  mobileImage: NzUploadFile[] = [];
  previewImage: string | undefined = '';
  previewVisible = false;

  constructor(private websiteSettingServices: WebsiteService, private toastrService: ToastrService, private modal: NzModalRef) {}

  ngOnInit(): void {
    if (this.slide) {
      this.isEdit = true;
      this.addBannerFrm = new FormGroup({
        link: new FormControl(this.slide.link, [Validators.required]),
        altText: new FormControl(this.slide.altText),
        status: new FormControl(this.slide.status),
      });

      this.desktopImage.push(
        {
          uid: '',
          name: '',
          status: 'done',
          url: this.slide.desktopImageUrl,
          response: {
            id: '',
            isMain: true,
            publicId: '',
            url: this.slide.desktopImageUrl
          }
        } as NzUploadFile
      );

      this.mobileImage.push(
        {
          uid: '',
          name: '',
          status: 'done',
          url: this.slide.mobileImageUrl,
          response: {
            id: '',
            isMain: true,
            publicId: '',
            url: this.slide.mobileImageUrl
          }
        } as NzUploadFile
      );
    } else {
      this.addBannerFrm = new FormGroup({
        link: new FormControl('', [Validators.required]),
        altText: new FormControl(''),
        status: new FormControl(true),
      });
    }

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

    if(this.isEdit) {
      this.banner = {
        id: this.slide.id,
        orderNo: this.slide.orderNo,
        desktopImageUrl: this.desktopImage[0].response?.url,
        link: this.addBannerFrm.value.link,
        mobileImageUrl: this.mobileImage[0].response?.url,
        altText: this.addBannerFrm.value.altText,
        status: this.addBannerFrm.value.status
      };

      this.websiteSettingServices.updateSlide(this.banner).subscribe(
        {
          next: (response) => {
            console.log(response);
            this.isSubmitting = false;
            this.toastrService.success('Cập nhật banner thành công');
            this.destroyModal();
          },
          error: (err) => {
            console.log(err);
            this.isSubmitting = false;
          }
        }
      );
    } else {
      this.addNewBanner();
    }
  }

  addNewBanner() {
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
      {
        next: (response) => {
          console.log(response);
          this.isSubmitting = false;
          this.toastrService.success('Thêm banner thành công');
          this.destroyModal();
        },
        error: (error) => {
          console.log(error);
          this.isSubmitting = false;
        }
      }
    );
  }

  destroyModal(): void {
    this.modal.destroy();
  }
}
