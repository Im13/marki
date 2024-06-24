import { ProductSKUValue } from "./productSKUValue";

export interface ProductSKUs {
    id: number;
    localId: number;
    imageUrl: string;
    sku: string;
    barcode: string;
    importPrice: number;
    price: number;
    weight: number;
    quantity: number;
    productSKUValues: ProductSKUValue[];
}
