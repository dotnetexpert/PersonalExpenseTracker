import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtintercepterService } from './jwtintercepter.service';
import { JwtModule } from '@auth0/angular-jwt';
import { RegisterComponent } from './register/register.component';
import { ToastrModule } from 'ngx-toastr';
import { PersonalexpenseComponent } from './personalexpense/personalexpense.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HighchartsChartModule } from 'highcharts-angular';
import { MonthlychartComponent } from './charts/monthlychart/monthlychart.component';
import { CategorychartComponent } from './charts/categorychart/categorychart.component';
import { ExpensebycategoryComponent } from './charts/expensebycategory/expensebycategory.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    PersonalexpenseComponent,
    MonthlychartComponent,
    CategorychartComponent,
    ExpensebycategoryComponent,
  ],
  imports: [
    HighchartsChartModule,
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ToastrModule.forRoot({
      timeOut: 1000,
      positionClass: 'toast-top-left',
      preventDuplicates: true,
    }),
    FormsModule,
    ReactiveFormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return sessionStorage.getItem('currentUser')
            ? JSON.parse(sessionStorage.getItem('currentUser') as string).token
            : null;
        },
      },
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtintercepterService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
