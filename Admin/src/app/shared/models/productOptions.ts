import { ProductOptionValue } from "./productOptionValues";

export interface ProductOptions {
    productOptionId: number;
    optionName: string;
    optionValues: ProductOptionValue[];
    displayedValues: string[];
}
