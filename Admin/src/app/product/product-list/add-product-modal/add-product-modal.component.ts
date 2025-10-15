import { Component, Input, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { Product } from 'src/app/_shared/_models/products';
import { ProductService } from '../../product-service.service';
import { ToastrService } from 'ngx-toastr';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ProductOptions } from 'src/app/_shared/_models/productOptions';
import { ProductSKUs } from 'src/app/_shared/_models/productSKUs';
import { ProductOptionValue } from 'src/app/_shared/_models/productOptionValues';
import { ProductSKUValues } from 'src/app/_shared/_models/productSKUValues';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { Photo } from 'src/app/_shared/_models/photo';
import { ProductType } from 'src/app/_shared/_models/productTypes';
import { ConvertVieService } from 'src/app/_core/_services/convert-vie.service';
import { SyncService } from './sync-values/sync.service';

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
  selector: 'app-add-product-modal',
  templateUrl: './add-product-modal.component.html',
  styleUrls: ['./add-product-modal.component.css'],
})
export class AddProductModalComponent implements OnInit {
  @Input() product?: Product = inject(NZ_MODAL_DATA);

  addForm: FormGroup;
  isEdit?: boolean;
  isSubmitting: boolean = false;
  productOptions: ProductOptions[] = [];
  productOptionId: number = 0;
  currentOptionValueText = '';
  productSKUs: ProductSKUs[] = [];
  editId: number | null = null;
  variantValues: ProductOptionValue[][] = [];
  valueTempId: number = 0;
  popoverVisible = false;
  productTypes: ProductType[];
  selectedProductTypeId: number;
  isProductTypeLoading = false;
  popupVisible: boolean = false;

  skuPhotoList: NzUploadFile[] = [];
  photoList: NzUploadFile[] = [];
  mainPhoto: NzUploadFile[] = [];
  previewImage: string | undefined = '';
  previewVisible = false;

  productImages: Photo[] = [];

  constructor(
    private modal: NzModalRef,
    private productService: ProductService,
    private toastrService: ToastrService,
    private convertVieService: ConvertVieService,
    private syncService: SyncService
  ) { }

  ngOnInit(): void {
    if (this.isEdit == null) this.isEdit = false;
    this.getProductTypes();

    if (this.product == null) {
      this.product = {
        id: null,
        name: '',
        description: '',
        importPrice: null,
        productOptions: [],
        productSKU: '',
        productTypeId: null,
        productSkus: [],
        imageUrl: '',
        photos: [],
        slug: ''
      };
    } else {
      this.isEdit = true;
      this.selectedProductTypeId = this.product.productTypeId;

      this.photoList = this.product.photos.filter(p => p.isMain == false).map(p => {
        return {
          uid: p.id.toString(),
          name: p.publicId,
          status: 'done',
          url: p.url,
          response: p
        } as NzUploadFile
      });

      this.mainPhoto = this.product.photos.filter(p => p.isMain == true).map(p => {
        return {
          uid: p.id.toString(),
          name: p.publicId,
          status: 'done',
          url: p.url,
          response: {
            id: p.id,
            isMain: true,
            publicId: p.publicId,
            url: p.url
          }
        } as NzUploadFile
      })

      this.regenerateValueTempIds();
    }

    this.addForm = new FormGroup({
      productName: new FormControl(this.product.name, [Validators.required]),
      productDescription: new FormControl(this.product.description),
      productTypeId: new FormControl(this.product.productTypeId, [Validators.required]),
      productSKU: new FormControl(this.product.productSKU, [Validators.required]),
      importPrice: new FormControl(this.product.importPrice),
      productStyle: new FormControl(this.product.style),
      productMaterial: new FormControl(this.product.material),
      productSeason: new FormControl(this.product.season),
    });
  }

