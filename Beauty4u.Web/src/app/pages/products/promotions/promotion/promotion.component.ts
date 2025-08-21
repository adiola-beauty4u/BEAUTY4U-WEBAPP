import { Component, OnInit, ViewChild, TemplateRef, ViewChildren, QueryList } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button'; // stepper needs buttons
import { MatInputModule } from '@angular/material/input';   // if you use inputs in steps
import { FormBuilder, FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from 'src/app/services/product.service';
import { LoadingService } from 'src/app/services/loading.service';
import { PromotionService } from 'src/app/services/promotion.service';
import { ModalService } from 'src/app/services/modal.service';
import { MatChipsModule, MatChipListbox } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatAccordion, MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';

import { ItemValue } from 'src/interfaces/item-value';
import { SysCodesSelectComponent } from 'src/app/components/syscodes-select/syscodes-select.component';
import { RadioListComponent } from 'src/app/components/radio-list/radio-list.component';
import { ProductsComponent } from '../../products.component';
import { TableGroup, CellData, ColumnDef, RowData } from 'src/interfaces/table-data';
import { ChildTableComponent } from 'src/app/components/child-table/child-table.component';
import { ItemGroupSelectComponent } from 'src/app/components/item-group-select/item-group-select.component';
import { VendorSelectComponent } from 'src/app/components/vendor-select/vendor-select.component';
import { BrandSelectComponent } from 'src/app/components/brand-select/brand-select.component';
import { PromotionProductSearchParams } from 'src/interfaces/product-for-promotion-search-request';
import { PromotionRule } from 'src/interfaces/promotion-rule';
import { DecimalFormatDirective } from 'src/directives/decimal-format.directive';

@Component({
  selector: 'app-promotion',
  imports: [
    MatStepperModule,
    CommonModule,
    MatCardModule,
    MatIcon,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatChipListbox,
    MatDatepickerModule,
    MatNativeDateModule,
    MatAccordion,
    MatExpansionModule,
    SysCodesSelectComponent,
    RadioListComponent,
    ChildTableComponent,
    ItemGroupSelectComponent,
    VendorSelectComponent,
    BrandSelectComponent,
    DecimalFormatDirective,
    MatStepper
  ],
  templateUrl: './promotion.component.html',
  styleUrl: './promotion.component.scss'
})

export class PromotionComponent implements OnInit {

  promoForm: FormGroup;
  productSearchForm: FormGroup;

  promotionProductsCount = 0;

  promotionRules: {
    promoRuleId: number;
    promoSearchForm: FormGroup;
    productResults: TableGroup;
  }[] = [];

  promotionProductsTable: TableGroup = {
    tableName: '',
    columns: [],
    rows: []
  };

  productPromotions!: TableGroup;

  @ViewChild('productPromoContent', { static: true }) templateRef!: TemplateRef<any>;
  @ViewChild(ItemGroupSelectComponent) itemGroupSelect!: ItemGroupSelectComponent;
  @ViewChild('stepper') private stepper!: MatStepper;
  @ViewChildren(ItemGroupSelectComponent) itemGroupSelects!: QueryList<ItemGroupSelectComponent>;

  constructor(private fb: FormBuilder, private readonly productService: ProductService,
    private readonly loadingService: LoadingService,
    private readonly promotionService: PromotionService,
    private readonly modalService: ModalService) {

  }

  promoStatusItems: ItemValue[] = [
    { displayText: 'Active', value: 'active' },
    { displayText: 'Inactive', value: 'inactive' },
  ];

  ngOnInit(): void {

    this.productSearchForm = this.fb.group({
      vendor: [null],
      itemGroupCode: new FormControl<string>(''),
      styleCode: '',
      styleDesc: '',
      brand: [null],
      color: '',
      size: '',
      upc: '',
      sku: ''
    });

    this.promoForm = this.fb.group({
      promoNo: [''], // optional
      promoName: ['', Validators.required],
      registerDate: [new Date(), Validators.required],
      fromDate: [new Date(), Validators.required],
      toDate: [new Date(), Validators.required],
      promoType: ['', Validators.required],
      promoStatus: [{ displayText: 'Inactive', value: 'inactive' }],
      promoRate: [0.00, Validators.required],
      store: [null]
    })
  }

  clear(): void {
    this.promoForm.reset();
  }

  getPromotions(row: any) {
    this.loadingService.show("Getting promotions..");
    this.promotionService.searchProductPromotionsBySku(row?.product?.sku).subscribe({
      next: data => {
        this.productPromotions = data;
        this.modalService.openModal(row?.product?.styleDesc, this.templateRef, {});
        this.loadingService.hide();
      },
      error: err => {
        console.error('Promotion API error', err);
        this.loadingService.hide();
      }
    });
  }

  viewProducts(): void {

    const request: PromotionProductSearchParams = {
      category: this.productSearchForm.value.itemGroupCode?.code,
      vendorCode: this.productSearchForm.value.vendor?.value,
      brand: this.productSearchForm.value.brand?.value,
      color: this.productSearchForm.value.color,
      size: this.productSearchForm.value.size,
      styleCode: this.productSearchForm.value.styleCode,
      styleDesc: this.productSearchForm.value.styleDesc,
      sku: this.productSearchForm.value.sku,
      upc: this.productSearchForm.value.upc,
      promoRate: this.promoForm.value.promoRate,
      promoType: this.promoForm.value.promoType?.value,
    };

    this.loadingService.show("Searching products...");
    this.productService.searchProductsForPromotion(request).subscribe({
      next: (data) => {
        this.promotionProductsCount = data.rows.length;
        this.promotionProductsTable = data;

        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Promotion Products load failed:', err);
        this.loadingService.hide();
        this.promotionProductsTable.rows = [];
      }
    });
  }

  viewRuleProducts(rule: PromotionRule): void {

    const request: PromotionProductSearchParams = {
      category: rule.promoSearchForm.value.itemGroupCode?.code,
      vendorCode: rule.promoSearchForm.value.vendor?.value,
      brand: rule.promoSearchForm.value.brand?.value,
      color: rule.promoSearchForm.value.color.value,
      size: rule.promoSearchForm.value.size.value,
      styleCode: rule.promoSearchForm.value.styleCode.value,
      styleDesc: rule.promoSearchForm.value.styleDesc.value,
      sku: rule.promoSearchForm.value.sku.value,
      upc: rule.promoSearchForm.value.upc.value,
      promoRate: rule.promoSearchForm.value.promoRate,
      promoType: rule.promoSearchForm.value.promoType?.value,
    };

    this.loadingService.show("Searching products...");
    this.productService.searchProductsForPromotion(request).subscribe({
      next: (data) => {
        rule.productResults = data;

        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Promotion Products load failed:', err);
        this.loadingService.hide();
        rule.productResults = {
          ...rule.productResults,
          rows: []
        };
      }
    });
  }

  getDisplayedColumns(group: TableGroup): string[] {
    return group?.columns.map(c => c.fieldName);
  }

  clearProductSearch(): void {
    this.productSearchForm.reset();
    this.itemGroupSelect.clear();
    this.promotionProductsTable = {
      ...this.promotionProductsTable,
      rows: []
    };
    this.promotionProductsCount = 0;
  }

  clearRuleProductSearch(rule: PromotionRule, index: number): void {
    rule.promoSearchForm.reset({
      vendor: [null],
      itemGroupCode: this.fb.control<string>(''),
      styleCode: this.fb.control<string>(''),
      styleDesc: this.fb.control<string>(''),
      brand: [null],
      color: this.fb.control<string>(''),
      size: this.fb.control<string>(''),
      upc: this.fb.control<string>(''),
      sku: this.fb.control<string>(''),
      promoRate: [this.promoForm.value.promoRate, Validators.required],
      promoType: [this.promoForm.value.promoType, Validators.required],
    });
    const itemGroupSelect = this.itemGroupSelects.get(index);
    itemGroupSelect?.clear();
    rule.productResults = {
      ...rule.productResults,
      rows: []
    };
  }

  onRowDeleted(row: any) {
    this.promotionProductsCount = this.promotionProductsTable.rows.length;
  }

  addPromotionRule(): void {
    this.promotionRules.push({
      promoRuleId: 0,
      promoSearchForm: this.fb.group({
        vendor: [null],
        itemGroupCode: this.fb.control<string>(''),
        styleCode: this.fb.control<string>(''),
        styleDesc: this.fb.control<string>(''),
        brand: [null],
        color: this.fb.control<string>(''),
        size: this.fb.control<string>(''),
        upc: this.fb.control<string>(''),
        sku: this.fb.control<string>(''),
        promoRate: [this.promoForm.value.promoRate, Validators.required],
        promoType: [this.promoForm.value.promoType, Validators.required],
      }),
      productResults: {
        tableName: '',
        columns: [],
        rows: [] = [],
        invalidRows: 0
      } as TableGroup
    });
  }

  allowReviewPromotion(): boolean {
    const hasAnyProduct = this.promotionRules.some(
      rule => rule.productResults.rows && rule.productResults.rows.length > 0
    );

    return hasAnyProduct;
  }

  goToReviewPromotionStep(): void {
    this.promotionProductsTable.columns = this.promotionRules[0].productResults.columns;
    this.promotionProductsTable.rows = this.promotionRules.map(r => r.productResults.rows).flat();

    this.stepper.next();
  }
}
