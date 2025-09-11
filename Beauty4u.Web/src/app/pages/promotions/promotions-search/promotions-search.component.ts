import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit, ElementRef, Input, Output, EventEmitter, inject } from '@angular/core';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { FormBuilder, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule, NgTemplateOutlet } from '@angular/common';
import { MatInput } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatAccordion, MatExpansionModule, MatExpansionPanel } from '@angular/material/expansion';
import { MatChipsModule, MatChipListbox } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { LoadingService } from 'src/app/services/loading.service';
import { ChildTableComponent } from 'src/app/components/child-table/child-table.component';
import { TableData, CellData, ColumnDef, RowData } from 'src/interfaces/table-data';
import { PromotionService } from 'src/app/services/promotion.service';
import { ModalService } from 'src/app/services/modal.service';
import { SysCodesSelectComponent } from 'src/app/components/syscodes-select/syscodes-select.component';
import { RadioListComponent } from 'src/app/components/radio-list/radio-list.component';
import { PromotionSearchParams } from 'src/interfaces/promotion-search-request';
import { Router } from '@angular/router';
import { FlatpickrDirective } from 'src/directives/flatpickr.directive';
import { StoreAutocompleteComponent } from 'src/app/components/store-autocomplete/store-autocomplete.component';
import { SyscodesAutocompleteComponent } from 'src/app/components/syscodes-autocomplete/syscodes-autocomplete.component';
import { TableComponent } from 'src/app/components/table/table.component';

import { ItemValue } from 'src/interfaces/item-value';
import { StoreSelectComponent } from 'src/app/components/store-select/store-select.component';
import { GetProductPromotionRequest } from 'src/interfaces/get-product-promotion-request';
import { StoreCheckListComponent } from 'src/app/components/store-check-list/store-check-list.component';
import { ConfirmDialogComponent } from 'src/app/pages/ui-components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { PromotionTransferRequest } from 'src/interfaces/promotion-transfer-request';
import { TransferScheduleComponent } from 'src/app/components/transfer-schedule/transfer-schedule.component';

@Component({
  selector: 'app-promotions',
  imports: [
    MatFormFieldModule,
    MatSelectModule,
    CommonModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatAccordion,
    MatExpansionModule,
    MatChipsModule, MatIconModule, MatChipListbox,
    RadioListComponent,
    TableComponent,
    MatLabel,
    MatInput,
    StoreSelectComponent,
    SysCodesSelectComponent,
    MatButtonModule,
    StoreCheckListComponent,
    TransferScheduleComponent
  ],
  templateUrl: './promotions-search.component.html',
  styleUrl: './promotions-search.component.scss'
})
export class PromotionsSearchComponent implements OnInit, AfterViewInit {
  dialog = inject(MatDialog);
  searchForm: FormGroup;
  output!: TableData;
  promoItems?: TableData;
  selectedPromo?: any;
  selectedRow: RowData;
  promoTransferForm: FormGroup;
  isInlineGrouped: boolean = false;
  promoTransferSchedule?: Date;

  @Input() enableRowTransfer = false;
  @Input() enableRowDelete = false;
  @Input() transferLabel = '';

  @Output() rowsSelected = new EventEmitter<any[]>();
  @Output() rowTransferred = new EventEmitter<any>();
  @Output() rowsTransferred = new EventEmitter<any[]>();
  @Output() rowsDeleted = new EventEmitter<any[]>();
  @Output() columnsReady = new EventEmitter<any[]>();

  @ViewChild('promoItemsContent', { static: true }) promoItemsTemplateRef!: TemplateRef<any>;
  @ViewChild('newPromoType', { static: true }) newPromoType!: TemplateRef<any>;
  @ViewChild('promoTransferContent', { static: true }) promoTransferContent!: TemplateRef<any>;
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

    this.promoTransferForm = this.fb.group({
      promoNo: [''],
      storeList: [[]],
      transferMode: [null],
      scheduleDate: '',
      scheduleTime: '',
      mode: ''
    })

