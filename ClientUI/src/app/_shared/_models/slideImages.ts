export interface SlideImage {
    id: number;
    orderNo: number;
    desktopImageUrl: string;
    mobileImageUrl: string;
    tabletImageUrl?: string; // Optional tablet image
    link: string;
    altText: string;
    status: boolean;
}
