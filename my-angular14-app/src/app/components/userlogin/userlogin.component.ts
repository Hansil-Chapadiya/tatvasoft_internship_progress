import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-userlogin',
  templateUrl: './userlogin.component.html',
  styleUrls: ['./userlogin.component.css']
})
export class UserloginComponent implements OnInit {

  emailAddress: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private http: HttpClient, private router: Router) { }

  userLogin() {
    const url = 'https://tatvasoft-internship-progress.onrender.com/api/Login/userLogin';

    const params = new HttpParams()
      .set('EmailAddress', this.emailAddress)
      .set('Password', this.password);

    this.http.post<any>(url, null, { params }).subscribe({
      next: (res: any) => {
        if (res.success && res.token) {
          // Decode JWT payload
          const tokenParts = res.token.split('.');
          const payload = JSON.parse(atob(tokenParts[1]));

          const userRole = payload?.role?.toLowerCase();

          if (userRole === 'user') {
            localStorage.setItem('token', res.token);
            this.router.navigate(['/userhomepage']);
          } else {
            this.errorMessage = 'Only users can access this page.';
            alert('⛔ Access denied: Only users can login here.');
          }
        } else {
          this.errorMessage = res.message || 'Login failed.';
        }
      }
      ,
      error: (err) => {
        if (err.status === 403) {
          alert(err.error?.message || 'Access denied');
        } else {
          alert('Something went wrong');
        }
      }
    });
  }

  ngOnInit(): void {
    const token = localStorage.getItem('token');

    if (token) {
      try {
        const tokenPayload = JSON.parse(atob(token.split('.')[1]));
        const userRole = tokenPayload?.role?.toLowerCase();

        if (userRole === 'user') {
          this.router.navigate(['/userhomepage']);
        } else {
          // Optional: clear token if user not admin
          localStorage.removeItem('token');
        }
      } catch (e) {
        console.error('Invalid token');
        localStorage.removeItem('token'); // clear corrupted token
      }
    }
  }
}
