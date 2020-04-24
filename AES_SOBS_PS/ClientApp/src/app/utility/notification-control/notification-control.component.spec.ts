import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationControlComponent } from './notification-control.component';

describe('NotificationControlComponent', () => {
  let component: NotificationControlComponent;
  let fixture: ComponentFixture<NotificationControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
