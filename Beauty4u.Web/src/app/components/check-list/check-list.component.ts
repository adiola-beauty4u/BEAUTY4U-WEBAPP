import { Component, Input, forwardRef, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-check-list',
  standalone: true,
  imports: [CommonModule, MatCheckboxModule],
  templateUrl: './check-list.component.html',
  styleUrls: ['./check-list.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckListComponent),
      multi: true,
    },
  ],
})
export class CheckListComponent implements ControlValueAccessor, OnChanges {
  @Input() label: string = '';
  @Input() items: ItemValue[] = [];
  @Input() inline: boolean = false;
  @Input() orientation: 'vertical' | 'horizontal' = 'vertical';
  @Input() disabled: boolean = false;
  @Input() addAll: boolean = false;
  @Input() defaultValue: any[] | null = null; // ✅ optional default
  @Input() selectAllIfNull: boolean = true;

  selectedValues: any[] = [];
  allSelected = false;

  private onChange = (_: any) => { };
  private onTouched = () => { };

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['items'] && this.items?.length > 0 && this.selectedValues.length === 0) {
      if (this.defaultValue) {
        this.selectedValues = [...this.defaultValue];
      } else if (this.selectAllIfNull) {
        // ✅ select all if flag is true
        this.selectedValues = this.items.map(i => i.value);
      } else {
        // ✅ otherwise select none
        this.selectedValues = [];
      }
      this.allSelected = this.selectedValues.length === this.items.length;
      this.propagateChange();
    }
  }

  writeValue(values: any[]): void {
    if (values && values.length > 0) {
      this.selectedValues = [...values];
    } else if (this.defaultValue) {
      this.selectedValues = [...this.defaultValue];
    } else if (this.selectAllIfNull) {
      this.selectedValues = this.items.map(i => i.value);
    } else {
      this.selectedValues = [];
    }

    this.allSelected = this.selectedValues.length === this.items.length;
  }


  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  isSelected(value: any): boolean {
    return this.selectedValues.includes(value);
  }

  toggleItem(value: any, checked: boolean) {
    if (checked) {
      this.selectedValues = [...this.selectedValues, value];
    } else {
      this.selectedValues = this.selectedValues.filter(v => v !== value);
    }
    this.allSelected = this.selectedValues.length === this.items.length;
    this.propagateChange();
  }

  toggleAll(checked: boolean) {
    if (checked) {
      this.selectedValues = this.items.map(i => i.value);
    } else {
      this.selectedValues = [];
    }
    this.allSelected = checked;
    this.propagateChange();
  }

  private propagateChange() {
    this.onChange(this.selectedValues);
    this.onTouched();
  }
}
