import { Component, Input, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { Product } from 'src/app/shared/models/products';
import { ProductService } from '../../product-service.service';
import { ToastrService } from 'ngx-toastr';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ProductOptions } from 'src/app/shared/models/productOptions';
import { ProductSKUs } from 'src/app/shared/models/productSKUs';
import { ConvertVieService } from 'src/app/core/services/convert-vie.service';
import { ProductOptionValue } from 'src/app/shared/models/productOptionValues';
import { ProductSKUValues } from 'src/app/shared/models/productSKUValues';
import { NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { HttpClient, HttpRequest, HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';

const getBase64 = (file: File): Promise<string | ArrayBuffer | null> =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () =>
      {
        resolve(reader.result);
      }
    reader.onerror = (error) => reject(error);
  });

@Component({
  selector: 'app-add-product-modal',
  templateUrl: './add-product-modal.component.html',
  styleUrls: ['./add-product-modal.component.css'],
})
export class AddProductModalComponent implements OnInit {
  @Input() product?: Product = inject(NZ_MODAL_DATA);

  addForm: FormGroup;
  isEdit?: boolean;
  productOptions: ProductOptions[] = [];
  productOptionId: number = 0;
  currentOptionValueText = '';
  productSKUs: ProductSKUs[] = [];
  editId: number | null = null;
  variantValues: ProductOptionValue[][] = [];
  valueTempId: number = 0;
  popoverVisible = false;

  fallback =
    'data:image/png;base64,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';

  fileList: NzUploadFile[] = [
  ];
  previewImage: string | undefined = '';
  previewVisible = false;

  constructor(
    private modal: NzModalRef,
    private productService: ProductService,
    private toastrService: ToastrService,
    private convertVieService: ConvertVieService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    if (this.product == null) {
      this.product = {
        id: null,
        name: '',
        description: '',
        importPrice: null,
        productBrandId: null,
        productOptions: [],
        productSKU: '',
        productTypeId: null,
        productSkus: [],
      };
    } else {
      this.isEdit = true;

      this.product.productOptions.forEach((option) => {
        const productOption = {
          id: null,
          optionName: option.optionName,
          valuesToDisplay: [],
          productOptionId: option.productOptionId,
          productOptionValues: option.productOptionValues,
        };

        productOption.productOptionValues.forEach((value) => {
          productOption.valuesToDisplay.push(value.valueName);
        });

        this.productOptions.push(productOption);
      });

      this.productSKUs = this.product.productSkus;
    }

    if (this.isEdit == null) this.isEdit = false;

    this.addForm = new FormGroup({
      productName: new FormControl(this.product.name),
      productDescription: new FormControl(this.product.description),
      productTypeId: new FormControl(this.product.productTypeId),
      productBrandId: new FormControl(this.product.productBrandId),
      productSKU: new FormControl(this.product.productSKU),
      importPrice: new FormControl(this.product.importPrice),
    });

    // //Fake data
    // this.productOptions = [
    //   {
    //     optionName: 'Size',
    //     productOptionId: 0,
    //     productOptionValues: [
    //       {
    //         valueTempId: 1,
    //         value: 'S',
    //         valueName: 'Size'
    //       },
    //       {
    //         valueTempId: 2,
    //         value: 'M',
    //         valueName: 'Size'
    //       },
    //       {
    //         valueTempId: 3,
    //         value: 'L',
    //         valueName: 'Size'
    //       }
    //     ]
    //   },
    //   {
    //     optionName: 'Color',
    //     productOptionId: 1,
    //     productOptionValues: [
    //       {
    //         valueTempId: 4,
    //         value: 'White',
    //         valueName: 'Mau sac'
    //       },
    //       {
    //         valueTempId: 5,
    //         value: 'Red',
    //         valueName: 'Mau sac'
    //       },
    //       {
    //         valueTempId: 6,
    //         value: 'Blue',
    //         valueName: 'Mau sac'
    //       }
    //     ]
    //   }
    // ];
  }

  handlePreview = async (file: NzUploadFile): Promise<void> => {
    if (!file.url && !file.preview) {
      file.preview = await getBase64(file.originFileObj!);
    }
    this.previewImage = file.url || file.preview;
    this.previewVisible = true;
  };

  handleUpload = (item: any) => {
    const formData = new FormData();
    formData.append('file', item.file as any, item.name);

    const req = new HttpRequest('POST', item.action, formData, {
      reportProgress : true,
      withCredentials: false
    });

    // return this.http.request(req).subscribe((event: HttpEvent<{}>) => {
    //   if (event.type === HttpEventType.UploadProgress) {
    //     if (event.total > 0) {
    //       (event as any).percent = event.loaded / event.total * 100; // tslint:disable-next-line:no-any
    //     }
    //     // To process the upload progress bar, you must specify the `percent` attribute to indicate progress.
    //     item.onProgress(event, item.file);
    //   } else if (event instanceof HttpResponse) { /* success */
    //     item.onSuccess(event.body, item.file, event);
    //   }
    // },(err) => { /* error */
    //   item.onError(err, item.file);
    //   console.log(err)
    // });


    return this.productService.productImageUpload(formData).subscribe({
      next: (photo) => {
        console.log(photo);
        item.onSuccess(item.file);
      },
      error: err => {
        console.log(err);
      }
    });
  }

