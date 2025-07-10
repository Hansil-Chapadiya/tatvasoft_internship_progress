import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Mission {
  id: number;
  missionTitle: string;
  missionDescription: string;
  startDate: string;
  endDate: string;
  totalSeats: number;
  status: string; // "CLOSED" | "APPLIED" | "OPEN"
  missionImage: string;
}

@Component({
  selector: 'app-userhomepage',
  templateUrl: './userhomepage.component.html',
  styleUrls: ['./userhomepage.component.css']
})

export class UserhomepageComponent implements OnInit {
  missions: Mission[] = [];
  userId: number | null = null;
  selectedMissionId: number | null = null;
  selectedMissionTitle: string = '';
  showConfirmationModal: boolean = false;
  showApplicationModal: boolean = false;
  userApplications: any[] = [];


  constructor(private http: HttpClient) { }

  getDeadline(startDate: string): Date {
    const start = new Date(startDate);
    start.setDate(start.getDate() - 10);
    return start;
  }

  checkIfApplied(missionId: number): void {
    const token = localStorage.getItem('token');
    if (!token || this.userId === null) {
      console.error('Missing token or userId');
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    const body = {
      userId: this.userId,
      missionId: missionId
    };
    console.log(missionId, this.userId);
    const url = 'https://tatvasoft-internship-progress.onrender.com/Applied';

    this.http.post<any>(url, body, { headers }).subscribe({
      next: (res) => {
        const mission = this.missions.find(m => m.id === missionId);
        if (mission) {
          mission.status = res.alreadyApplied ? 'APPLIED' : 'OPEN';
        }
      },
      error: (err) => console.error('Check status error:', err)
    });
  }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('No token found!');
      return;
    }

    this.userId = this.getUserIdFromToken(); // 👈 Extract from token

    const headers = {
      Authorization: `Bearer ${token}`
    };

    this.http.get<any>(
      'https://tatvasoft-internship-progress.onrender.com/api/Mission/GetAll',
      { headers }
    ).subscribe({
      next: (res) => {
        this.missions = res.data;

        // 🔁 Check for each mission
        this.missions.forEach(mission => {
          this.checkIfApplied(mission.id);
        });
      },

      error: (err) => console.error('Mission fetch error:', err)
    });
  }


  getUserIdFromToken(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return Number(payload.nameid); // Match with backend claim key
    } catch (error) {
      console.error('Invalid token format:', error);
      return null;
    }
  }

  applyForMission(missionId: number): void {
    const token = localStorage.getItem('token');
    if (!token || this.userId === null) {
      console.error('Token or userId missing!');
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    const body = {
      userId: this.userId,
      missionId: missionId
    };

    const apiUrl = 'https://tatvasoft-internship-progress.onrender.com/Apply';

    this.http.post<any>(apiUrl, body, { headers }).subscribe({
      next: (res) => {
        if (res.success) {
          const mission = this.missions.find(m => m.id === missionId);
          if (mission) mission.status = 'APPLIED';
          this.closeModal();
        }
      },
      error: (err) => {
        console.error('Application error:', err);
        this.closeModal();
      }

    });
  }
  closeModal(): void {
    this.showConfirmationModal = false;
    this.selectedMissionId = null;
    this.selectedMissionTitle = '';
  }

  confirmApply(missionId: number, missionTitle: string): void {
    this.selectedMissionId = missionId;
    this.selectedMissionTitle = missionTitle;
    this.showConfirmationModal = true;
  }

  viewMyApplications(): void {
    const token = localStorage.getItem('token');
    if (!token || this.userId === null) {
      console.error('Token or userId missing!');
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`
    };

    const url = `https://tatvasoft-internship-progress.onrender.com/MyApplications/${this.userId}`;

    this.http.get<any>(url, { headers }).subscribe({
      next: (res) => {
        this.userApplications = res.data;
        this.showApplicationModal = true;
      },
      error: (err) => {
        console.error('Error fetching applications:', err);
        this.userApplications = [];
      }
    });
  }



}

