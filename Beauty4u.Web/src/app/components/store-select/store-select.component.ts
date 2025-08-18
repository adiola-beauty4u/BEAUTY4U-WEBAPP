import { Component, Input, OnInit, forwardRef } from '@angular/core';
import { FormGroup, NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { map, startWith } from 'rxjs/operators';
import { SelectControlComponent } from '../select-control/select-control.component';
import { ItemValue } from 'src/interfaces/item-value';

import { StoreService } from 'src/app/services/store.service';
import { StoreDto } from 'src/interfaces/store';

@Component({
  selector: 'app-store-select',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    SelectControlComponent
  ],
  templateUrl: './store-select.component.html',
  styleUrl: './store-select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => StoreSelectComponent),
      multi: true
    }
  ]
})
export class StoreSelectComponent implements ControlValueAccessor {
  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;

  control = new FormControl();
  private onChange: any = () => { };
  onTouched: any = () => { };

  stores: StoreDto[] = [];
  storeValues: ItemValue[] = [];

  constructor(private storeService: StoreService) { }

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

  ngOnInit(): void {
    this.storeService.getStores().subscribe({
      next: data => {
        this.storeValues = data.map(item => ({
          displayText: item.name,
          value: item.code
        } as ItemValue));
      },
      error: err => console.error('Store API error', err)
    });
  }

}
