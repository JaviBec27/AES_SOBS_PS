import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppSidebarFormComponent } from './app-sidebar-form.component';

describe('AppSidebarFormComponent', () => {
  let component: AppSidebarFormComponent;
  let fixture: ComponentFixture<AppSidebarFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppSidebarFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppSidebarFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
