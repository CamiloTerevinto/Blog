import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { LoginResult } from './../models/login-result.model';
import { environment } from 'src/environments/environment';
import { LogInModel, SignUpModel } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AuthService {
    isLoggedIn = false;
    private authToken: string;
    private isAdmin: boolean | null = null;

    constructor(private readonly httpClient: HttpClient) {
        const storedToken = window.localStorage.getItem('authToken');
        if (storedToken) {
            this.authToken = storedToken;
            this.isLoggedIn = true;
        }
    }

    login(model: LogInModel): Observable<boolean> {
        return this.httpClient
            .post<LoginResult>(`${environment.baseUrl}authentication/sign-in`, model)
            .pipe(
                map(data => {
                    this.authToken = data.token;
                    window.localStorage.setItem('authToken', data.token);
                    this.isLoggedIn = true;
                    return true;
                }),
                catchError((err, caught) => { console.error(err); return of(false); })
            );
    }

    public registerAccount(model: SignUpModel): Observable<any> {
        return this.httpClient
            .post<LoginResult>(`${environment.baseUrl}authentication/sign-up`, model)
            .pipe(
                map(data => {
                    this.authToken = data.token;
                    window.localStorage.setItem('authToken', data.token);
                    this.isLoggedIn = true;
                    return true;
                }),
                catchError((err, caught) => { console.error(err); return of(false); })
            );
    }

    isUserAdmin() {
        if (this.isAdmin !== null) {
            return this.isAdmin === true;
        }

        const jwtData = this.authToken.split('.')[1];
        const decodedJwtJsonData = window.atob(jwtData);
        const decodedJwtData = JSON.parse(decodedJwtJsonData);

        this.isAdmin = decodedJwtData["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Admin";

        return this.isAdmin;
    }

    public get token() { return this.authToken; }
}