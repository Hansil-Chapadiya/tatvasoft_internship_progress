import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MissionApplicationsComponent } from './mission-applications.component';

describe('MissionApplicationsComponent', () => {
  let component: MissionApplicationsComponent;
  let fixture: ComponentFixture<MissionApplicationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MissionApplicationsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MissionApplicationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
