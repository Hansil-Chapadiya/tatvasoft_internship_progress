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

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('No token found!');
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`
    };

    this.http.get<any>(
      'https://tatvasoft-internship-progress.onrender.com/api/Mission/GetAll',
      { headers }
    ).subscribe({
      next: (res) => this.missions = res.data,
      error: (err) => console.error('Mission fetch error:', err)
    });
  }


}
