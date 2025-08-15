import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ItemGroupSelectComponent } from 'src/app/components/item-group-select/item-group-select.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { VendorSelectComponent } from 'src/app/components/vendor-select/vendor-select.component';
import { CommonModule, NgTemplateOutlet } from '@angular/common';
import { MatInput } from '@angular/material/input';
import { MatButton } from '@angular/material/button';
import { MatAccordion, MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatChipsModule, MatChipListbox } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { ProductService } from 'src/app/services/product.service';
import { ProductSearchRequest } from 'src/interfaces/product-search-request';
import { LoadingService } from 'src/app/services/loading.service';
import { ChildTableComponent } from 'src/app/components/child-table/child-table.component';
import { TableGroup, CellData, ColumnDef, RowData } from 'src/interfaces/table-data';
import { PromotionService } from 'src/app/services/promotion.service';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-products',
  imports: [ItemGroupSelectComponent,
    MatFormFieldModule,
    MatSelectModule,
    VendorSelectComponent,
    CommonModule,
    ReactiveFormsModule,
    MatInput,
    MatButton,
    MatAccordion,
    MatExpansionModule,
    MatChipsModule, MatIconModule, MatChipListbox,
    ChildTableComponent],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit {
  searchForm: FormGroup;
  output: TableGroup;
  productPromotions: TableGroup;

  defaultPageSize: 10;

  @ViewChild(ItemGroupSelectComponent) itemGroupSelect!: ItemGroupSelectComponent;
  @ViewChild(ChildTableComponent) childTable!: ChildTableComponent;
  @ViewChild('customContent', { static: true }) templateRef!: TemplateRef<any>;
  @ViewChild(MatExpansionPanel) filterPanel!: MatExpansionPanel;

  constructor(private fb: FormBuilder, private readonly productService: ProductService,
    private readonly loadingService: LoadingService,
    private readonly promotionService: PromotionService,
    private readonly modalService: ModalService) {

  }
  ngOnInit(): void {
    this.searchForm = this.fb.group({
      vendor: [null],
      itemGroupCode: new FormControl<string>(''),
      styleCode: '',
      styleDesc: '',
      brand: '',
      color: '',
      size: '',
      upc: '',
      sku: ''
    });
  }

  clear(): void {
    this.searchForm.reset();
    this.itemGroupSelect.clear();
    this.output.rows = [];
    this.childTable.clear();
    this.childTable.refresh();
  }

  search(): void {
    const request: ProductSearchRequest = {
      category: this.searchForm.value.itemGroupCode?.code,
      vendorCode: this.searchForm.value.vendor?.value,
      brand: this.searchForm.value.brand,
      color: this.searchForm.value.color,
      size: this.searchForm.value.size,
      styleCode: this.searchForm.value.styleCode,
      styleDesc: this.searchForm.value.styleDesc,
      sku: this.searchForm.value.sku,
      upc: this.searchForm.value.upc,
    };
    this.loadingService.show("Searching products...");
    this.productService.searchProducts(request).subscribe({
      next: (data) => {
        this.output = data;
        this.filterPanel.close();
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Transfer search failed:', err);
        this.output.rows = [];
        this.loadingService.hide();
      }
    });
  }

  getDisplayedColumns(group: TableGroup): string[] {
    return group?.columns.map(c => c.fieldName);
  }

  getPromotions(row: any) {
    this.loadingService.show("Getting promotions..");
    this.promotionService.searchProductPromotionsBySku(row?.product?.sku).subscribe({
      next: data => {
        this.productPromotions = data;
        this.modalService.openModal(row?.product?.styleDesc, this.templateRef, { });
        this.loadingService.hide();
      },
      error: err => {
        console.error('Promotion API error', err);
        this.loadingService.hide();
      }
    });
  }
}
