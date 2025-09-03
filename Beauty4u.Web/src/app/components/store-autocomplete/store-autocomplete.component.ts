import { Component, Input, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule, FormControl } from '@angular/forms';
import { AutocompleteComponent } from '../autocomplete/autocomplete.component';

import { ItemValue } from 'src/interfaces/item-value';
import { StoreDto } from 'src/interfaces/store';
import { StoreService } from 'src/app/services/store.service';

@Component({
  selector: 'app-store-autocomplete',
  imports: [CommonModule, ReactiveFormsModule, AutocompleteComponent],
  templateUrl: './store-autocomplete.component.html',
  styleUrl: './store-autocomplete.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => StoreAutocompleteComponent),
      multi: true
    }
  ]
})
export class StoreAutocompleteComponent implements ControlValueAccessor {
  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;
  control = new FormControl();
  private onChange: any = () => { };
  onTouched: any = () => { };

  stores: StoreDto[] = [];
  storeValues: ItemValue[] = [];

  constructor(private storeService: StoreService) { }

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
