// flatpickr.directive.ts
import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { NgControl } from '@angular/forms';
import flatpickr from 'flatpickr';
import { Instance } from 'flatpickr/dist/types/instance';

@Directive({
    selector: '[appFlatpickr]',
    standalone: true
})
export class FlatpickrDirective implements OnInit {
    @Input() appFlatpickrOptions: flatpickr.Options.Options = {};
    private picker!: Instance;

    constructor(private el: ElementRef, private control: NgControl) { }

    ngOnInit() {
        this.picker = flatpickr(this.el.nativeElement, {
            dateFormat: 'Y-m-d',
            ...this.appFlatpickrOptions,
            defaultDate: this.control.value,
            onChange: (dates) => {
                this.control.control?.setValue(dates[0], { emitEvent: true });
            }
        });

        // sync FormControl → Flatpickr
        this.control.valueChanges?.subscribe((val) => {
            if (val && this.picker) {
                this.picker.setDate(val, false); // update without re-trigger
            }
        });
    }
}
