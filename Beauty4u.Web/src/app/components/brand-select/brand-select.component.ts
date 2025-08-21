import { Component, Input, OnInit, forwardRef } from '@angular/core';
import { FormGroup, NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { map, startWith } from 'rxjs/operators';
import { BrandService } from 'src/app/services/brand.service';
import { SelectControlComponent } from '../select-control/select-control.component';
import { FormControl } from '@angular/forms';
import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-brand-select',
  imports: [CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    SelectControlComponent],
  templateUrl: './brand-select.component.html',
  styleUrl: './brand-select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => BrandSelectComponent),
      multi: true
    }
  ]
})
export class BrandSelectComponent implements ControlValueAccessor {
  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;

  control = new FormControl();

  brands: string[] = [];
  brandValues: ItemValue[] = [];

  constructor(private brandService: BrandService) { }

  private onChange: any = () => { };
  onTouched: any = () => { };

  ngOnInit(): void {
    this.brandService.getBrands().subscribe({
      next: data => {
        this.brandValues = data.map(brand => ({
          displayText: brand,
          value: brand
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