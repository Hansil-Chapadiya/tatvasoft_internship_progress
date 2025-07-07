import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface Mission {
  id: number;
  missionTitle: string;
  startDate: string;
  endDate: string;
  city: string;
}
@Component({
  selector: 'app-mission',
  templateUrl: './mission.component.html',
  styleUrls: ['./mission.component.css']
})
export class MissionComponent implements OnInit {

  missions: Mission[] = [];
  constructor(private http: HttpClient, private router : Router) { }

  ngOnInit(): void {
    this.fetchMissions();
  }

  fetchMissions(): void {
    const token = localStorage.getItem('token'); // get token after login

    if (!token) {
      console.error("No token found.");
      this.router.navigate(['/login']);
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`
    };

    this.http.get<{ success: boolean, message: string, data: Mission[] }>('https://tatvasoft-internship-progress.onrender.com/api/Mission/GetAll', { headers })
      .subscribe({
        next: (res) => {
          this.missions = res.data;
        },
        error: (err) => {
          console.error('Error fetching missions:', err);
        }
      });
  }

  deleteMission(missionId: number): void {

    const token = localStorage.getItem('token');
    if (!token) {
      console.error("No token found.");
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`
    };

    if (confirm('Are you sure to delete this user?')) {
      this.http.delete(`https://tatvasoft-internship-progress.onrender.com/api/Mission/Delete/${missionId}`, { headers }).subscribe(() => {
        this.missions = this.missions.filter(m => m.id !== missionId);
      });
    }
  }
  editMission(mission: Mission): void {
    // Later you can navigate to an edit page or show modal
    console.log('Editing mission:', mission);
  }

  addMission(): void {
    // Later show modal or route to Add User page
    console.log('Add new mission clicked!');
  }
}
