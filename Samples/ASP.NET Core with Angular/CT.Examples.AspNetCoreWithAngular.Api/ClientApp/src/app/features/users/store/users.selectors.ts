import { createFeatureSelector, createSelector } from '@ngrx/store';
import { UsersState } from './users.reducer';

export const getUsersState = createFeatureSelector<UsersState>('Users');

export const selectUsers = createSelector(getUsersState, 
    (state: UsersState) => state.users 
);