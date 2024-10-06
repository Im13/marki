import { Photo } from "./photo";

export interface Product {
    id: number;
    productSku: string;
    slug: string;
    name: string;
    description: string;
    photos: Photo[];
    productTypeId: number;
}