import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { MaterialModule } from 'src/app/material.module';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { FileuploadComponent } from 'src/app/pages/ui-components/fileupload/fileupload.component';
import { VendorDto } from 'src/interfaces/vendor';
import { VendorService } from 'src/app/services/vendor.service';
import { ReactiveFormsModule, FormControl, FormGroup, FormBuilder, Validators, FormsModule } from '@angular/forms';
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
import { ExpandableTableComponent } from 'src/app/components/expandable-table/expandable-table.component';
import { MatCheckbox, MatCheckboxChange } from '@angular/material/checkbox';
import { AuthService } from 'src/app/services/authentication/auth.service';
import { Router } from '@angular/router';

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
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
  standalone: true,
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
    ExpandableTableComponent,
    MatCheckbox,
    FormsModule
  ],
})
export class RegistrationComponent implements OnInit {
  vendorForm: FormGroup;
  vendors: VendorDto[] = [];
  filteredVendors: VendorDto[] = [];
  isLoading: boolean = false;
  dialog = inject(MatDialog);
  invalidRowCount = 0;
  bulkRegisterTemplatePath = 'assets/templates/Bulk_Product_Register_Template.csv'
  bulkRegisterTemplateName = 'BulkProductRegister.csv'

  isSaved: boolean = false;
  showInvalidOnly: boolean = false;

  uploadOutput: any;

  tableGroupValues: any[] = [];

  dataSource = new MatTableDataSource<any>();
  displayedColumns: string[] = [];

  dataSourceOutput = new MatTableDataSource<any>();
  displayedColumnsOutput: string[] = [];

  isValidFile: boolean = false;
  previewResult = '';

  tableData: any;
  tableOutputData: any;

  @ViewChild('fileUploadRef') fileUpload!: any;
  @ViewChild('stepper') private stepper!: MatStepper;

  constructor(private vendorService: VendorService,
    private dataCheckService: DataCheckService,
    private productService: ProductService,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private authService: AuthService,
    private router: Router) {
    this.vendorForm = this.fb.group({
      vendor: ['', Validators.required],
    });
  }
  ngOnInit(): void {

    this.vendorService.getVendors().subscribe({
      next: data => {
        this.vendors = data;
      },
      error: err => console.error('Vendor API error', err)
    });

    this.vendorControl.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filterVendors(typeof value === 'string' ? value : value?.name || ''))
      )
      .subscribe(filtered => this.filteredVendors = filtered);
  }

  private _filterVendors(name: string): VendorDto[] {
    const filterValue = name.toLowerCase();
    return this.vendors.filter(vendor =>
      vendor.name.toLowerCase().includes(filterValue) ||
      vendor.code.toLowerCase().includes(filterValue)
    );
  }

  get vendorControl() {
    return this.vendorForm.get('vendor')!;
  }

  displayVendor(vendor: any): string {
    return vendor ? `${vendor.name}` : '';
  }

  fileSelected(): boolean {
    var self = this;
    const file = this.fileUpload.getSelectedFile();
    return file;
  }

  previewFile() {
    var self = this;
    this.showInvalidOnly = false;
    const file = this.fileUpload.getSelectedFile();
    this.isValidFile = false;
    if (!file) {
      alert('No file selected!');
      return;
    }

    if (this.vendorControl?.value?.code == undefined) {
      alert('No vendor selected!');
      return;
    }

    if (!this.authService.getUserCode()){
        this.router.navigate(['/authentication/login']);
    }

    const product: BulkProduct = {
      vendorId: this.vendorControl?.value?.vendorId || 0,
      vendorCode: this.vendorControl?.value?.code || "",
      vendorName: this.vendorControl?.value?.name || "",
      userCode: this.authService.getUserCode()!,
      productFile: file,
    };
    this.loadingService.show("Validating rows...");
    this.productService.bulkProductRegisterPreview(product).subscribe({
      next: (output) => {
        this.uploadOutput = output;

        this.tableGroupValues = output.tableData.tableGroups;
        this.isValidFile = output.isValid;
        this.previewResult = output.previewResult;
        this.tableData = output.tableData;
        this.displayedColumns = output.tableData.columns.map((col: any) => col.fieldName);
        this.invalidRowCount = this.tableGroupValues.reduce((sum, group) =>
          sum + group.rows.filter((r: any) => !r.isValid).length, 0);
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
    this.vendorForm.reset();
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
        title: 'Confirm Bulk Registration',
        message: 'Do you want to proceed with registering all products?',
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
    
    if (!this.authService.getUserCode()){
      this.router.navigate(['/authentication/login']);
    }

    this.loadingService.show("Saving products...");
    const file = this.fileUpload.getSelectedFile();

    const product: BulkProduct = {
      vendorId: this.vendorControl?.value?.vendorId || 0,
      vendorCode: this.vendorControl?.value?.code || "",
      vendorName: this.vendorControl?.value?.name || "",
      userCode: this.authService.getUserCode()!,
      productFile: file,
    };
    this.productService.uploadBulkProductRegistration(product).subscribe({
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

  onInvalidFilterToggle(event: MatCheckboxChange) {
    this.tableGroupValues = structuredClone(this.uploadOutput.tableData.tableGroups);
    if (this.showInvalidOnly) {
      this.tableGroupValues.forEach(group => {
        group.rows = group.rows.filter((row: Row) => row.isValid === false);
      });
    }
  }
}
