import { Component, Input, OnInit, forwardRef } from '@angular/core';
import { FormGroup, NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { map, startWith } from 'rxjs/operators';
import { VendorDto } from 'src/interfaces/vendor';
import { VendorService } from 'src/app/services/vendor.service';
import { SelectControlComponent } from '../select-control/select-control.component';
import { FormControl } from '@angular/forms';
import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-vendor-select',
  imports: [CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    SelectControlComponent],
  templateUrl: './vendor-select.component.html',
  styleUrl: './vendor-select.component.scss',
    providers: [
      {
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => VendorSelectComponent),
        multi: true
      }
    ]
})
export class VendorSelectComponent implements ControlValueAccessor {
  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;

  control = new FormControl();

  vendors: VendorDto[] = [];
  vendorValues: ItemValue[] = [];

  constructor(private vendorService: VendorService) { }

  private onChange: any = () => { };
  onTouched: any = () => { };

  ngOnInit(): void {
    this.vendorService.getVendors().subscribe({
      next: data => {
        this.vendorValues = data.map(vendor => ({
          displayText: vendor.name,
          value: vendor.code
        } as ItemValue));
      },
      error: err => console.error('Vendor API error', err)
    });
  }

  // CVA methods
  writeValue(value: ItemValue | null): void {
    this.control.setValue(value, { emitEvent: false });
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
    this.control.valueChanges.subscribe(fn);
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.control.disable() : this.control.enable();
  }
}