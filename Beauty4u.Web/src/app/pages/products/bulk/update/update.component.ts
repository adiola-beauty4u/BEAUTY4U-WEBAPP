import { Component, OnInit, ViewChild, inject, Input, WritableSignal, signal, computed } from '@angular/core';
import { MaterialModule } from 'src/app/material.module';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { FileuploadComponent } from 'src/app/pages/ui-components/fileupload/fileupload.component';
import { VendorDto } from 'src/interfaces/vendor';
import { VendorService } from 'src/app/services/vendor.service';
import { ReactiveFormsModule, FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { map, startWith } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { PreviewDialogComponent } from 'src/app/pages/ui-components/preview-dialog/preview-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { DataCheckService } from 'src/app/services/datacheck.service';
import { ProductService } from 'src/app/services/product.service';
import { BulkProduct } from 'src/interfaces/bulk-product';
import { MatTableDataSource } from '@angular/material/table';
import { MatStepper } from '@angular/material/stepper';
import { ConfirmDialogComponent } from 'src/app/pages/ui-components/confirm-dialog/confirm-dialog.component';
import { LoadingService } from 'src/app/services/loading.service';
import { MatIcon } from '@angular/material/icon';

import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckbox, MatCheckboxChange } from '@angular/material/checkbox';

import { ExpandableTableComponent } from 'src/app/components/expandable-table/expandable-table.component';

interface Cell {
  rawValue: any;
  textValue: string;
  isValid: boolean;
}

interface Column {
  fieldName: string;
  header: string;
}

interface Row {
  cells: Cell[];
  isValid: boolean;
}

@Component({
  selector: 'app-update',
  imports: [
    CommonModule,
    MaterialModule,
    MatButtonModule,
    MatSelectModule,
    FileuploadComponent,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    MatStepper,
    MatIcon,
    FormsModule,
    MatTableModule,
    MatIconModule,
    MatCardModule,
    MatTooltipModule,
    ExpandableTableComponent,
    MatCheckbox
  ],
  templateUrl: './update.component.html',
  styleUrl: './update.component.scss'
})
export class UpdateComponent implements OnInit {
  dialog = inject(MatDialog);
  invalidRowCount = 0;
  bulkUpdateTemplatePath = 'assets/templates/Bulk_Product_Register_Template.csv'
  bulkUpdateTemplateName = 'BulkProductRegister.csv'

  isSaved: boolean = false;

  showInvalidOnly: boolean = false;

  uploadOutput: any;

  tableGroupValues: any[] = [];

  @ViewChild('fileUploadRef') fileUpload!: any;
  @ViewChild('stepper') private stepper!: MatStepper;

  constructor(private vendorService: VendorService,
    private dataCheckService: DataCheckService,
    private productService: ProductService,
    private fb: FormBuilder,
    private loadingService: LoadingService) {

  }
  ngOnInit(): void {
  }

  fileSelected(): boolean {
    var self = this;
    const file = this.fileUpload?.getSelectedFile();
    return file;
  }

  previewFile() {
    var self = this;
    const file = this.fileUpload.getSelectedFile();
    if (!file) {
      alert('No file selected!');
      return;
    }
    this.showInvalidOnly = false;

    this.loadingService.show("Validating rows...");
    this.productService.bulkProductUpdatePreview(file).subscribe({
      next: (output) => {
        this.uploadOutput = output;
        this.tableGroupValues = output.tableData.tableGroups;
        this.stepper.next();
        this.loadingService.hide();
      },
      error: (errr) => {
        console.error('Upload error', errr);
        this.loadingService.hide();
      },
    });
  }

  formatText(value: string | null | undefined): string {
    if (!value) return '';
    return value.replace(/\n/g, '<br>');
  }
  getCellValue(row: any, column: string): string {
    return row[column] ?? '';
  }

  clearForm() {
    this.fileUpload?.clear();
  }

  handleUploaded(response: any) {
    console.log('Parent received upload response:', response);
  }

  confirmUpload() {
    this.stepper.next();
  }

  confirmBulkRegister() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Confirm Bulk Update',
        message: 'Do you want to proceed with the bulk update?',
        icon: 'warning' // optional: pass 'info', 'error', etc.
      },
      width: '400px',
      panelClass: 'confirm-dialog-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.sendBulkRequest();
      }
    });
  }

  sendBulkRequest() {
    this.loadingService.show("Saving products...")
    const file = this.fileUpload.getSelectedFile();

    this.productService.uploadBulkProductUpdate(file).subscribe({
      next: (output) => {
        this.tableGroupValues = output.tableGroups;
        this.stepper.next();
        this.loadingService.hide();
      },
      error: (errr) => {
        console.error('Upload error', errr);
        this.loadingService.hide();
      },
    });
  }

  reset() {
    this.clearForm();
    this.stepper.reset();
  }

  

  getTotalRowCount(groupIndex?: number): number {
    if (groupIndex != null) {
      return this.tableGroupValues[groupIndex].rows.length;
    }
    return this.tableGroupValues.reduce((sum, group) => sum + group.rows.length, 0);
  }

  getInvalidRowCount(groupIndex?: number): number {
    if (groupIndex != null) {
      return this.tableGroupValues[groupIndex].rows.filter((r: any) => !r.isValid).length;
    }
    return this.tableGroupValues.reduce((sum, group) =>
      sum + group.rows.filter((r: any) => !r.isValid).length, 0);
  }

  onInvalidFilterToggle(event: MatCheckboxChange) {
      this.tableGroupValues = structuredClone(this.uploadOutput.tableData.tableGroups);
      if (this.showInvalidOnly) {
        this.tableGroupValues.forEach(group => {
          group.rows = group.rows.filter((row: Row) => row.isValid === false);
        });
      }
    }
}
