import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, mergeMap, catchError, switchMap, tap } from 'rxjs/operators';

import { Action } from '@ngrx/store';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { UsersService } from './users.service';
import { UserActions } from './users.actions';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class UsersEffects {
    constructor(private readonly actions$: Actions, 
        private readonly usersService: UsersService,
        private readonly snackbar: MatSnackBar) {
    }
     
    loadUsers$: Observable<Action> = createEffect(() => this.actions$
        .pipe(
            ofType(UserActions.load),
            mergeMap(() => 
                this.usersService.loadUsers()
                    .pipe(
                        map(users => UserActions.loadSuccess({users})),
                        catchError(error => of(UserActions.error({ error, friendly: "An error occurred while loading users." })))
                    )
            )
        ));

     
    updateUsers$: Observable<Action> = createEffect(() => this.actions$
    .pipe(
        ofType(UserActions.update),
        map(action => action.user),
        switchMap(user => 
            this.usersService.updateUser(user)
                .pipe(
                    map(() => UserActions.updateSuccess()),
                    catchError(error => of(UserActions.error({ error, friendly: "An error occurred while attempting to update the user." })))
                )
        )
    ));
    
    raiseErrorMessage$: Observable<Action> = createEffect(() => this.actions$
        .pipe(
            ofType(UserActions.error),
            tap(action => {
                console.error(action.error);
                this.snackbar.open(action.friendly, "Close", { duration: 2000 });
            })
        ), { dispatch: false })
}
