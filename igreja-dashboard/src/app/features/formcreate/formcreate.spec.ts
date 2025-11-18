import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Formcreate } from './formcreate';

describe('Formcreate', () => {
  let component: Formcreate;
  let fixture: ComponentFixture<Formcreate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Formcreate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Formcreate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