  getProductTypes() {
    this.productService.getAllProductTypes().subscribe({
      next: types => {
        this.productTypes = types;
      },
      error: err => {
        console.log(err);
      }
    });
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

    return this.productService.productImageUpload(formData).subscribe({
      next: (photo: Photo) => {
        item.onSuccess(item.file);
        this.product.productSkus.forEach(sku => {
          sku.photos.push(photo);
        });
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
    this.bindDataToProductObject();
  }

  convertValuesToDisplayToProductOptionValues() {
    this.productOptions.forEach((option) => {
      const optionValues: ProductOptionValue[] = [];

      option.valuesToDisplay.forEach((value) => {
        optionValues.push({
          value: value,
          valueName: value,
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
        imageUrl: '',
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
        photos: []
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
    this.isSubmitting = true;
    this.groupProductImages();
    this.bindDataToProductObject();

    if (this.addForm.valid) {
      if (!this.isEdit) {
        this.bindDataToProductObject();

        if (this.product.slug == '') {
          this.product.slug = this.convertToSlug(this.product.name);
        }

        this.productService.addProduct(this.product).subscribe({
          next: () => {
            this.toastrService.success('Thêm sản phẩm thành công!');
            this.destroyModal();
          },
          error: (err) => {
            this.toastrService.error(err);
            this.isSubmitting = false;
          },
        });
      } else {
        const updatePayload = this.prepareUpdatePayload();

        this.productService.editProduct(this.product.id, updatePayload).subscribe({
          next: () => {
            this.toastrService.success('Sửa sản phẩm thành công!');
            this.destroyModal();
          },
          error: (err) => {
            this.toastrService.error(err.error?.message || err.message || 'Có lỗi xảy ra');
            this.isSubmitting = false;
          },
        });
      }
    }
  }

  prepareUpdatePayload() {
    this.convertValuesToDisplayToProductOptionValues();

    const productOptions = this.productOptions.map(option => ({
      optionName: option.optionName,
      productOptionValues: (option.productOptionValues || []).map(val => ({
        valueName: val.valueName || val.value,
        valueTempId: val.valueTempId || 0  // ← Ensure valueTempId exists
      }))
    }));

    const valueTempIdMap = new Map<string, number>();
    productOptions.forEach(opt => {
      opt.productOptionValues.forEach(val => {
        const key = `${opt.optionName}::${val.valueName}`;
        valueTempIdMap.set(key, val.valueTempId);
      });
    });

    const productSKUs = this.productSKUs.map(sku => {
      const skuValues = (sku.productSKUValues || []).map(val => {
        // ✅ Build key from optionName + optionValue
        const key = `${val.optionName}::${val.optionValue}`;
        const valueTempId = valueTempIdMap.get(key) || 0;

        return {
          valueTempId: valueTempId
        };
      });

      return {
        sku: sku.sku,
        barcode: sku.barcode || '',
        quantity: +sku.quantity || 0,
        price: +sku.price || 0,
        importPrice: +sku.importPrice || 0,
        weight: +sku.weight || 0,
        imageUrl: sku.imageUrl || '',
        productSKUValues: skuValues,
        photos: (sku.photos || []).map(photo => ({
          url: photo.url,
          isMain: photo.isMain || false,
          publicId: photo.publicId || ''
        }))
      };
    });

    const payload = {
      name: this.addForm.value.productName,
      description: this.addForm.value.productDescription || '',
      productSKU: this.addForm.value.productSKU,
      importPrice: +this.addForm.value.importPrice || 0,
      slug: this.product.slug || this.convertToSlug(this.addForm.value.productName),
      productTypeId: this.addForm.value.productTypeId,
      style: this.addForm.value.productStyle || '',
      season: this.addForm.value.productSeason || '',
      material: this.addForm.value.productMaterial || '',
      productOptions: productOptions,
      productSKUs: productSKUs,
      photos: this.productImages.map(photo => ({
        url: photo.url,
        isMain: photo.isMain || false,
        publicId: photo.publicId || ''
      }))
    };

    return payload;
  }

  convertToSlug(productName: string): string {
    return productName
      .toLowerCase() // Chuyển về chữ thường
      .normalize("NFD") // Chuẩn hóa Unicode để tách dấu
      .replace(/[\u0300-\u036f]/g, "") // Xóa dấu
      .replace(/đ/g, "d") // Chuyển đ -> d
      .replace(/[^a-z0-9\s-]/g, "") // Xóa ký tự đặc biệt
      .trim() // Xóa khoảng trắng thừa
      .replace(/\s+/g, "-"); // Thay khoảng trắng bằng dấu "-"
  }


  bindDataToProductObject() {
    this.product.name = this.addForm.value.productName;
    this.product.importPrice = +this.addForm.value.importPrice;
    this.product.description = this.addForm.value.productDescription;
    this.product.productSKU = this.addForm.value.productSKU;
    this.product.productTypeId = this.addForm.value.productTypeId;
    this.product.productSkus = this.productSKUs;
    this.product.photos = this.productImages;
    this.product.style = this.addForm.value.productStyle;
    this.product.material = this.addForm.value.productMaterial;
    this.product.season = this.addForm.value.productSeason;

    this.convertValuesToDisplayToProductOptionValues();
    this.product.productOptions = this.productOptions;

    this.product.productSkus = this.productSKUs;
    this.product.photos = this.productImages;
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

  handleUploadProductChange(info: { file: NzUploadFile }, isMainPhoto: boolean): void {
    if (info.file.status === 'done') {
      console.log(info)
    }
  }

  groupProductImages() {
    this.productImages = this.photoList.map(
      file => {
        if (file.response) {
          return {
            ...file.response,
            isMain: false
          } as Photo;
        } else {

        }

        return undefined;
      }
    ).filter(photo => photo != undefined) as Photo[];

    const mainImage = {
      ...this.mainPhoto[0].response,
      isMain: true
    } as Photo;

    this.productImages.push(mainImage);
  }

  private regenerateValueTempIds() {
    let tempIdCounter = 1;

    this.productOptions = [];

    if (this.product.productOptions && this.product.productOptions.length > 0) {
      this.product.productOptions.forEach((option) => {
        const productOption = {
          id: option.id,
          optionName: option.optionName,
          valuesToDisplay: [],
          productOptionId: option.productOptionId,
          productOptionValues: [],
        };

        if (option.productOptionValues && option.productOptionValues.length > 0) {
          option.productOptionValues.forEach((value) => {
            const newValueTempId = tempIdCounter++;

            const optionValue = {
              valueName: value.valueName,
              valueTempId: newValueTempId,  // ← New ID
              value: value.valueName
            };

            productOption.valuesToDisplay.push(value.valueName);
            productOption.productOptionValues.push(optionValue);
          });
        }

        this.productOptions.push(productOption);
      });
    }

    this.valueTempId = tempIdCounter;

    const valueMapping = new Map<string, number>();

    this.productOptions.forEach(option => {
      option.productOptionValues.forEach(value => {
        const key = `${option.optionName}::${value.valueName}`;
        valueMapping.set(key, value.valueTempId);
      });
    });

    this.productSKUs = [];

    if (this.product.productSkus && this.product.productSkus.length > 0) {
      this.product.productSkus.forEach(sku => {
        const newSKU = {
          id: sku.id,
          localId: sku.localId,
          sku: sku.sku,
          barcode: sku.barcode,
          quantity: sku.quantity,
          price: sku.price,
          importPrice: sku.importPrice,
          weight: sku.weight,
          imageUrl: sku.imageUrl,
          photos: sku.photos || [],
          productSKUValues: []
        };

        // Update ProductSKUValues with new valueTempIds
        if (sku.productSKUValues && sku.productSKUValues.length > 0) {
          sku.productSKUValues.forEach(skuValue => {
            // Find matching valueTempId by optionName + optionValue
            const key = `${skuValue.optionName}::${skuValue.optionValue}`;
            const newValueTempId = valueMapping.get(key);

            if (newValueTempId) {
              newSKU.productSKUValues.push({
                id: skuValue.id,
                valueTempId: newValueTempId,  // ← Updated!
                optionName: skuValue.optionName,
                optionValue: skuValue.optionValue
              });
            } else {
              console.warn(`Cannot find valueTempId for: ${key}`);
            }
          });
        }

        this.productSKUs.push(newSKU);
      });
    }
  }

  removeOption(option: ProductOptions): void {
    this.productOptions = this.productOptions.filter(o => {
      if (this.isEdit) {
        return o.id !== option.id;
      }
      return o.productOptionId !== option.productOptionId;
    });

    if (this.productOptions.length > 0) {
      this.quickAddVariants();
    } else {
      this.productSKUs = [];
    }
  }

  // ✅ NEW METHOD: Sync SKU field value to all SKUs
  syncSKUField(fieldKey: string, value: any): void {
    if (!this.syncService.shouldSync(fieldKey)) {
      return; // Don't sync if not enabled
    }

    // Sync to all SKUs
    this.productSKUs.forEach(sku => {
      if (fieldKey === 'photos') {
        // Special handling for photos (deep copy)
        sku[fieldKey] = JSON.parse(JSON.stringify(value));
      } else {
        sku[fieldKey] = value;
      }
    });

    console.log(`✅ Synced ${fieldKey} to ${this.productSKUs.length} SKUs`);
  }

  // ✅ NEW METHOD: Handle input blur events
  onSKUFieldBlur(sku: ProductSKUs, fieldKey: string): void {
    const value = sku[fieldKey];
    this.syncSKUField(fieldKey, value);
  }

  // ✅ NEW METHOD: Handle photo change for SKU
  onSKUPhotoChange(sku: ProductSKUs): void {
    if (this.syncService.shouldSync('photos')) {
      const photos = sku.photos;
      this.productSKUs.forEach(s => {
        if (s !== sku) {
          s.photos = JSON.parse(JSON.stringify(photos));
        }
      });
      console.log(`✅ Synced photos to ${this.productSKUs.length} SKUs`);
    }
  }
}
