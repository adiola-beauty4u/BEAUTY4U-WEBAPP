import { TableGroup } from "./table-data";
import { FormGroup } from "@angular/forms";
export interface PromotionRule {
    promoRuleId: number;
    promoSearchForm: FormGroup;
    productResults: TableGroup;
}