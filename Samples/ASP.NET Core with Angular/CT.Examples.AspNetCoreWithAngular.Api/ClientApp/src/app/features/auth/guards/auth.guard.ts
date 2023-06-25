import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(private readonly authService: AuthService,
                private readonly router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.authService.isLoggedIn) {
            return true;
        }

        return this.router.parseUrl('/login');
    }
}