import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.css']
})
export class SkillsComponent implements OnInit {

  skills: any[] = [];
  newSkill = {
    name: '',
    isActive: true
  };

  selectedSkill: any = {};
  showAddSkillModal = false;
  showEditModal = false;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchSkills();
  }

  fetchSkills(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    this.http.get<any>('https://tatvasoft-internship-progress.onrender.com/api/Skill/GetAll', { headers })
      .subscribe({
        next: (res) => this.skills = res.data,
        error: (err) => console.error('Error fetching skills:', err)
      });
  }

  openModal(): void {
    this.newSkill = { name: '', isActive: true };
    this.showAddSkillModal = true;
  }
  closeModal(): void {
    this.showAddSkillModal = false;
  }
  addSkill(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    this.http.post<any>('https://tatvasoft-internship-progress.onrender.com/api/Skill/Add', this.newSkill, { headers })
      .subscribe({
        next: () => {
          this.fetchSkills();
          this.closeModal();
        },
        error: (err) => console.error('Error adding skill:', err)
      });
  }

  editSkill(skill: any): void {
    this.selectedSkill = { ...skill };
    this.showEditModal = true;
  }
  closeEditModal(): void {
    this.showEditModal = false;
  }
  updateSkill(): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    this.http.put<any>(`https://tatvasoft-internship-progress.onrender.com/api/Skill/Update`, this.selectedSkill, { headers })
      .subscribe({
        next: () => {
          this.fetchSkills();
          this.closeEditModal();
        },
        error: (err) => console.error('Error updating skill:', err)
      });
  }

  deleteSkill(skillId: number): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    this.http.delete<any>(`https://tatvasoft-internship-progress.onrender.com/api/Skill/Delete/${skillId}`, { headers })
      .subscribe({
        next: () => this.fetchSkills(),
        error: (err) => console.error('Error deleting skill:', err)
      });
  }
}
