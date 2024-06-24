import { ProductOptions } from "./productOptions";
import { ProductSKUs } from "./productSKUs";

export interface Product {
  id: number;
  name: string;
  description: string;
  productBrandId: number;
  productTypeId: number;
  productSKU: string;
  importPrice: number;
  productOptions: ProductOptions[];
  productSkus: ProductSKUs[];
}
