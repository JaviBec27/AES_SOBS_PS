import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayOrderModuleComponent } from './pay-order-module.component';

describe('PayOrderModuleComponent', () => {
  let component: PayOrderModuleComponent;
  let fixture: ComponentFixture<PayOrderModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayOrderModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayOrderModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
