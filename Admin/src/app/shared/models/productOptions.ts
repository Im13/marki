import { ProductOptionValue } from "./productOptionValues";

export interface ProductOptions {
    productOptionId: number;
    optionName: string;
    // Because of nz-select only return number array, to this value will be used to convert value to productOptionValues
    valuesToDisplay: string[];
    productOptionValues: ProductOptionValue[];
}
