export interface User {
    email: string;
    displayName: string;
    token: string;
}

export interface Address {
    fullName: string;
    cityOrProvinceId: number;
    districtId: number;
    wardId: number;
    street: string;
}