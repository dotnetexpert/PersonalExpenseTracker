import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlychartComponent } from './monthlychart.component';

describe('MonthlychartComponent', () => {
  let component: MonthlychartComponent;
  let fixture: ComponentFixture<MonthlychartComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MonthlychartComponent]
    });
    fixture = TestBed.createComponent(MonthlychartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
