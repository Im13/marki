import { Photo } from "./photo";
import { ProductOptions } from "./productOptions";
import { ProductSKUs } from "./productSKUs";

export interface Product {
  id: number;
  name: string;
  description: string;
  productTypeId: number;
  productSKU: string;
  importPrice: number;
  imageUrl: string;
  totalQuantity?: number;
  productOptions: ProductOptions[];
  productSkus: ProductSKUs[];
  photos: Photo[];
  slug: string;
  style?: string;
  material?: string;
  season?: string;
}
