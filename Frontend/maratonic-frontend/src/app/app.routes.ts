import { Routes } from '@angular/router';

// Pages
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { RacesListComponent } from './pages/races/races-list/races-list.component';
import { RaceDetailComponent } from './pages/races/race-detail/race-detail.component';
import { AdminDashboardComponent } from './pages/admin/dashboard/admin-dashboard.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';

export const routes: Routes = [

  // --- Public Routes ---
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // --- User Pages ---
  { path: 'profile', component: ProfileComponent },

  { path: 'races', component: RacesListComponent },
  { path: 'races/:id', component: RaceDetailComponent },

  // --- Admin Pages ---
  {
    path: 'admin',
    children: [
      { path: '', component: AdminDashboardComponent }
    ]
  },

  // --- 404 ---
  { path: '**', component: NotFoundComponent }
];
