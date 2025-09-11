import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActiveScheduledJobSelectComponent } from './active-scheduled-job-select.component';

describe('ActiveScheduledJobSelectComponent', () => {
  let component: ActiveScheduledJobSelectComponent;
  let fixture: ComponentFixture<ActiveScheduledJobSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActiveScheduledJobSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ActiveScheduledJobSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
