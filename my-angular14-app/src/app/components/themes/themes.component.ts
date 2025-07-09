import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-themes',
  templateUrl: './themes.component.html',
  styleUrls: ['./themes.component.css']
})
export class ThemesComponent implements OnInit {

  themes: any[] = [];
  newTheme = {
    title: '',
    isActive: true
  };
  selectedTheme: any = {};
  showAddThemeModal = false;
  showEditModal = false;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchThemes();
  }

  fetchThemes(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    this.http.get<any>('https://tatvasoft-internship-progress.onrender.com/api/MissionTheme/GetAll', { headers })
      .subscribe({
        next: (res) => this.themes = res.data,
        error: (err) => console.error('Error fetching themes:', err)
      });
  }

  openModal(): void {
    this.newTheme = { title: '', isActive: true };
    this.showAddThemeModal = true;
  }

  closeModal(): void {
    this.showAddThemeModal = false;
  }

  addTheme(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    this.http.post<any>('https://tatvasoft-internship-progress.onrender.com/api/MissionTheme/Add', this.newTheme, { headers })
      .subscribe({
        next: () => {
          this.fetchThemes();
          this.closeModal();
        },
        error: (err) => console.error('Error adding theme:', err)
      });
  }

  editTheme(theme: any): void {
    this.selectedTheme = { ...theme };
    this.showEditModal = true;
  }

  closeEditModal(): void {
    this.showEditModal = false;
  }

  updateTheme(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    const payload = {
      id: this.selectedTheme.id,
      title: this.selectedTheme.title,
      isActive: this.selectedTheme.isActive
    };

    this.http.put<any>('https://tatvasoft-internship-progress.onrender.com/api/MissionTheme/Update', payload, { headers })
      .subscribe({
        next: () => {
          this.fetchThemes();
          this.closeEditModal();
        },
        error: (err) => console.error('Error updating theme:', err)
      });
  }


  deleteTheme(themeId: number): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    this.http.delete<any>(`https://tatvasoft-internship-progress.onrender.com/api/MissionTheme/Delete/${themeId}`, { headers })
      .subscribe({
        next: () => this.fetchThemes(),
        error: (err) => console.error('Error deleting theme:', err)
      });
  }

}
