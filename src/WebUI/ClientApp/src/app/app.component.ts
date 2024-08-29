import { ActivateguardService } from './activateguard.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Personal Expense Tracker';
  isExpanded: boolean = true;
  session: any;
  constructor(
    private route: Router,
    public activateguardService: ActivateguardService
  ) {
    this.session = sessionStorage.getItem('currentUser');
  }
  logOut() {
    sessionStorage.clear();
    this.activateguardService.login = false;
    this.session = undefined;
    this.route.navigateByUrl('/login');
  }
}
