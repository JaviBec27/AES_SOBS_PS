import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppSidebarMinimizerComponent } from './app-sidebar-minimizer.component';

describe('AppSidebarMinimizerComponent', () => {
  let component: AppSidebarMinimizerComponent;
  let fixture: ComponentFixture<AppSidebarMinimizerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppSidebarMinimizerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppSidebarMinimizerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
