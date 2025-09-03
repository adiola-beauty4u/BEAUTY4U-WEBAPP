import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyscodesAutocompleteComponent } from './syscodes-autocomplete.component';

describe('SyscodesAutocompleteComponent', () => {
  let component: SyscodesAutocompleteComponent;
  let fixture: ComponentFixture<SyscodesAutocompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SyscodesAutocompleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SyscodesAutocompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
