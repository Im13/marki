import { Photo } from "./photo";
import { ProductOption } from "./productOption";
import { ProductSKU } from "./productSKU";

export interface Product {
    id: number;
    productSKU: string;
    slug: string;
    name: string;
    description: string;
    photos: Photo[];
    productTypeId: number;
    productSkus: ProductSKU[];
    productOptions: ProductOption[];
}
