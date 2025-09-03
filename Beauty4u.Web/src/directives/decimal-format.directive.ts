import { Directive, HostListener, ElementRef } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
    selector: '[decimalFormat]',
    standalone: true
})
export class DecimalFormatDirective {
    constructor(
        private el: ElementRef<HTMLInputElement>,
        private control: NgControl
    ) { }

    @HostListener('blur')
    onBlur() {
        const rawValue = this.el.nativeElement.value;

        if (rawValue !== null && rawValue !== '') {
            const value = parseFloat(rawValue);

            if (!isNaN(value)) {
                const formatted = value.toFixed(2);

                this.el.nativeElement.value = formatted;
                this.control.control?.setValue(+formatted, { emitEvent: true });
            }
        }
    }
}
