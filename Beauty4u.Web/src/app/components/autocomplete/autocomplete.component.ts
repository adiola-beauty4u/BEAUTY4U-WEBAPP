import { Component, Input, forwardRef, OnChanges, SimpleChanges, ElementRef, ViewChild, ViewContainerRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule, FormControl } from '@angular/forms';
import { TemplatePortal } from '@angular/cdk/portal';

import {
  Overlay,
  OverlayModule,
  OverlayRef,
  FlexibleConnectedPositionStrategy,
} from '@angular/cdk/overlay';

import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-autocomplete',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, OverlayModule],
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AutocompleteComponent),
      multi: true
    }
  ]
})
export class AutocompleteComponent implements ControlValueAccessor, OnChanges {
  @Input() label: string;
  @Input() placeholder: string = 'Search...';
  @Input() items: ItemValue[] = [];
  @Input() addAll: boolean = false; // 👈 optional “All” flag

  overlayRef!: OverlayRef;

  activeIndex: number = -1;

  control = new FormControl<ItemValue | null>(null);
  filteredItems: ItemValue[] = [];
  dropdownOpen = false;

  @ViewChild('inputEl') inputEl!: ElementRef<HTMLInputElement>;
  @ViewChild('dropdownTemplate') dropdownTemplate!: any;

  constructor(private overlay: Overlay, private vcr: ViewContainerRef) { }

  private onChange = (_: any) => { };
  private onTouched = () => { };

  ngOnInit() {
    this.filteredItems = this.items;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['items']) {
      this.filteredItems = this.items;

      // 🔹 Ensure input reflects current value or clears if none
      if (this.inputEl) {
        this.inputEl.nativeElement.value = this.control.value
          ? this.displayItem(this.control.value)
          : '';
      }
    }
  }

  filterItems(query: string) {
    const q = (query || '').toLowerCase();
    if (!q) {
      this.filteredItems = this.items;
      return;
    }
    this.filteredItems = this.items.filter(item =>
      item.displayText.toLowerCase().includes(q) ||
      ('' + item.value).toLowerCase().includes(q)
    );
  }


  onBlur() {
    setTimeout(() => (this.dropdownOpen = false), 200);
    this.onTouched();
  }

  // ControlValueAccessor
  // ControlValueAccessor
  writeValue(value: ItemValue | null): void {
    this.control.setValue(value);

    if (value === null) {
      // 🔹 Case 1: reset to blank
      this.filteredItems = this.items;
      if (this.inputEl) {
        this.inputEl.nativeElement.value = '';
      }
    } else {
      // 🔹 Case 2: set default value (e.g. "All")
      this.filteredItems = this.items;
      if (this.inputEl) {
        this.inputEl.nativeElement.value = this.displayItem(value);
      }
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  displayItem(item: ItemValue | null): string {
    return item?.value ? item.displayText : (this.addAll ? 'All' : '');
  }


  onInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredItems = this.items.filter((i) =>
      i.displayText.toLowerCase().includes(value)
    );
  }

  openDropdown(): void {
    if (this.overlayRef) {
      this.overlayRef.dispose();
    }

    const positionStrategy: FlexibleConnectedPositionStrategy = this.overlay
      .position()
      .flexibleConnectedTo(this.inputEl.nativeElement)
      .withPositions([
        {
          originX: 'start',
          originY: 'bottom',
          overlayX: 'start',
          overlayY: 'top',
        },
        {
          originX: 'start',
          originY: 'top',
          overlayX: 'start',
          overlayY: 'bottom',
        },
      ]);

    this.overlayRef = this.overlay.create({
      hasBackdrop: true,
      backdropClass: 'cdk-overlay-transparent-backdrop',
      positionStrategy,
      scrollStrategy: this.overlay.scrollStrategies.reposition(),
    });

    const portal = new TemplatePortal(this.dropdownTemplate, this.vcr);
    this.overlayRef.attach(portal);

    this.overlayRef.backdropClick().subscribe(() => this.closeDropdown());
  }

  closeDropdown(): void {
    if (this.overlayRef) {
      this.overlayRef.dispose();
      this.overlayRef = null!;
    }
  }

  selectItem(item: ItemValue | null): void {
    if (!item) {
      this.filteredItems = this.items;
    }
    this.control.setValue(item);
    this.inputEl.nativeElement.value = this.displayItem(item);
    this.onChange(item);
    this.closeDropdown();
  }


}