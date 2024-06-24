import { ProductOptionValue } from "./productOptionValues";

export interface ProductOptions {
    id: number;
    productOptionId: number;
    optionName: string;
    // Because of nz-select only return number array, to this value will be used to convert value to productOptionValues
    valuesToDisplay: string[];
    productOptionValues: ProductOptionValue[];
}
