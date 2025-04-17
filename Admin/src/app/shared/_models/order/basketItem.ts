import { ProductSkuValue } from "./productSkuValue";

export interface BasketItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    sku: string;
    productSKUValues: ProductSkuValue[];
    optionValueCombination: string;
    productId: number;
}
