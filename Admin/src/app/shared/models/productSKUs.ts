import { ProductOption } from "./productOption";

export interface ProductSKUs {
    id: number;
    imageUrl: string;
    sku: string;
    barcode: string;
    importPrice: number;
    price: number;
    weight: number;
    quantity: number;
    options: ProductOption[];
}
