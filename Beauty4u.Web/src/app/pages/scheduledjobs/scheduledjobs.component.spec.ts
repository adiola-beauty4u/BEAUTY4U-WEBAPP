import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduledjobsComponent } from './scheduledjobs.component';

describe('ScheduledjobsComponent', () => {
  let component: ScheduledjobsComponent;
  let fixture: ComponentFixture<ScheduledjobsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScheduledjobsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScheduledjobsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
