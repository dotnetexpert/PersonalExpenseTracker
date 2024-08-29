import { ExpensebycategoryComponent } from './charts/expensebycategory/expensebycategory.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ActivateguardService } from './activateguard.service';
import { RegisterComponent } from './register/register.component';
import { PersonalexpenseComponent } from './personalexpense/personalexpense.component';
import { MonthlychartComponent } from './charts/monthlychart/monthlychart.component';
import { CategorychartComponent } from './charts/categorychart/categorychart.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'register', component: RegisterComponent },
  {
    path: 'dashboard',
    component: HomeComponent,
    canActivate: [ActivateguardService],
  },
  {
    path: 'personal',
    component: PersonalexpenseComponent,
    canActivate: [ActivateguardService],
  },
  { path: 'login', component: LoginComponent },
  { path: 'month', component: MonthlychartComponent },
  { path: 'category', component: CategorychartComponent },
  { path: 'expensebycategory', component: ExpensebycategoryComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
