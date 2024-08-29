import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ActivateguardService implements CanActivate {
  login: boolean = false;
  constructor(
    private router: Router,
    private jwtHelperService: JwtHelperService
  ) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    if (sessionStorage.getItem('currentUser')) {
      this.login = true;
      return true;
    } else {
      alert("You can't Access!!");
      this.router.navigateByUrl('/login');
      this.login = false;
      return false;
    }
  }
}
