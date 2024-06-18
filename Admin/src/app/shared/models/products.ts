import { ProductOptions } from "./productOptions";
import { ProductSKUs } from "./productSKUs";

export interface Product {
  name: string;
  productSKU: string;
  description: string;
  productTypeId: number;
  productBrandId: number;
  importPrice: number;
  productOptions: ProductOptions[];
  productSKUs: ProductSKUs[];
}
