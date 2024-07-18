import { ProductSKUValues } from "./productSKUValues";

export interface ProductSKUDetails {
  sku: string;
  quantity: number;
  price: number;
  importPrice: number;
  barcode: string;
  imageUrl: string;
  weight: number;
  productName: string;
  productSKU: string;
  productSKUValues: ProductSKUValues[];
}
