import { ProductOptionValue } from "./productOptionValue";

export interface ProductSkuValue {
  id: number;
  optionName: string;
  optionValue: string;
  productOptionValue: ProductOptionValue;
}
