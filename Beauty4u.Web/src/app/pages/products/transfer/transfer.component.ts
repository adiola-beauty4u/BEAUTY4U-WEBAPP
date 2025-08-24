import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatStepperModule } from '@angular/material/stepper';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule, FormBuilder } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialog } from '@angular/material/dialog';

import { StoreDto } from 'src/interfaces/store';
import { StoreService } from 'src/app/services/store.service';
import { DateSearchRequest } from 'src/interfaces/date-search-request';
import { ProductService } from 'src/app/services/product.service';
import { TransferProductSearchResult } from 'src/interfaces/transfer-product-search';
import { ExpandableTableComponent } from 'src/app/components/expandable-table/expandable-table.component';
import { LoadingService } from 'src/app/services/loading.service';
import { ConfirmDialogComponent } from 'src/app/pages/ui-components/confirm-dialog/confirm-dialog.component';
import { ProductTransferRequest } from 'src/interfaces/product-transfer-request';
import { MultiTableComponent } from 'src/app/components/multi-table/multi-table.component';
import { TableData } from 'src/interfaces/table-data';

export interface StoreWithSelection extends StoreDto {
  selected: boolean;
}

@Component({
  selector: 'app-transfer',
  imports: [MatCardModule,
    MatStepperModule,
    MatIcon,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    FormsModule,
    MatNativeDateModule,
    ExpandableTableComponent,
    MatCheckboxModule,
    CommonModule],
  templateUrl: './transfer.component.html',
  styleUrl: './transfer.component.scss'
})
export class TransferComponent implements OnInit {

  dialog = inject(MatDialog);

  products: TransferProductSearchResult[] = [];

  fromDate = new Date();
  toDate = new Date();

  productList: any[] = [];
  tableGroupValues: any[] = [];
  tablePreviewValues: any[] = [];
  tableGroupResultValues: any[] = [];
  productSearchResult: TableData;

  stores: StoreWithSelection[] = [];
  allSelected = false;

  @ViewChild('stepper') private stepper!: MatStepper

  constructor(private productService: ProductService, private storeService: StoreService, private fb: FormBuilder, private loadingService: LoadingService) {

  }


  ngOnInit(): void {
    this.storeService.getStores().subscribe({
      next: data => {
        this.stores = data.map(store => ({ ...store, selected: false }));
      },
      error: err => console.error('Store API error', err)
    });

  }

  searchTransfer() {
    const request: DateSearchRequest = {
      dateStart: this.fromDate.toISOString().split('T')[0],
      dateEnd: this.toDate.toISOString().split('T')[0]
    };
    this.loadingService.show("Searching products...");
    this.productService.transferSearch(request).subscribe({
      next: (data) => {
        this.productSearchResult = data.tableData;
        this.productList = data.products;
        this.tableGroupValues = data.tableData.tableGroups;
        this.products = data;
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Transfer search failed:', err);
        this.products = [];
        this.loadingService.hide();
      }
    });

  }

  clearForm() {
    this.fromDate = new Date();
    this.toDate = new Date();
    this.products = [];
    this.tableGroupValues = [];
    this.stores.forEach(store => store.selected = false);
    this.allSelected = false;
  }

  clear() {

  }

  previewTransfer() {
    this.loadingService.show("Searching products...");

    const transferProductRequest: ProductTransferRequest = {
      StoreCodes: this.getSelectedStoreCodes(),
      UPCList: this.productList.map(x => x.upc),
      SkuList: this.productList.map(x => x.sku),
    };

    this.productService.transferPreview(transferProductRequest).subscribe({
      next: (data) => {
        console.log(data);
        this.tablePreviewValues = data.tableGroups;
        this.nextStep();
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Transfer search failed:', err);
        this.products = [];
        this.loadingService.hide();
      }
    });
  }

  nextStep() {
    this.stepper.next();
  }
  previousStep() {
    this.stepper.previous();
  }

  toggleAll() {
    this.stores.forEach(store => store.selected = this.allSelected);
  }

  getSelectedStoreCodes() {
    return this.stores.filter(x => x.selected).map(x => x.code);
  }

  confirmTransfer() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Confirm Transfer',
        message: 'Do you want to proceed with the transfer to all the selected stores?',
        icon: 'warning' // optional: pass 'info', 'error', etc.
      },
      width: '400px',
      panelClass: 'confirm-dialog-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.transferToStores();
      }
    });
  }

  transferToStores() {
    this.loadingService.show("Transferring products...");

    const transferProductRequest: ProductTransferRequest = {
      StoreCodes: this.getSelectedStoreCodes(),
      UPCList: this.productList.map(x => x.upc),
      SkuList: this.productList.map(x => x.sku)
    };

    this.productService.transferToStores(transferProductRequest).subscribe({
      next: (data) => {
        console.log(data);
        this.tableGroupResultValues = data.tableGroups;
        this.nextStep();
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Transfer search failed:', err);
        this.products = [];
        this.loadingService.hide();
      }
    });
  }

  reset() {
    this.clearForm();
    this.stepper.reset();
  }
}
