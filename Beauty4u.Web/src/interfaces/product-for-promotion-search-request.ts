import { ProductSearchRequest } from "./product-search-request"
export interface PromotionProductSearchParams extends ProductSearchRequest {
    promoType: string,
    promoRate: number
}