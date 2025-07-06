import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface User {
  id: number;
  firstName: string;
  lastName: string;
  emailAddress: string;
  userType: string;
}

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  users: User[] = [];
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers(): void {
    const token = localStorage.getItem('token'); // get token after login

    if (!token) {
      console.error("No token found.");
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`
    };

    this.http.get<{ success: boolean, message: string, data: User[] }>('https://tatvasoft-internship-progress.onrender.com/api/User/All', { headers })
      .subscribe({
        next: (res) => {
          this.users = res.data;
        },
        error: (err) => {
          console.error('Error fetching users:', err);
        }
      });
  }

  deleteUser(userId: number): void {

    const token = localStorage.getItem('token');
    if (!token) {
      console.error("No token found.");
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`
    };

    if (confirm('Are you sure to delete this user?')) {
      this.http.delete(`https://tatvasoft-internship-progress.onrender.com/api/User/Delete/${userId}`, { headers }).subscribe(() => {
        this.users = this.users.filter(u => u.id !== userId);
      });
    }
  }

  editUser(user: User): void {
    // Later you can navigate to an edit page or show modal
    console.log('Editing user:', user);
  }

  addUser(): void {
    // Later show modal or route to Add User page
    console.log('Add new user clicked!');
  }



}
