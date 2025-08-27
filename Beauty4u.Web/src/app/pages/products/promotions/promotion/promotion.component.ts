import { Component, OnInit, ViewChild, TemplateRef, ViewChildren, QueryList, inject } from '@angular/core';
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
import { ActivatedRoute, Router } from '@angular/router';

import { ItemValue } from 'src/interfaces/item-value';
import { SysCodesSelectComponent } from 'src/app/components/syscodes-select/syscodes-select.component';
import { RadioListComponent } from 'src/app/components/radio-list/radio-list.component';
import { ProductsComponent } from '../../products.component';
import { TableData, CellData, ColumnDef, RowData } from 'src/interfaces/table-data';
import { ChildTableComponent } from 'src/app/components/child-table/child-table.component';
import { ItemGroupSelectComponent } from 'src/app/components/item-group-select/item-group-select.component';
import { VendorSelectComponent } from 'src/app/components/vendor-select/vendor-select.component';
import { BrandSelectComponent } from 'src/app/components/brand-select/brand-select.component';
import { PromotionProductSearchParams } from 'src/interfaces/product-for-promotion-search-request';
import { PromotionRule } from 'src/interfaces/promotion-rule';
import { DecimalFormatDirective } from 'src/directives/decimal-format.directive';
import { TableComponent } from 'src/app/components/table/table.component';
import { PromotionCreateRequest } from 'src/interfaces/promotion-create-request';
import { ConfirmDialogComponent } from 'src/app/pages/ui-components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { SelectControlComponent } from 'src/app/components/select-control/select-control.component';

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
    ItemGroupSelectComponent,
    VendorSelectComponent,
    BrandSelectComponent,
    DecimalFormatDirective,
    MatStepper,
    TableComponent,
    SelectControlComponent
  ],
  templateUrl: './promotion.component.html',
  styleUrl: './promotion.component.scss'
})

export class PromotionComponent implements OnInit {
  dialog = inject(MatDialog);
  promoForm: FormGroup;
  productSearchForm: FormGroup;

  transferLabel = 'Add';

  addProductsByRules: boolean = false;

  promotionRules: {
    promoRuleId: number;
    promoSearchForm: FormGroup;
    productResults: TableData;
  }[] = [];

  promotionProductsTable: TableData = {
    tableName: '',
    columns: [],
    rows: [],
    tableGroups: []
  };

  selectedProducts: TableData = {
    tableName: '',
    columns: [],
    rows: [],
    tableGroups: []
  };

  retailPriceConditions: ItemValue[] = [
    { displayText: 'Equal to', value: '=' },
    { displayText: 'Greater than', value: '>' },
    { displayText: 'Greater than or Equal to', value: '>=' },
    { displayText: 'Less than', value: '<' },
    { displayText: 'Less than or equal to', value: '<=' },
  ];

  productPromotions!: TableData;
  selectedProduct: any;

  @ViewChild('productPromoContent', { static: true }) templateRef!: TemplateRef<any>;
  @ViewChild(ItemGroupSelectComponent) itemGroupSelect!: ItemGroupSelectComponent;
  @ViewChild('stepper') private stepper!: MatStepper;
  @ViewChildren(ItemGroupSelectComponent) itemGroupSelects!: QueryList<ItemGroupSelectComponent>;

  constructor(private fb: FormBuilder, private readonly productService: ProductService,
    private readonly loadingService: LoadingService,
    private readonly promotionService: PromotionService,
    private readonly modalService: ModalService,
    private route: ActivatedRoute,
    private router: Router) {

  }

  promoStatusItems: ItemValue[] = [
    { displayText: 'Active', value: 'active' },
    { displayText: 'Inactive', value: 'inactive' },
  ];

  ngOnInit(): void {
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

    this.route.queryParamMap.subscribe(params => {
      this.addProductsByRules = (params.get('addBy') ?? '') == 'rules';
    });

    this.productSearchForm = this.fb.group({
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
      retailPriceCondition: [null],
      retailPriceRate: this.fb.control<string>(''),
    });
  }

  clear(): void {
    this.promoForm.reset();
  }

