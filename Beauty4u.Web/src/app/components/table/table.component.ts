import { Component, Input, OnInit, OnChanges, SimpleChanges, TemplateRef, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableData, ColumnDef, RowData, CellData } from 'src/interfaces/table-data';
import { ColumnDataType } from 'src/interfaces/column-data-type';

@Component({
  selector: 'app-table',
  imports: [FormsModule, CommonModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss'
})

export class TableComponent implements OnInit, OnChanges {

  @Input() tableData: TableData;
  @Input() slideInContent!: TemplateRef<any>;
  @Input() pageSize = 100;
  @Input() methodList: { [key: string]: (row: any) => void } = {};
  @Input() showInvalidCount = false;
  @Input() transferLabel: string = '';
  @Input() enableRowTransfer = false;
  @Input() enableRowDelete = false;

  @Output() rowTransferred = new EventEmitter<any>();
  @Output() rowsTransferred = new EventEmitter<any[]>();
  @Output() rowDeleted = new EventEmitter<any>();
  @Output() rowsDeleted = new EventEmitter<any[]>();

  searchText: string = '';
  page: number = 1;
  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  pageSizes: number[] = [20, 50, 100, 250, 500];
  hideTable: boolean = false;

  viewSlideIn: boolean = false;
  slideInTable: TableData;
  slideInColumn: ColumnDef | undefined;
  slideInDisplay: ColumnDef | undefined;

  ColumnDataType = ColumnDataType;

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['tableData']) {

    }
    if (changes['slideInTableData']) {

    }
  }
  toggleTable() {
    this.hideTable = !this.hideTable;
  }
  getInvalidRowCount(): number {
    return this.tableData?.rows?.filter((r: any) => r.isInvalid)?.length || 0;
  }
  get filteredRows(): RowData[] {
    let filtered = this.tableData.rows.filter(row =>
      this.tableData.columns.some(col =>
        !col.isCommand &&
        row.cells[col.fieldName]?.textValue
          .toLowerCase()
          .includes(this.searchText.toLowerCase())
      )
    );

    if (this.sortColumn) {
      filtered.sort((a, b) => {
        const aVal = a.cells[this.sortColumn]?.rawValue;
        const bVal = b.cells[this.sortColumn]?.rawValue;

        if (aVal == null) return 1;
        if (bVal == null) return -1;

        if (aVal < bVal) return this.sortDirection === 'asc' ? -1 : 1;
        if (aVal > bVal) return this.sortDirection === 'asc' ? 1 : -1;
        return 0;
      });
    }

    return filtered;
  }

  get pagedRows(): RowData[] {
    const start = (this.page - 1) * this.pageSize;
    return this.filteredRows.slice(start, start + this.pageSize);
  }


  sort(col: ColumnDef | undefined) {
    if (this.sortColumn === col?.fieldName) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = col?.fieldName ?? '';
      this.sortDirection = 'asc';
    }
  }

  totalPages(): number {
    return Math.ceil(this.filteredRows.length / this.pageSize);
  }

  goToPage(p: number) {
    if (p >= 1 && p <= this.totalPages()) this.page = p;
  }

  highlight(text: string): string {
    if (!this.searchText) return text;
    const regex = new RegExp(`(${this.escapeRegex(this.searchText)})`, 'gi');
    return text.replace(regex, `<mark>$1</mark>`);
  }

  highlightValue(row: RowData, col: ColumnDef): string {
    if (!this.searchText) return row.cells[col.fieldName].textValue;
    const regex = new RegExp(`(${this.escapeRegex(this.searchText)})`, 'gi');
    return row.cells[col.fieldName].textValue.replace(regex, `<mark>$1</mark>`);
  }

  private escapeRegex(text: string): string {
    return text.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  }

  startItem(): number {
    return ((this.page - 1) * this.pageSize) + 1;
  }

  endItem(): number {
    const end = this.page * this.pageSize;
    return end > this.filteredRows.length ? this.filteredRows.length : end;
  }

  gotoSlideIn(col: ColumnDef | undefined, row: any): void {

    if (!col) {
      console.warn("gotoSlideIn called without a valid column");
      return;
    }

    this.slideInDisplay = this.getFirstSlideInColumn();
    this.viewSlideIn = true;
    this.slideInColumn = { ...col };
    this.slideInColumn.isHidden = false; //ensure column will show
    this.callSlideInCommand(row)
  }

  closeSlideIn(): void {
    this.viewSlideIn = false;
  }

  callSlideInCommand(row: any): void {
    this.methodList[this.slideInColumn?.slideInCommand!]?.(row);
  }

  getFirstSlideInColumn(): ColumnDef | undefined {
    return this.tableData?.columns.find(col => col.isSlideInColumn);
  }

  onDeleteRow(row: any) {
    const index = this.tableData?.rows?.indexOf(row);
    if (index >= 0) {
      this.tableData.rows.splice(index, 1);
      //this.applySearch(); // refresh filteredData & pagedData
      this.rowDeleted.emit(row); // notify parent
    }
  }

  onDeleteAll() {
    // rows that match filter
    const rowsToDelete = this.filteredRows;

    const remainingRows = this.tableData.rows.filter(
      row => !rowsToDelete.includes(row)
    );

    this.tableData = { ...this.tableData, rows: remainingRows };

    this.rowsDeleted.emit(rowsToDelete);
  }

  onTransferRow(row: any) {
    const index = this.tableData?.rows?.indexOf(row);
    if (index >= 0) {
      this.tableData.rows.splice(index, 1);
      //this.applySearch(); // refresh filteredData & pagedData
      this.rowTransferred.emit(row); // notify parent
    }
  }

  onTransferAll() {
    // rows that match filter
    const rowsToTransfer = this.filteredRows;

    // emit only the filtered rows
    this.rowsTransferred.emit(rowsToTransfer);

    // keep all rows that are NOT in filteredRows
    const remainingRows = this.tableData.rows.filter(
      row => !rowsToTransfer.includes(row)
    );

    this.tableData = { ...this.tableData, rows: remainingRows };
  }

}
