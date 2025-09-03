import {
  Component,
  Input,
  OnInit,
  OnChanges,
  SimpleChanges,
  TemplateRef,
  Output,
  EventEmitter
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';

import { TableData, ColumnDef, RowData } from 'src/interfaces/table-data';
import { ColumnDataType } from 'src/interfaces/column-data-type';

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [FormsModule, CommonModule, ScrollingModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss'
})
export class TableComponent implements OnInit, OnChanges {
  @Input() tableData: TableData = { columns: [], rows: [], tableGroups: [], tableName: '' };
  @Input() slideInContent!: TemplateRef<any>;
  @Input() pageSize = 100;
  @Input() methodList: { [key: string]: (rowData: RowData, row: any) => void } = {};
  @Input() showInvalidCount = false;
  @Input() transferLabel: string = '';
  @Input() enableRowTransfer = false;
  @Input() enableRowDelete = false;
  @Input() groupingEnabled = false;

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

  // Cached rows
  filteredRows: RowData[] = [];
  groupedRows: { key: string; rows: RowData[]; collapsed: boolean; totalCount: number }[] = [];

  viewSlideIn: boolean = false;
  slideInColumn: ColumnDef | undefined;
  slideInDisplay: ColumnDef | undefined;

  ColumnDataType = ColumnDataType;

  ngOnInit(): void {
    this.updateFilteredRows();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['tableData']) {
      this.updateFilteredRows();
    }
  }

  updateFilteredRows() {
    let filtered = this.tableData?.rows ?? [];

    if (this.searchText) {
      filtered = filtered.filter(row =>
        this.tableData.columns.some(col =>
          !col.isCommand &&
          row.cells[col.fieldName]?.textValue
            ?.toLowerCase()
            .includes(this.searchText.toLowerCase())
        )
      );
    }

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

    this.filteredRows = filtered;

    // Reset to first page whenever filter/sort changes
    this.page = 1;
    this.buildGroups();
  }

  /** Rows of the current page */
  get pagedRows(): RowData[] {
    const start = (this.page - 1) * this.pageSize;
    return this.filteredRows.slice(start, start + this.pageSize);
  }

  buildGroups() {
    if (!this.groupingEnabled) {
      this.groupedRows = [
        {
          key: 'All',
          rows: this.pagedRows,
          collapsed: false,
          totalCount: this.filteredRows.length
        }
      ];
      return;
    }

    // Build groups based on all filtered rows (for total count)
    const groups: { [key: string]: RowData[] } = {};
    this.filteredRows.forEach(row => {
      const key = row.groupValue || 'Ungrouped';
      if (!groups[key]) groups[key] = [];
      groups[key].push(row);
    });

    // For each group, slice rows according to current page (global paging)
    const start = (this.page - 1) * this.pageSize;
    const end = start + this.pageSize;
    const pagedSet = new Set(this.pagedRows);

    this.groupedRows = Object.keys(groups).map(key => {
      const allRows = groups[key];
      const pageRows = allRows.filter(r => pagedSet.has(r));

      return {
        key,
        rows: pageRows,
        collapsed: false,
        totalCount: allRows.length
      };
    });
  }

  // === Paging helpers ===
  totalPages(): number {
    return Math.ceil(this.filteredRows.length / this.pageSize);
  }

  goToPage(p: number) {
    if (p >= 1 && p <= this.totalPages()) {
      this.page = p;
      this.buildGroups();
    }
  }

  startItem(): number {
    return this.filteredRows.length === 0 ? 0 : ((this.page - 1) * this.pageSize) + 1;
  }

  endItem(): number {
    const end = this.page * this.pageSize;
    return end > this.filteredRows.length ? this.filteredRows.length : end;
  }

  // === Sorting ===
  sort(col: ColumnDef | undefined) {
    if (!col) return;
    if (this.sortColumn === col.fieldName) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = col.fieldName;
      this.sortDirection = 'asc';
    }
    this.updateFilteredRows();
  }

  // === Utilities ===
  toggleGroup(group: any) {
    group.collapsed = !group.collapsed;
  }

  toggleGrouping() {
    this.groupingEnabled = !this.groupingEnabled;
    this.buildGroups();
  }

  toggleTable() {
    this.hideTable = !this.hideTable;
  }

  highlightValue(row: RowData, col: ColumnDef): string {
    const value = row.cells[col.fieldName]?.textValue ?? '';
    if (!this.searchText) return value;
    const regex = new RegExp(`(${this.escapeRegex(this.searchText)})`, 'gi');
    return value.replace(regex, `<mark>$1</mark>`);
  }

  private escapeRegex(text: string): string {
    return text.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  }

  // === Slide-in panel ===
  gotoSlideIn(col: ColumnDef | undefined, rowData: RowData, row: any): void {
    if (!col) return;
    this.slideInDisplay = this.getFirstSlideInColumn();
    this.viewSlideIn = true;
    this.slideInColumn = { ...col, isHidden: false };
    this.callSlideInCommand(rowData, row);
  }

  closeSlideIn(): void {
    this.viewSlideIn = false;
  }

  callSlideInCommand(rowData: RowData, row: any): void {
    this.methodList[this.slideInColumn?.slideInCommand!]?.(rowData, row);
  }

  getFirstSlideInColumn(): ColumnDef | undefined {
    return this.tableData?.columns.find(col => col.isSlideInColumn);
  }

  getInvalidRowCount(): number {
    return this.tableData?.rows?.filter((r: any) => r.isInvalid)?.length || 0;
  }

  // === Row operations ===
  onDeleteRow(row: any) {
    const index = this.tableData?.rows?.indexOf(row);
    if (index >= 0) {
      this.tableData.rows.splice(index, 1);
      this.updateFilteredRows();
      this.rowDeleted.emit(row);
    }
  }

  onDeleteAll() {
    const rowsToDelete = this.filteredRows;
    const remainingRows = this.tableData.rows.filter(
      row => !rowsToDelete.includes(row)
    );
    this.tableData = { ...this.tableData, rows: remainingRows };
    this.updateFilteredRows();
    this.rowsDeleted.emit(rowsToDelete);
  }

  onTransferRow(row: any) {
    const index = this.tableData?.rows?.indexOf(row);
    if (index >= 0) {
      this.tableData.rows.splice(index, 1);
      this.updateFilteredRows();
      this.rowTransferred.emit(row);
    }
  }

  onTransferAll() {
    const rowsToTransfer = this.filteredRows;
    const remainingRows = this.tableData.rows.filter(
      row => !rowsToTransfer.includes(row)
    );
    this.tableData = { ...this.tableData, rows: remainingRows };
    this.updateFilteredRows();
    this.rowsTransferred.emit(rowsToTransfer);
  }

  trackByRow(index: number, row: RowData): any {
    return row.rowKey ?? index;
  }
}
