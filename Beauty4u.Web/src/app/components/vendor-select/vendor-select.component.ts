import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { map, startWith } from 'rxjs/operators';
import { VendorDto } from 'src/interfaces/vendor';
import { VendorService } from 'src/app/services/vendor.service';

@Component({
  selector: 'app-vendor-select',
  imports: [CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule],
  templateUrl: './vendor-select.component.html',
  styleUrl: './vendor-select.component.scss'
})
export class VendorSelectComponent implements OnInit {
  @Input({ required: true }) formGroup!: FormGroup;

  vendors: VendorDto[] = [];
  filteredVendors: VendorDto[] = [];

  constructor(private vendorService: VendorService) { }

  ngOnInit(): void {
    this.vendorService.getVendors().subscribe({
      next: data => {
        this.vendors = data;

        this.formGroup.get('vendor')?.valueChanges
          .pipe(
            startWith(''),
            map(value => this._filterVendors(typeof value === 'string' ? value : value?.name || ''))
          )
          .subscribe(filtered => (this.filteredVendors = filtered));
      },
      error: err => console.error('Vendor API error', err)
    });
  }

  displayVendor(vendor: VendorDto): string {
    return vendor ? `${vendor.name}` : '';
  }

  private _filterVendors(name: string): VendorDto[] {
    const filterValue = name.toLowerCase();
    return this.vendors.filter(
      v =>
        v.name.toLowerCase().includes(filterValue) ||
        v.code.toLowerCase().includes(filterValue)
    );
  }
}