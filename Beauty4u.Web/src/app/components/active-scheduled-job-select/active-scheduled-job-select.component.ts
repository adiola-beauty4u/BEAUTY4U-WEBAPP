import { Component, Input, OnInit, forwardRef } from '@angular/core';
import { FormGroup, NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { map, startWith } from 'rxjs/operators';

import { SelectControlComponent } from '../select-control/select-control.component';
import { ItemValue } from 'src/interfaces/item-value';
import { ScheduledJobService } from 'src/app/services/scheduled-job.service';
import { ScheduledJob } from 'src/interfaces/scheduled-job';

@Component({
  selector: 'app-active-scheduled-job-select',
  imports: [CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    SelectControlComponent],
  templateUrl: './active-scheduled-job-select.component.html',
  styleUrl: './active-scheduled-job-select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ActiveScheduledJobSelectComponent),
      multi: true
    }
  ]
})

export class ActiveScheduledJobSelectComponent implements ControlValueAccessor {

  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;
  @Input() defaultValue: any;

  activeJobs: ItemValue[] = [];

  control = new FormControl();

  constructor(private scheduledJobService: ScheduledJobService) { }

  private onChange: any = () => { };
  onTouched: any = () => { };

  ngOnInit(): void {
    this.scheduledJobService.getActiveJobs().subscribe({
      next: data => {
        this.activeJobs = data.map(item => ({
          displayText: item.name,
          value: item.scheduledJobId.toString()
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
