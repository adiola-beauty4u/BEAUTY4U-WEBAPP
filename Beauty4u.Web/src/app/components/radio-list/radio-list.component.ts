import { Component, Input, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatRadioModule } from '@angular/material/radio';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule } from '@angular/forms';

import { ItemValue } from 'src/interfaces/item-value';


@Component({
  selector: 'app-radio-list',
  imports: [
    CommonModule, MatRadioModule, ReactiveFormsModule
  ],
  templateUrl: './radio-list.component.html',
  styleUrl: './radio-list.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RadioListComponent),
      multi: true
    }
  ]
})

export class RadioListComponent implements ControlValueAccessor {
  @Input() label!: string;
  @Input() items: ItemValue[] = [];
  @Input() orientation: 'vertical' | 'horizontal' = 'horizontal';
  @Input() disabled = false;
  @Input() addAll = true;

  @Input() defaultValue: ItemValue | null = null;

  @Input() inline = false;

  value: ItemValue | null = null;

  onChange: any = () => { };
  onTouched: any = () => { };

  ngOnInit(): void {
    // If form does not set an initial value, use the default
    if (!this.value && this.defaultValue) {
      this.value = this.defaultValue;
      this.onChange(this.defaultValue);
    }
  }

  writeValue(value: ItemValue | null): void {
    if (value !== null) {
      this.value = value;
    } else if (this.defaultValue) {
      // Use default if nothing is passed
      this.value = this.defaultValue;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  selectValue(val: any) {
    const selected = this.items.find(i => i.value === val) || null;
    this.value = selected;
    this.onChange(this.value);
    this.onTouched();
  }
}