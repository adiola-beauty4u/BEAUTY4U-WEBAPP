import {
  Component,
  Input,
  forwardRef
} from '@angular/core';
import {
  ControlValueAccessor,
  FormBuilder,
  FormGroup,
  NG_VALUE_ACCESSOR,
  Validators
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';

export interface TransferScheduleValue {
  mode: 'now' | 'schedule';
  dateTime?: Date;
}

@Component({
  selector: 'app-transfer-schedule',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatRadioModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule
  ],
  templateUrl: './transfer-schedule.component.html',
  styleUrls: ['./transfer-schedule.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TransferScheduleComponent),
      multi: true
    }
  ]
})
export class TransferScheduleComponent implements ControlValueAccessor {
  @Input() label?: string;
  @Input() inline = false;
  @Input() orientation: 'vertical' | 'horizontal' = 'vertical';

  form: FormGroup;
  minDate = new Date();

  private onChange: (value: TransferScheduleValue | null) => void = () => { };
  private onTouched: () => void = () => { };

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      mode: ['now'],
      scheduleDate: [{ value: null, disabled: true }, Validators.required],
      scheduleTime: [{ value: '', disabled: true }, Validators.required]
    });

    this.form.valueChanges.subscribe(() => {
      this.emitValue();
      this.onTouched();
    });
  }

  // ControlValueAccessor methods
  writeValue(value: TransferScheduleValue | null): void {
    if (!value) {
      this.form.reset({ mode: 'now' });
      return;
    }

    this.form.get('mode')!.setValue(value.mode, { emitEvent: false });

    if (value.mode === 'schedule' && value.dateTime) {
      this.form.get('scheduleDate')!.enable({ emitEvent: false });
      this.form.get('scheduleTime')!.enable({ emitEvent: false });
      this.form.patchValue(
        {
          scheduleDate: value.dateTime,
          scheduleTime: this.formatTime(value.dateTime)
        },
        { emitEvent: false }
      );
    } else {
      this.form.get('scheduleDate')!.disable({ emitEvent: false });
      this.form.get('scheduleTime')!.disable({ emitEvent: false });
    }
  }

  registerOnChange(fn: (value: TransferScheduleValue | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    if (isDisabled) {
      this.form.disable({ emitEvent: false });
    } else {
      this.form.enable({ emitEvent: false });
    }
  }

  // helpers
  private emitValue() {
    const mode = this.form.get('mode')!.value as 'now' | 'schedule';

    if (mode === 'schedule') {
      const date = this.form.get('scheduleDate')!.value as Date | null;
      const time = this.form.get('scheduleTime')!.value as string;
      if (date && time) {
        const [hours, minutes] = time.split(':').map(Number);
        const dateTime = new Date(date);
        dateTime.setHours(hours, minutes, 0, 0);
        this.onChange({ mode, dateTime });
        return;
      }
    }
    this.onChange({ mode });
  }

  private formatTime(date: Date): string {
    const h = date.getHours().toString().padStart(2, '0');
    const m = date.getMinutes().toString().padStart(2, '0');
    return `${h}:${m}`;
  }
}
