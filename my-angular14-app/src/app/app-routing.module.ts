import { importProvidersFrom, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { UsersComponent } from './components/users/users.component';
import { MissionComponent } from './components/mission/mission.component';
import { AdminGuard } from './guards/admin.guard';
import { UserGuard } from './guards/user.guard';
import { UserloginComponent } from './components/userlogin/userlogin.component';
import { UserhomepageComponent } from './components/userhomepage/userhomepage.component';
import { ThemesComponent } from './components/themes/themes.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: 'dashboard', component: DashboardComponent,
    canActivate: [AdminGuard],
    children: [
      { path: 'users', component: UsersComponent },
      { path: 'mission', component: MissionComponent },
      { path: 'themes', component: ThemesComponent },]
  },
  { path: 'sidebar', component: SidebarComponent },
  {
    path: 'userlogin', component: UserloginComponent,
  },
  {
    path: 'userhomepage', component: UserhomepageComponent,
    canActivate: [UserGuard]
  },

  { path: '**', redirectTo: 'login' }
  // Add future routes here like:
  // { path: 'dashboard', component: DashboardComponent },
  // { path: 'user', component: UserComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
