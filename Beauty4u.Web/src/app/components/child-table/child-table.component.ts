import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { RowData } from 'src/interfaces/table-data';
import { MatCardModule } from '@angular/material/card';


@Component({
  selector: 'app-child-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatIconModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatCardModule
  ],
  templateUrl: './child-table.component.html',
})
export class ChildTableComponent implements OnInit, OnChanges {
  @Input() group: any;
  @Input() displayedColumns: string[] = [];
  @Input() pageSize = 5;
  @Input() pageIndex = 0;
  @Input() showInvalidCount = false;
  @Input() showSearchBox = false;
  @Input() methodList: { [key: string]: (row: any) => void } = {};
  @Output() page = new EventEmitter<PageEvent>();


  filteredData: any[] = [];
  pagedData: any[] = [];
  searchText = '';
  

  ngOnInit() {
    this.applySearch();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['group']) {
      this.applySearch();
    }
  }

  applySearch() {
    const allData = this.group?.rows || [];
    const lowerSearch = this.searchText.toLowerCase();

    this.filteredData = allData.filter((row: RowData) =>
      Object.values(row.cells).some((cell: any) =>
        (cell.textValue || '').toString().toLowerCase().includes(lowerSearch)
      )
    );
    this.pageIndex = 0;
    this.updatePagedData();
  }

  updatePagedData() {
    const start = this.pageIndex * this.pageSize;
    const end = start + this.pageSize;
    this.pagedData = this.filteredData.slice(start, end);
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePagedData();
    this.page.emit(event);
  }

  refresh() {
    this.applySearch();
  }

  getTotalRowCount(): number {
    return this.group?.rows?.length || 0;
  }

  getInvalidRowCount(): number {
    return this.group?.rows?.filter((r: any) => r.isInvalid)?.length || 0;
  }

  clear(): void {
    this.searchText = '';
    if (this.group) {
      this.group.rows = [];
    }
    this.filteredData = [];
    this.pagedData = [];
    this.pageIndex = 0;
    this.updatePagedData();
  }
}
