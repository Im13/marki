import { Component, EventEmitter, Output } from '@angular/core';
import { CustomerParams } from 'src/app/_shared/_models/customer/customerParams';
import { SlideImage } from 'src/app/_shared/_models/slideImages';
import { WebsiteService } from '../../website.service';
import { ToastrService } from 'ngx-toastr';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AddBannerModalComponent } from '../add-banner-modal/add-banner-modal.component';

@Component({
  selector: 'app-all-banner',
  templateUrl: './all-banner.component.html',
  styleUrls: ['./all-banner.component.css'],
})
export class AllBannerComponent {
  @Output() checkId = new EventEmitter<Set<number>>();

  loading = true;
  slides: readonly SlideImage[] = [];
  customerParams = new CustomerParams();
  totalItems = 0;

  //Order selected
  current = 1;
  checked = false;
  indeterminate = false;
  setOfCheckedId = new Set<number>();

  constructor(private webServices: WebsiteService, private toastrService: ToastrService, private modalServices: NzModalService) {}

  ngOnInit(): void {
    this.getSlides();
  }

  updateCheckedSet(id: number, checked: boolean): void {
    if (checked) {
      this.setOfCheckedId.add(id);
    } else {
      this.setOfCheckedId.delete(id);
    }
  }

  onCurrentPageDataChange(listOfCurrentPageData: readonly SlideImage[]): void {
    this.slides = listOfCurrentPageData;
    this.refreshCheckedStatus();
  }

  refreshCheckedStatus(): void {
    this.checked = this.slides.every(({ id }) =>
      this.setOfCheckedId.has(id)
    );
    this.indeterminate =
      this.slides.some(({ id }) => this.setOfCheckedId.has(id)) &&
      !this.checked;
  }

  onItemChecked(id: number, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.checkId.emit(this.setOfCheckedId);
    this.refreshCheckedStatus();
  }

  onAllChecked(checked: boolean): void {
    this.slides.forEach(({ id }) => this.updateCheckedSet(id, checked));
    this.checkId.emit(this.setOfCheckedId);
    this.refreshCheckedStatus();
  }

  onEditSlide(slide: SlideImage) {
    const modal = this.modalServices.create<AddBannerModalComponent, SlideImage>({
      nzTitle: 'Thiết lập sản phẩm',
      nzContent: AddBannerModalComponent,
      nzCentered: true,
      nzWidth: '70vh',
      nzData: slide
    });

    modal.afterClose.subscribe(() => this.getSlides());
  }

  onPageChange(pageNumber: number) {
    this.getSlides();
  }

  onPageSizeChange(pageSize: number) {
    this.getSlides();
  }

  getSlides() {
    this.loading = true;

    this.webServices.getSlides().subscribe({
      next: (response) => {
        this.slides = response;
        this.loading = false;
      },
        error: (err) => {
          console.log(err);
          this.loading = false;
        }
    });
  }

  shortenUrl(url: string, length: number): string {
    if (!url) return '';
    return url.length > length ? url.substring(0, length) + '...' : url;
  }

  switchStatus(slide: SlideImage) {
    slide.status = !slide.status;

    this.webServices.updateSlide(slide).subscribe({
      next: () => {
        this.loading = false;
        this.toastrService.success('Cập nhật trạng thái thành công');
      },
      error: (err) => {
        console.log(err);
        this.loading = false;
      },
    });
  }

}
