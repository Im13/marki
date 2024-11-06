import * as cuid from "cuid";
import { ProductSkuValue } from "./productSkuValue";

export interface BasketItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    sku: string;
    productSKUValues: ProductSkuValue[];
}

export interface Basket {
    id: string;
    items: BasketItem[];
}

export class Basket implements Basket {
    id = cuid();
    items: BasketItem[] = [] 
}

export interface BasketTotals {
    shipping: number;
    subtotal: number;
    total: number;
}