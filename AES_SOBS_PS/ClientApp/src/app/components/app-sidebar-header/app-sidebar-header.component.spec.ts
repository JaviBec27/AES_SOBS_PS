import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppSidebarHeaderComponent } from './app-sidebar-header.component';

describe('AppSidebarHeaderComponent', () => {
  let component: AppSidebarHeaderComponent;
  let fixture: ComponentFixture<AppSidebarHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppSidebarHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppSidebarHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
