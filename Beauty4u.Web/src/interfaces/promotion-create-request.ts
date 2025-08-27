export interface PromotionCreateRequest {
    promoName: string;
    promoType: string;
    promoRate: number;
    fromDate: Date;
    toDate: Date;
    promotionItems?: any[];
    promotionRules?: any[];
}