  getPromotions(row: any) {
    this.loadingService.show("Getting promotions..");
    this.promotionService.searchProductPromotionsBySku(row?.product?.sku).subscribe({
      next: data => {
        this.productPromotions = data;
        this.selectedProduct = row.product;
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
      promoRate: this.productSearchForm.value.promoRate,
      promoType: this.productSearchForm.value.promoType?.value,
    };

    if (this.productSearchForm.value.retailPriceCondition?.value && this.productSearchForm.value.retailPriceRate) {
      request.retailPrice = this.productSearchForm.value.retailPriceCondition?.value + ' ' + this.productSearchForm.value.retailPriceRate;
    }

    this.loadingService.show("Searching products...");
    this.productService.searchProductsForPromotion(request).subscribe({
      next: (data) => {
        this.promotionProductsTable = data;
        this.selectedProducts.columns = data.columns;
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
      color: rule.promoSearchForm.value.color?.value,
      size: rule.promoSearchForm.value.size?.value,
      styleCode: rule.promoSearchForm.value.styleCode?.value,
      styleDesc: rule.promoSearchForm.value.styleDesc?.value,
      sku: rule.promoSearchForm.value.sku?.value,
      upc: rule.promoSearchForm.value.upc?.value,
      promoRate: rule.promoSearchForm.value.promoRate,
      promoType: rule.promoSearchForm.value.promoType?.value,
    };

    if (rule.promoSearchForm.value.retailPriceCondition?.value && rule.promoSearchForm.value.retailPriceRate) {
      request.retailPrice = rule.promoSearchForm.value.retailPriceCondition?.value + ' ' + rule.promoSearchForm.value.retailPriceRate;
    }

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

  getDisplayedColumns(group: TableData): string[] {
    return group?.columns.map(c => c.fieldName);
  }

  clearProductSearch(): void {
    this.productSearchForm.reset();
    this.itemGroupSelect.clear();
    this.promotionProductsTable = {
      ...this.promotionProductsTable,
      rows: []
    };
  }

  clearRuleProductSearch(rule: PromotionRule, index: number): void {
    this.productSearchForm.reset();
    this.itemGroupSelect.clear();
    this.promotionProductsTable = {
      ...this.promotionProductsTable,
      rows: []
    };

    rule.promoSearchForm.reset();
    const itemGroupSelect = this.itemGroupSelects.get(index);
    itemGroupSelect?.clear();
    rule.productResults = {
      ...rule.productResults,
      rows: []
    };
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
        retailPriceCondition: [null],
        retailPriceRate: this.fb.control<string>(''),
      }),
      productResults: {
        tableName: '',
        columns: [],
        rows: [],
        tableGroups: []
      } as TableData
    });
  }

  deletePromotionRule(rule: any): void {
    const index = this.promotionRules.indexOf(rule);
    if (index >= 0) {
      this.promotionRules.splice(index, 1);
    }
  }

  allowReviewPromotion(): boolean {

    if (!this.addProductsByRules) {
      return this.selectedProducts?.rows?.length > 0;
    }

    const hasAnyProduct = this.promotionRules.every(
      rule => rule.productResults.rows && rule.productResults.rows.length > 0
    );


    return hasAnyProduct;
  }

  goToReviewPromotionStep(): void {
    if (!this.addProductsByRules) {
      this.stepper.next();
    } else {
      this.promotionProductsTable.columns = this.promotionRules[0].productResults.columns;
      this.promotionProductsTable.rows = this.promotionRules.map(r => r.productResults.rows).flat();

      this.stepper.next();
    }
  }

  onRowTransfer(row: any) {
    if (!this.selectedProducts.rows.some(r => r.rowKey === row.rowKey)) {
      this.selectedProducts.rows.push(row);
    }
  }

  onRowsTransfer(rows: any[]) {
    const newRows = rows.filter(row =>
      !this.selectedProducts.rows.some(r => r.rowKey === row.rowKey)
    );

    this.selectedProducts.rows = [
      ...this.selectedProducts.rows,
      ...newRows
    ];
  }

  onProductColumnsReady(cols: any[]) {
    this.selectedProducts.columns = cols;
  }

  onRowsDeleted(rows: any[]) {
    const remainingRows = this.selectedProducts.rows.filter(
      row => !rows.includes(row)
    );

    this.selectedProducts = { ...this.selectedProducts, rows: remainingRows };
  }

  confirmCreatePromo() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Confirm Promo Creation',
        message: 'Create promotion?',
        icon: 'warning' // optional: pass 'info', 'error', etc.
      },
      width: '400px',
      panelClass: 'confirm-dialog-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.createPromo();
      }
    });
  }

  createPromo() {
    var promotionCreateRequest: PromotionCreateRequest = {
      promoName: this.promoForm.value.promoName,
      fromDate: this.promoForm.value.fromDate,
      toDate: this.promoForm.value.toDate,
      promoRate: this.promoForm.value.promoRate,
      promoType: this.promoForm.value.promoType?.value,
    };
    if (this.addProductsByRules) {
      promotionCreateRequest.promotionRules = this.promotionRules.map(rule => {
        const formValue = rule.promoSearchForm.value;
        return {
          promoType: formValue.promoType?.value,
          promoRate: formValue.promoRate,
          vendor: formValue.vendor?.value,
          itemGroup: formValue.itemGroupCode?.code,
          brand: formValue.brand?.value,
          retailPriceCondition: formValue.retailPriceCondition?.value,
          retailPriceRate: formValue.retailPriceRate || 0,
        };
      });
    } else {
      promotionCreateRequest.promotionItems = this.selectedProducts.rows.map(row => { return row.additionalData });
    }

    this.loadingService.show("Saving promotion...");
    this.promotionService.createPromotion(promotionCreateRequest).subscribe({
      next: (data) => {
        alert("Promotion successfully saved.")
        this.loadingService.hide();
        this.router.navigate(['/products/promotions']);
      },
      error: (err) => {
        console.error('Promotion create failed:', err);
        this.loadingService.hide();
      }
    });
  }
}
