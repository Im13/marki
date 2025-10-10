export interface RecommendationDto {
  productId: number;
  productName: string;
  productSlug: string;
  minPrice: number;
  maxPrice: number;
  priceDisplay: string;
  imageUrl: string;
  totalStock: number;
  availableColors: string[];
  availableSizes: string[];
  score: number;
  reasonCode: string;
  reasonText: string;
}

export interface TrackInteractionRequest {
  productId: number;
  skuId?: number;
  durationSeconds?: number;
}

export enum InteractionType {
  View = 1,
  AddToCart = 2,
  Purchase = 3
}