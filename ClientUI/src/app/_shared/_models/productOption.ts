import { ProductOptionValue } from "./productOptionValue";

export interface ProductOption {
  id: number;
  optionName: string;
  productOptionValues: ProductOptionValue[];
}
