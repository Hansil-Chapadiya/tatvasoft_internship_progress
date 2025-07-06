import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  emailAddress: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private http: HttpClient, private router: Router) { }

  login() {
    const url = 'https://tatvasoft-internship-progress.onrender.com/api/Login/Login';

    const params = new HttpParams()
      .set('EmailAddress', this.emailAddress)
      .set('Password', this.password);

    this.http.post<any>(url, null, { params }).subscribe({
      next: (res: any) => {
        if (res.success) {
          localStorage.setItem('token', res.token); // if you return JWT
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = res.message;
        }
      },
      error: (err) => {
        this.errorMessage = 'Invalid login. Please try again.';
        console.error(err);
      }
    });
  }

  ngOnInit(): void { }
}
