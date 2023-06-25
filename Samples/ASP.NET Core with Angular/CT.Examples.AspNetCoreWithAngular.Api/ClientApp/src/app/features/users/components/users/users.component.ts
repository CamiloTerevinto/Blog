import { Component, OnInit, ViewChild } from '@angular/core';
import { Actions, ofType } from '@ngrx/effects';
import { Store, select } from '@ngrx/store';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';

import { UserModel } from 'src/app/features/users/models/user.model';
import { selectUsers } from './../../store/users.selectors';
import { UserActions } from './../../store/users.actions';
import { EditUserDialogComponent } from './../edit-user-dialog/edit-user-dialog.component';
import { UsersState } from '../../store/users.reducer';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  displayedColumns: string[] = ['firstName', 'lastName', 'emailAddress', 'phoneNumber', 'actions'];
  dataSource: MatTableDataSource<UserModel>;

  users: UserModel[];

  @ViewChild(MatPaginator, { static: false })
  paginator: MatPaginator;

  constructor(private readonly store: Store<UsersState>,
    private readonly dialog: MatDialog,
    private readonly snackbar: MatSnackBar,
    private readonly actions$: Actions) {
    store.dispatch(UserActions.load());
  }

  ngOnInit(): void {
    this.store.pipe(select(selectUsers))
      .subscribe(users => {
        if (!users) {
          return;
        }

        this.users = users;
        this.dataSource = new MatTableDataSource<UserModel>(users);
        this.dataSource.paginator = this.paginator;
      });

    this.actions$
      .pipe(ofType(UserActions.updateSuccess))
      .subscribe(() => {
        this.users = null;
        this.store.dispatch(UserActions.load());
        this.snackbar.open("The user has been updated successfully.", "Close", { duration: 2000 });
      });
  }

  public applyFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.dataSource.filter = value.toLowerCase();
  }

  public editUser(user: UserModel) {
    const dialogRef = this.dialog.open(EditUserDialogComponent, {
      width: '500px',
      data: user
    });

    dialogRef.afterClosed().subscribe((userModel: UserModel) => {
      if (!userModel) {
        return;
      }

      userModel.userId = user.userId;

      this.store.dispatch(UserActions.update({ user: userModel }));
    });
  }
}