    const columns = this.output?.columns;
    this.columnsReady.emit(columns);
  }

  ngAfterViewInit(): void {

  }

  getDisplayedColumns(group: TableData): string[] {
    return group?.columns.map(c => c.fieldName);
  }

  viewItems(rowData: RowData, row: any) {
    this.loadingService.show("Getting promotion..");
    var getProductPromotionRequest: GetProductPromotionRequest = {
      promoNo: row?.promo?.promoNo,
      fromPromoPage: true
    };
    this.selectedRow = rowData;

    this.promotionService.searchProductPromotionsByPromoNo(getProductPromotionRequest).subscribe({
      next: data => {
        this.promoItems = data;
        this.selectedPromo = row.promo;
        this.isInlineGrouped = row.promo.promoRuleCount > 0;
        // this.modalService.openModal(`(${row?.promo?.promoNo}) ${row?.promo?.promoName}`, this.promoItemsTemplateRef, {});
        this.loadingService.hide();
      },
      error: err => {
        console.error('Promotion API error', err);
        this.loadingService.hide();
      }
    });
  }

  clear(): void {
    const today = new Date();
    this.searchForm.reset({
      store: [null],
      fromDate: today,
      toDate: today,
      promoType: '',
      promoNo: '',
      promoName: '',
      promoStatus: { displayText: "Active", value: "active" }
    });

    this.output = { columns: [], tableName: '', rows: [], tableGroups: [] } as TableData;
    this.promoItems = { columns: [], tableName: '', rows: [], tableGroups: [] } as TableData;
    this.selectedPromo = null;
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
        const columns = data?.columns;
        this.columnsReady.emit(columns);
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
    this.modalService.openModal(`Create New Promo`, this.newPromoType, {});
  }

  addByProduct(): void {
    this.router.navigate(['/promotions/promotion']);
    this.modalService.closeModal();
  }
  addByRules(): void {
    this.router.navigate(
      ['/promotions/promotion'],
      { queryParams: { addBy: 'rules' } }
    );
    this.modalService.closeModal();
  }
  editPromo(): void {
    this.router.navigate(
      ['/promotions/promotion'],
      { queryParams: { promono: this.selectedPromo.promoNo } }
    );
    this.modalService.closeModal();
  }

  transferToStores() {
    this.loadingService.show("Transferring products...");

    const promoTransferRequest: PromotionTransferRequest = {
      storeCodes: this.promoTransferForm.value.storeList,
      promoNo: this.promoTransferForm.value.promoNo,
      schedule: this.promoTransferForm.value.transferMode?.dateTime
    };

    this.promotionService.transferPromotion(promoTransferRequest).subscribe({
      next: (data) => {
        this.selectedPromo.storeCodes = data.storeCodes.map((x: any) => x.value);
        this.selectedPromo.storeList = data.storeCode;
        this.selectedRow.cells["StoreList"].textValue = data.storeCode;
        alert("Promo transfer success!")

        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Promo transfer failed:', err);
        alert("Promo transfer failed!")
        this.loadingService.hide();
      }
    });
  }

  transferPromo(): void {
    this.promoTransferForm.reset({
      promoNo: this.selectedPromo.promoNo,
      storeList: [...this.selectedPromo.storeCodes],
    });

    this.modalService.openModal(`Transfer Promotion to Stores`, this.promoTransferContent, {});
  }

  confirmStoreTransfer(): void {
    this.modalService.closeModal();

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Confirm Transfer',
        message: 'Do you want to proceed with the transfer to all the selected stores?',
        icon: 'warning'
      },
      width: '400px',
      panelClass: 'confirm-dialog-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.transferToStores();
      }
    });

    //console.log(this.promoTransferForm.value.storeList);
  }

  onRowsSelected(rows: any[]) {
    this.rowsSelected.emit(rows);
  }

  onRowTransferred(row: any) {
    this.rowTransferred.emit(row);
  }
  onRowsTransferred(rows: any[]) {
    this.rowsTransferred.emit(rows);
  }

  onRowsDeleted(rows: any[]) {
    this.rowsDeleted.emit(rows);
  }


  isTransferDisabled(): boolean {
    const transferMode = this.promoTransferForm.get('transferMode')?.value;

    if (transferMode?.mode === 'schedule') {
      return !transferMode.dateTime;
    }

    return false; // allow "now"
  }

}
