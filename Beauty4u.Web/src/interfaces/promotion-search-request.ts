export interface PromotionSearchParams {
    promoNo?: string;
    fromDate?: Date | null;
    promoDescription?: string;
    promoType?: string;
    status?: string;
    storeCode?: string;
    toDate?: Date | null;
}