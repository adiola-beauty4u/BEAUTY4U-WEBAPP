import { Component, Input, OnInit, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ControlValueAccessor,
  FormControl,
  FormGroup,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule
} from '@angular/forms';
import { CheckListComponent } from '../check-list/check-list.component';
import { ItemValue } from 'src/interfaces/item-value';
import { StoreService } from 'src/app/services/store.service';
import { StoreDto } from 'src/interfaces/store';

@Component({
  selector: 'app-store-check-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CheckListComponent
  ],
  templateUrl: './store-check-list.component.html',
  styleUrls: ['./store-check-list.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => StoreCheckListComponent),
      multi: true
    }
  ]
})
export class StoreCheckListComponent implements ControlValueAccessor, OnInit {
  @Input({ required: true }) formGroup!: FormGroup;
  @Input({ required: true }) formControlName!: string;
  @Input() defaultValue: any[] | null = null;
  @Input() selectAllIfNull: boolean = true;

  control = new FormControl<ItemValue[] | null>([]);
  private onChange: any = () => { };
  private onTouched: any = () => { };

  stores: StoreDto[] = [];
  storeValues: ItemValue[] = [];

  constructor(private storeService: StoreService) { }

  // CVA methods
  writeValue(values: ItemValue[] | null): void {
    this.control.setValue(values ?? [], { emitEvent: false });
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
