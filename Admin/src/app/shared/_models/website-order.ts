export interface OrderItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    sku: string;
    optionValueCombination: string;
    productId: number;
}

export interface WebsiteOrder {
    id: number;
    items: OrderItem[];
}