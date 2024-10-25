import { ProductSkuValue } from "./productSkuValue";

export interface ProductSKU {
  id: number;
  sku: string;
  price: number;
  imageUrl: string;
  barcode: string;
  weight: number;
  quantity: number;
  productSKUValues: ProductSkuValue;
}
