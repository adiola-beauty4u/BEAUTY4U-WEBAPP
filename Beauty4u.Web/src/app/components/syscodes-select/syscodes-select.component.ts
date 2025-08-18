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

import { SystemService } from 'src/app/services/system.service';
import { SysCode } from 'src/interfaces/sys-codes';

@Component({
  selector: 'app-syscodes-select',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    SelectControlComponent
  ],
  templateUrl: './syscodes-select.component.html',
  styleUrl: './syscodes-select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SysCodesSelectComponent),
      multi: true
    }
  ]
})

export class SysCodesSelectComponent implements ControlValueAccessor {
  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;
  @Input({ required: true }) sysCodeClass!: string;
  @Input({ required: true }) sysCodeLabel!: string;
  @Input({ required: true }) sysCodePlaceHolder!: string;

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
