import { Component, Input, forwardRef, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { startWith, map } from 'rxjs/operators';
import { FormControl } from '@angular/forms';

import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-select-control',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule
  ],
  templateUrl: './select-control.component.html',
  styleUrls: ['./select-control.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectControlComponent),
      multi: true
    }
  ]
})
export class SelectControlComponent implements OnChanges, ControlValueAccessor {
  @Input() label!: string;
  @Input() placeHolder!: string;
  @Input() selectItems: ItemValue[] = [];
  @Input() addAll = true;

  control = new FormControl();
  filteredItems: ItemValue[] = [];

  private onChange: any = () => { };
  onTouched: any = () => { };

  ngOnInit() {
    this.control.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          this._filterItems(typeof value === 'string' ? value : value?.displayText || '')
        )
      )
      .subscribe(filtered => (this.filteredItems = filtered));
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.control.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          this._filterItems(typeof value === 'string' ? value : value?.displayText || '')
        )
      )
      .subscribe(filtered => (this.filteredItems = filtered));
  }

  displayItem = (item: ItemValue | null): string =>
    item ? item.displayText : '';

  private _filterItems(search: string): ItemValue[] {
    const filterValue = search.toLowerCase();
    return this.selectItems.filter(option =>
      option?.displayText.toLowerCase().includes(filterValue)
    );
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