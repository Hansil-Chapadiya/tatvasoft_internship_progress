import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Mission {
  id: number;
  missionTitle: string;
  description: string;
  startDate: string;
  endDate: string;
  seatsLeft: number;
  deadline: string;
  status: string; // "CLOSED" | "APPLIED" | "OPEN"
  imageUrl: string;
}

@Component({
  selector: 'app-user-homepage',
  templateUrl: './user-homepage.component.html',
  styleUrls: ['./user-homepage.component.css']
})

@Component({
  selector: 'app-userhomepage',
  templateUrl: './userhomepage.component.html',
  styleUrls: ['./userhomepage.component.css']
})


export class UserhomepageComponent implements OnInit {

  missions: Mission[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<any>('https://tatvasoft-internship-progress.onrender.com/api/Mission/GetAll')
      .subscribe({
        next: (res) => this.missions = res.data,
        error: (err) => console.error('Mission fetch error:', err)
      });
  }

}