  quickAddVariants() {
    this.bindDataToProductObject();
    this.convertValuesToDisplayToProductOptionValues();
    this.product.productOptions = this.productOptions;
    this.productSKUs = this.generateSKUs(this.product);
  }

  convertValuesToDisplayToProductOptionValues() {
    this.productOptions.forEach((option) => {
      const optionValues: ProductOptionValue[] = [];

      option.valuesToDisplay.forEach((value) => {
        optionValues.push({
          value: value,
          valueName: option.optionName,
          valueTempId: this.valueTempId,
        });

        this.valueTempId++;
      });

      option.productOptionValues = optionValues;
    });
  }

  // Hàm tạo các SKUs từ các biến thể của sản phẩm
  generateSKUs(product: Product): ProductSKUs[] {
    // Lấy tất cả các giá trị của các biến thể
    this.variantValues = product.productOptions.map(
      (option) => option.productOptionValues
    );

    // console.log(this.variantValues)

    // Hàm đệ quy để kết hợp các giá trị của các biến thể
    const combine = (
      values: ProductOptionValue[][],
      index: number,
      current: ProductOptionValue[]
    ): ProductOptionValue[][] => {
      if (index === values.length) {
        return [current];
      }

      let result: ProductOptionValue[][] = [];
      for (let value of values[index]) {
        result = result.concat(
          combine(values, index + 1, current.concat(value))
        );
      }
      return result;
    };

    const combinations = combine(this.variantValues, 0, []);

    const skus: ProductSKUs[] = combinations.map((values, skuIndex) => {
      const productSkuValues: ProductSKUValues[] = [];
      let skuName: string = '';

      this.productOptions.forEach((option, index) => {
        productSkuValues.push({
          id: null,
          valueTempId: values[index].valueTempId,
          optionName: option.optionName,
          optionValue: values[index].value,
        });

        skuName += values[index].value;
      });

      return {
        id: null,
        localId: skuIndex + 1,
        barcode: '',
        imageUrl: 'thisisimageurl',
        importPrice: 0,
        sku:
          this.product.productSKU +
          this.convertVieService.removeVietnameseTones(
            skuName.replace(/\s/g, '')
          ),
        quantity: 1,
        price: 1,
        weight: 1,
        productSKUValues: productSkuValues,
      };
    });

    return skus;
  }

  startEdit(id: number): void {
    this.editId = id;
  }

  stopEdit() {
    this.editId = null;
  }

  handleOptionValueKeydown(event: any, data: ProductOptions) {
    if (event.key == 'Tab') {
      event.preventDefault();
    }

    if (event.key == 'Enter') {
      event.preventDefault();
    }
  }

  handleOptionKeydown(event: any, data: ProductOptions) {
    if (event.key == 'Enter') event.preventDefault();

    let index = this.productOptions.findIndex(
      (o) => o.productOptionId == data.productOptionId
    );
    this.productOptions[index].optionName = data.optionName;
  }

  destroyModal(): void {
    this.modal.destroy();
  }

  onSubmit() {
    this.bindDataToProductObject();

    console.log(this.product);

    if (!this.isEdit) {
      this.productService.addProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success('Thêm sản phẩm thành công!');
          this.destroyModal();
        },
        error: (err) => {
          this.toastrService.error(err);
        },
      });
    } else {
      this.productService.editProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success('Sửa sản phẩm thành công!');
          this.destroyModal();
        },
        error: (err) => {
          console.log(err);
          this.toastrService.error(err);
        },
      });
    }
  }

  bindDataToProductObject() {
    this.product.name = this.addForm.value.productName;
    this.product.importPrice = +this.addForm.value.importPrice;
    this.product.productBrandId = 1;
    this.product.description = this.addForm.value.productDescription;
    this.product.productSKU = this.addForm.value.productSKU;
    this.product.productTypeId = 1;
    this.product.productSkus = this.productSKUs;
  }

  onCreateVariants() {
    this.productOptions.push({
      id: null,
      optionName: '',
      productOptionValues: [],
      valuesToDisplay: [],
      productOptionId: this.productOptionId,
    });

    this.productOptionId++;
  }

  drop(event: CdkDragDrop<string[]>): void {
    moveItemInArray(
      this.productOptions,
      event.previousIndex,
      event.currentIndex
    );
  }
}
