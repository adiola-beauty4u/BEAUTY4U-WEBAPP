import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Testpage2Component } from './testpage2.component';

describe('Testpage2Component', () => {
  let component: Testpage2Component;
  let fixture: ComponentFixture<Testpage2Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Testpage2Component]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Testpage2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
