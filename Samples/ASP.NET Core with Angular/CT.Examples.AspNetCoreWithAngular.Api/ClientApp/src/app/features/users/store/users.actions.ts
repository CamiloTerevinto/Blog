import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { UserModel } from '../models/user.model';

export const UserActions = createActionGroup({
    source: "Clients",
    events: {
        "Error": props<{ error: any, friendly: string }>(),
        "Load": emptyProps(),
        "Load Success": props<{ users: UserModel[] }>(),
        "Update": props<{ user: UserModel }>(),
        "Update Success": emptyProps()
    }
});
