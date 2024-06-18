import { ProductOptionValue } from "./productOptionValues";

export interface ProductOptions {
    productOptionId: number;
    optionName: string;
    productOptionValues: ProductOptionValue[];
    displayedValues: string[];
}
