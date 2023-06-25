import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { UserModel } from '../models/user.model';
import { environment } from 'src/environments/environment.prod';

@Injectable({providedIn: 'root'})
export class UsersService {
    private serviceUrl = `${environment.baseUrl}accounts`;

    constructor(private readonly httpClient: HttpClient) { }
    
    public loadUsers() : Observable<UserModel[]> {
        return this.httpClient.get<UserModel[]>(this.serviceUrl);
    }

    public updateUser(model: UserModel) : Observable<void> {
        return this.httpClient.put<void>(`${this.serviceUrl}/${model.userId}`, model);
    }
}
