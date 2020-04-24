import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppContentHeaderComponent } from './app-content-header.component';

describe('AppContentHeaderComponent', () => {
  let component: AppContentHeaderComponent;
  let fixture: ComponentFixture<AppContentHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppContentHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppContentHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
