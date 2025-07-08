import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface Mission {
  id?: number;
  missionTitle: string;
  missionDescription: string;
  startDate: string;
  endDate: string;
  totalSeats: number;
  missionImage: string;
  countryId: number;
  cityId: number;
  missionThemeId: number;
  skillIds: number[];

  city?: string;
  country?: string;
  theme?: string;
  skills?: string[];
}

@Component({
  selector: 'app-mission',
  templateUrl: './mission.component.html',
  styleUrls: ['./mission.component.css']
})
export class MissionComponent implements OnInit {

  missions: Mission[] = [];
  isModalOpen = false;
  isEditMode = false;
  selectedMissionId: number | null = null;

  newMission: Mission = {
    missionTitle: '',
    missionDescription: '',
    startDate: '',
    endDate: '',
    totalSeats: 0,
    missionImage: '',
    countryId: 0,
    cityId: 0,
    missionThemeId: 0,
    skillIds: []
  };

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.fetchMissions();
  }

  fetchMissions(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    const headers = { Authorization: `Bearer ${token}` };

    this.http.get<any>('https://tatvasoft-internship-progress.onrender.com/api/Mission/GetAll', { headers })
      .subscribe({
        next: (res) => this.missions = res.data,
        error: (err) => console.error('Error fetching missions:', err)
      });
  }

  handleSkillInput(event: Event): void {
    const input = (event.target as HTMLInputElement).value;
    this.newMission.skillIds = input
      .split(',')
      .map(x => +x.trim())
      .filter(x => !isNaN(x));
  }

  openAddModal(): void {
    this.isEditMode = false;
    this.isModalOpen = true;
    this.resetForm();
  }

  openEditModal(mission: any): void {
    this.isEditMode = true;
    this.selectedMissionId = mission.id;
    this.isModalOpen = true;
    this.newMission = {
      missionTitle: mission.missionTitle,
      missionDescription: mission.missionDescription,
      startDate: mission.startDate,
      endDate: mission.endDate,
      totalSeats: mission.totalSeats,
      missionImage: mission.missionImage,
      countryId: mission.countryId,
      cityId: mission.cityId,
      missionThemeId: mission.missionThemeId,
      skillIds: mission.skillIds ?? []  // ideal case: backend gives actual skillIds

    };
  }

  saveMission(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = { Authorization: `Bearer ${token}` };

    if (this.isEditMode && this.selectedMissionId) {
      const body = { missionID: this.selectedMissionId, ...this.newMission };
      this.http.put('https://tatvasoft-internship-progress.onrender.com/api/Mission/Update', body, { headers }).subscribe(() => {
        this.fetchMissions();
        this.isModalOpen = false;
      });
    } else {
      this.http.post('https://tatvasoft-internship-progress.onrender.com/api/Mission/Add', this.newMission, { headers }).subscribe(() => {
        this.fetchMissions();
        this.isModalOpen = false;
      });
    }
  }

  deleteMission(missionId: number): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = { Authorization: `Bearer ${token}` };

    if (confirm('Are you sure to delete this mission?')) {
      this.http.delete(`https://tatvasoft-internship-progress.onrender.com/api/Mission/Delete/${missionId}`, { headers }).subscribe(() => {
        this.missions = this.missions.filter(m => m.id !== missionId);
      });
    }
  }

  resetForm(): void {
    this.newMission = {
      missionTitle: '',
      missionDescription: '',
      startDate: '',
      endDate: '',
      totalSeats: 0,
      missionImage: '',
      countryId: 0,
      cityId: 0,
      missionThemeId: 0,
      skillIds: []
    };
  }
}
