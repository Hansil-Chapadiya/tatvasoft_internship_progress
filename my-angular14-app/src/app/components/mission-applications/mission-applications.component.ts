import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface MissionApplication {
  applicationId: number;
  missionTitle: string;
  userId: number;
  missionId: number;
  applicationDate: string;
  userName: string;
  status: string;
}

@Component({
  selector: 'app-mission-applications',
  templateUrl: './mission-applications.component.html',
  styleUrls: ['./mission-applications.component.css']
})
export class MissionApplicationsComponent implements OnInit {

  missionApplications: MissionApplication[] = [];
  constructor(private http: HttpClient, private router: Router) { }


  ngOnInit(): void {
    this.fetchMissionApplications();
  }

  fetchMissionApplications(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
      return;
    };
    const headers = { 'Authorization': `Bearer ${token}` };

    // Simulating an HTTP request to fetch mission applications
    this.http.get<any>('https://tatvasoft-internship-progress.onrender.com/GetAll', { headers })
      .subscribe(
        {
          next: (res) => this.missionApplications = res.data,
          error: (err) => console.error('Error fetching missions:', err)
        }
      );
  }

  approveMissison(applicationId: number): void {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    const headers = { 'Authorization': `Bearer ${token}` };

    this.http.put<any>(
      `https://tatvasoft-internship-progress.onrender.com/Approve/${applicationId}`, {}, { headers }
    ).subscribe({
      next: (res) => {
        this.showAlert('✅ Application approved successfully!');
        this.fetchMissionApplications();
      },
      error: (err) => {
        const msg = err?.error?.message || '❌ Error approving application.';
        this.showAlert(`⚠️ ${msg}`);
      }
    });
  }


  rejectMission(applicationId: number): void {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    const headers = { 'Authorization': `Bearer ${token}` };

    this.http.put<any>(
      `https://tatvasoft-internship-progress.onrender.com/Reject/${applicationId}`, {}, { headers }
    ).subscribe({
      next: (res) => {
        this.showAlert('❌ Application rejected successfully!');
        this.fetchMissionApplications();
      },
      error: (err) => {
        const msg = err?.error?.message || '❌ Error rejecting application.';
        this.showAlert(`⚠️ ${msg}`);
      }
    });
  }


  alertMessage: string = '';

  showAlert(message: string): void {
    this.alertMessage = message;
    setTimeout(() => this.alertMessage = '', 5000);
  }

  closeAlert(): void {
    this.alertMessage = '';
  }


}
