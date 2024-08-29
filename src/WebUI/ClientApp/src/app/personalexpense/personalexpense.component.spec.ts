import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalexpenseComponent } from './personalexpense.component';

describe('PersonalexpenseComponent', () => {
  let component: PersonalexpenseComponent;
  let fixture: ComponentFixture<PersonalexpenseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PersonalexpenseComponent]
    });
    fixture = TestBed.createComponent(PersonalexpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
