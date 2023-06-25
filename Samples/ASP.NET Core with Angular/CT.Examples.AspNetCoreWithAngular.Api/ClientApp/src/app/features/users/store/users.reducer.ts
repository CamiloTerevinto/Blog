import { createReducer, on } from '@ngrx/store';
import { UserModel } from '../models/user.model';
import { UserActions } from './users.actions';

export interface UsersState {
    users: UserModel[]
};

const initialState: UsersState = {
    users: []
};

export const usersReducer = createReducer(initialState,
    on(UserActions.loadSuccess, (state, { users }) => ({ ...state, users })),
);
