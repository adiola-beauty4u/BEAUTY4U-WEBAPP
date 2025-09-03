import { Component, Input, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule, FormControl } from '@angular/forms';
import { AutocompleteComponent } from '../autocomplete/autocomplete.component';

import { SysCode } from 'src/interfaces/sys-codes';
import { SystemService } from 'src/app/services/system.service';
import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-syscodes-autocomplete',
  imports: [CommonModule, ReactiveFormsModule, AutocompleteComponent],
  templateUrl: './syscodes-autocomplete.component.html',
  styleUrl: './syscodes-autocomplete.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SyscodesAutocompleteComponent),
      multi: true
    }
  ]
})
export class SyscodesAutocompleteComponent implements ControlValueAccessor {
  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;

  @Input({ required: true }) sysCodeClass!: string;
  @Input({ required: true }) sysCodeLabel!: string;
  @Input({ required: true }) sysCodePlaceHolder!: string;
  @Input() addAll = true;

  control = new FormControl();
  private onChange: any = () => { };
  onTouched: any = () => { };

  sysCodes: SysCode[] = [];
  sysCodeValues: ItemValue[] = [];

  constructor(private systemService: SystemService) { }

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
    this.systemService.getSysCodeByClass(this.sysCodeClass).subscribe({
      next: data => {
        this.sysCodeValues = data.map(item => ({
          displayText: item.name,
          value: item.code
        } as ItemValue));
      },
      error: err => console.error('System API error', err)
    });
  }

}
