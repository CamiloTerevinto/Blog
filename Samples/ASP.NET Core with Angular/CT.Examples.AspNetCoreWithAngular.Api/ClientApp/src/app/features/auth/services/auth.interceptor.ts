import { Injectable } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpUserEvent, HttpEventType
} from '@angular/common/http';

import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';
import { catchError, Observable, of, switchMap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { SessionFinishedDialog } from '../components/session-finished/session-finished-dialog';

export class AuthenticationFailedUserEvent implements HttpUserEvent<any> {
    type: HttpEventType.User;
}

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(
        private readonly authService: AuthService,
        private readonly dialog: MatDialog) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.url == `${environment.baseUrl}authentication/sign-in` || req.url == `${environment.baseUrl}authentication/sign-up`) {
            return next.handle(req);
        }

        const requestWithAuthorization = req.clone({ setHeaders: { Authorization: `Bearer ${this.authService.token}` } });

        return next.handle(requestWithAuthorization).pipe(
            catchError((err: any) => {
                if (err instanceof HttpErrorResponse && err.status === 401) {
                    const dialogRef = this.dialog.open(SessionFinishedDialog)

                    return dialogRef.afterClosed().pipe(switchMap(() => of(new AuthenticationFailedUserEvent())));
                } else {
                    throw err;
                }
            })
        );
    }
}
