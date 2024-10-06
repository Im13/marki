import { Photo } from "./photo";
import { ProductSKU } from "./productSKU";

export interface Product {
    id: number;
    productSku: string;
    slug: string;
    name: string;
    description: string;
    photos: Photo[];
    productTypeId: number;
    productSkus: ProductSKU[];
}
