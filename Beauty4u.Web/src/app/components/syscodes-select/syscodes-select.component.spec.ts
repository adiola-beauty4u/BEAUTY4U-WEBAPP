import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyscodesSelectComponent } from './syscodes-select.component';

describe('SyscodesSelectComponent', () => {
  let component: SyscodesSelectComponent;
  let fixture: ComponentFixture<SyscodesSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SyscodesSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SyscodesSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
