import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

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

  showAddUserModal = false;

  newUser = {
    firstName: '',
    lastName: '',
    emailAddress: '',
    phoneNumber: '',
    password: '',
    userImage: '',
    userType: 'user',

  };

  openModal() {
    this.showAddUserModal = true;
  }

  closeModal() {
    this.showAddUserModal = false;
    this.resetNewUser();
  }

  resetNewUser() {
    this.newUser = {
      firstName: '',
      lastName: '',
      emailAddress: '',
      phoneNumber: '',
      password: '',
      userImage: '',
      userType: 'user',
    };
  }

  users: User[] = [];
  constructor(private http: HttpClient, private router: Router) { }

  selectedImage: File | null = null;

  onFileSelected(event: any) {
    this.selectedImage = event.target.files[0];
  }



  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers(): void {
    const token = localStorage.getItem('token'); // get token after login

    if (!token) {
      console.error("No token found.");
      this.router.navigate(['/login']);
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

  showEditModal = false;
  selectedUser: any = {};

  editUser(user: any) {
    this.selectedUser = { ...user };
    this.showEditModal = true;
  }

  closeEditModal() {
    this.showEditModal = false;
  }

  updateUser(): void {
    const token = localStorage.getItem('token');
    const headers = {
      Authorization: `Bearer ${token}`
    };

    this.http.put(`https://tatvasoft-internship-progress.onrender.com/api/User/Update`, this.selectedUser, { headers })
      .subscribe({
        next: () => {
          this.fetchUsers(); // Refresh user list
          this.closeEditModal();
        },
        error: (err) => {
          console.error('Error updating user:', err);
        }
      });
  }



  addUser(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('Token not found');
      return;
    }

    const headers = {
      Authorization: `Bearer ${token}`
    };

    if (this.selectedImage) {
      this.newUser.userImage = this.selectedImage.name;
    }

    this.http.post('https://tatvasoft-internship-progress.onrender.com/api/User/Add', this.newUser, { headers })
      .subscribe({
        next: (res: any) => {
          this.fetchUsers(); // Refresh table
          this.closeModal();
        },
        error: (err) => {
          console.error('Error adding user:', err);
        }
      });
    console.log('Add new user clicked!');
  }



}
