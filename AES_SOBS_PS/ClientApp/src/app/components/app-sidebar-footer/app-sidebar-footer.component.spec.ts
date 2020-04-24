import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppSidebarFooterComponent } from './app-sidebar-footer.component';

describe('AppSidebarFooterComponent', () => {
  let component: AppSidebarFooterComponent;
  let fixture: ComponentFixture<AppSidebarFooterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppSidebarFooterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppSidebarFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
