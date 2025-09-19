import { ProductSKUValues } from "./productSKUValues";

export interface ProductSKUDetails {
  id: number;
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
