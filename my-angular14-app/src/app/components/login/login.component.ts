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
        if (res.success && res.token) {
          // Decode JWT payload
          const tokenParts = res.token.split('.');
          const payload = JSON.parse(atob(tokenParts[1]));

          const userRole = payload?.role?.toLowerCase();

          if (userRole === 'admin') {
            localStorage.setItem('token', res.token);
            this.router.navigate(['/dashboard']);
          } else {
            this.errorMessage = 'Only admins can access this page.';
            alert('⛔ Access denied: Only admins can login here.');
          }
        } else {
          this.errorMessage = res.message || 'Login failed.';
        }
      },
      error: (err) => {
        this.errorMessage = 'Invalid login. Please try again.';
        console.error(err);
      }
    });
  }


  ngOnInit(): void {
    const token = localStorage.getItem('token');

    if (token) {
      try {
        const tokenPayload = JSON.parse(atob(token.split('.')[1]));
        const userRole = tokenPayload?.role?.toLowerCase();

        if (userRole === 'admin') {
          this.router.navigate(['/dashboard']);
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
