import { ProductOption } from "./productOption";
import { ProductSKUValue } from "./productSKUValue";

export interface ProductSKUs {
    id: number;
    imageUrl: string;
    sku: string;
    barcode: string;
    importPrice: number;
    price: number;
    weight: number;
    quantity: number;
    productSKUValues: ProductSKUValue[];
}
