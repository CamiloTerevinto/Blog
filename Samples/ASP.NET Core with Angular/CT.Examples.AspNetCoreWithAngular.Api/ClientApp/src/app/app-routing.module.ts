import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './features/auth/guards/auth.guard';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/auth/components/login/login.component';
import { SignUpComponent } from './features/auth/components/signup/signup.component';
import { UsersComponent } from './features/users/components/users/users.component';

const routes: Routes = [
  { 
    path: '', 
    component: HomeComponent, 
    canActivate: [AuthGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: '/users' },
      { path: 'users', component: UsersComponent },
    ]
  },
  { path : 'login', component: LoginComponent },
  { path : 'signup', component: SignUpComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
