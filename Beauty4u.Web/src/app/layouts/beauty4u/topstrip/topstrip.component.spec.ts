import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopstripComponent } from './topstrip.component';

describe('TopstripComponent', () => {
  let component: TopstripComponent;
  let fixture: ComponentFixture<TopstripComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopstripComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopstripComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
