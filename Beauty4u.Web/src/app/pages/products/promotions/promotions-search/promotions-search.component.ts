import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule, NgTemplateOutlet } from '@angular/common';
import { MatInput } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButton } from '@angular/material/button';
import { MatAccordion, MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatChipsModule, MatChipListbox } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { LoadingService } from 'src/app/services/loading.service';
import { ChildTableComponent } from 'src/app/components/child-table/child-table.component';
import { TableGroup, CellData, ColumnDef, RowData } from 'src/interfaces/table-data';
import { PromotionService } from 'src/app/services/promotion.service';
import { ModalService } from 'src/app/services/modal.service';
import { VendorSelectComponent } from 'src/app/components/vendor-select/vendor-select.component';
import { StoreSelectComponent } from 'src/app/components/store-select/store-select.component';
import { SysCodesSelectComponent } from 'src/app/components/syscodes-select/syscodes-select.component';
import { RadioListComponent } from 'src/app/components/radio-list/radio-list.component';
import { PromotionSearchParams } from 'src/interfaces/promotion-search-request';
import { Router } from '@angular/router';

import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-promotions',
  imports: [
    MatFormFieldModule,
    MatSelectModule,
    CommonModule,
    ReactiveFormsModule,
    MatInput,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButton,
    MatAccordion,
    MatExpansionModule,
    MatChipsModule, MatIconModule, MatChipListbox,
    ChildTableComponent,
    StoreSelectComponent,
    SysCodesSelectComponent,
    RadioListComponent
  ],
  templateUrl: './promotions-search.component.html',
  styleUrl: './promotions-search.component.scss'
})
export class PromotionsSearchComponent implements OnInit {
  searchForm: FormGroup;
  output?: TableGroup;
  promoItems?: TableGroup;

  @ViewChild('promoItemsContent', { static: true }) promoItemsTemplateRef!: TemplateRef<any>;
  @ViewChild(MatExpansionPanel) filterPanel!: MatExpansionPanel;

  promoStatusItems: ItemValue[] = [
    { displayText: 'Active', value: 'active' },
    { displayText: 'Inactive', value: 'inactive' },
  ];

  constructor(private fb: FormBuilder,
    private readonly loadingService: LoadingService,
    private readonly promotionService: PromotionService,
    private readonly modalService: ModalService,
    private router: Router) {

  }
  ngOnInit(): void {
    this.searchForm = this.fb.group({
      vendor: [null],
      store: [null],
      fromDate: new Date(),
      toDate: new Date(),
      promoType: '',
      promoNo: '',
      promoName: '',
      promoStatus: { displayText: "Active", value: "active" }
    });
  }


  getDisplayedColumns(group: TableGroup): string[] {
    return group?.columns.map(c => c.fieldName);
  }

  viewItems(row: any) {
    this.loadingService.show("Getting promotion..");
    this.promotionService.searchProductPromotionsByPromoNo(row?.promo?.promoNo).subscribe({
      next: data => {
        this.promoItems = data;
        this.modalService.openModal(`(${row?.promo?.promoNo}) ${row?.promo?.promoName}`, this.promoItemsTemplateRef, {});
        this.loadingService.hide();
      },
      error: err => {
        console.error('Promotion API error', err);
        this.loadingService.hide();
      }
    });
  }

  clear(): void {
    this.searchForm.reset({
      store: [null],
      fromDate: new Date(),
      toDate: new Date(),
      promoType: '',
      promoNo: '',
      promoName: '',
      promoStatus: { displayText: "Active", value: "active" }
    });
    this.output = undefined;
  }
  search(): void {
    const request: PromotionSearchParams = {
      promoNo: this.searchForm.value.promoNo,
      fromDate: this.searchForm.value.fromDate,
      toDate: this.searchForm.value.fromDate,
      promoDescription: this.searchForm.value.promoName,
      promoType: this.searchForm.value.promoType?.value,
      status: this.searchForm.value.promoStatus?.value,
      storeCode: this.searchForm.value.store?.value,
    };
    this.loadingService.show("Getting promotions..");
    this.promotionService.searchPromotions(request).subscribe({
      next: data => {
        this.output = data;
        this.filterPanel.close();
        this.loadingService.hide();
      },
      error: err => {
        console.error('Promotion API error', err);
        this.loadingService.hide();
      }
    });
  }

  new(): void {
    this.router.navigate(['/products/promotion']);
  }
}
