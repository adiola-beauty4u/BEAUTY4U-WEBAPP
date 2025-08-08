import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendorSelectComponent } from './vendor-select.component';

describe('VendorSelectComponent', () => {
  let component: VendorSelectComponent;
  let fixture: ComponentFixture<VendorSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VendorSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VendorSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
