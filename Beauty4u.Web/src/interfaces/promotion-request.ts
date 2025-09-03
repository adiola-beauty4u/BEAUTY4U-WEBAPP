export interface PromotionRequest {
    promoNo?: string;
    promoName: string;
    promoType: string;
    sumQty?: number;
    sumAmt?: number;
    sumAdd?: number;
    promoRate: number;
    fromDate: Date;
    toDate: Date;
    promoStatus?: boolean;
    promotionItems?: any[];
    promotionRules?: any[];
}