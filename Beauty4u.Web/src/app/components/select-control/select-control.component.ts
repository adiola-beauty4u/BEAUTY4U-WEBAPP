import { Component, Input, forwardRef, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { startWith, map } from 'rxjs/operators';
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
  @Input() defaultValue: any;
  @Input() includeCodeInDisplay = true;

  control = new FormControl(); // UI control
  filteredItems: ItemValue[] = [];

  private onChange: (value: ItemValue | null) => void = () => { };
  private onTouched: () => void = () => { };

  ngOnInit() {
    this.control.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filterItems(typeof value === 'string' ? value : value?.displayText || ''))
      )
      .subscribe(filtered => (this.filteredItems = filtered));

    this.applyDefault();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectItems']) {
      this.filteredItems = this.selectItems.slice();
      this.applyDefault();
    }
    if (changes['defaultValue']) {
      this.applyDefault();
    }
  }

  private applyDefault(): void {
    if (this.defaultValue) {
      const selected = this.selectItems.find(i => i.value === this.defaultValue);
      if (selected) {
        setTimeout(() => this.writeValue(selected)); // defer to next tick
      }
    }
  }

  displayItem = (item: ItemValue | null): string =>
    item ? item.displayText : '';

  private _filterItems(search: string): ItemValue[] {
    const filterValue = search.toLowerCase();
    return this.selectItems.filter(option =>
      option?.displayText.toLowerCase().includes(filterValue)
    );
  }

  // --------------------
  // CVA implementation
  // --------------------
  writeValue(value: ItemValue | null): void {
    // set both UI + propagate to parent
    this.control.setValue(value, { emitEvent: false });
    if (value) {
      this.onChange(value);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
    this.control.valueChanges.subscribe(val => {
      if (typeof val === 'object' && val !== null) {
        this.onChange(val); // whole object
      } else {
        this.onChange(null);
      }
    });
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.control.disable() : this.control.enable();
  }
}
