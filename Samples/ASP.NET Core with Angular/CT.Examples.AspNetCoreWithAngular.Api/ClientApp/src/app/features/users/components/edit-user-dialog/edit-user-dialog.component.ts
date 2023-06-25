import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserModel } from 'src/app/features/users/models/user.model';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-edit-user-dialog',
  templateUrl: './edit-user-dialog.component.html',
  styleUrls: ['./edit-user-dialog.component.css']
})
export class EditUserDialogComponent {

  editUserForm: UntypedFormGroup;

  constructor(public readonly dialogRef: MatDialogRef<EditUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserModel) { 
      this.editUserForm = new UntypedFormGroup({
        'emailAddress': new UntypedFormControl(data.emailAddress, [ Validators.required, Validators.email ]),
        'password': new UntypedFormControl(''),
        'firstName': new UntypedFormControl(data.firstName, [ Validators.required ]),
        'lastName': new UntypedFormControl(data.lastName, [ Validators.required ]),
        'phoneNumber': new UntypedFormControl(data.phoneNumber, [ Validators.required ])
      });
    }

    onCancel(): void {
      this.dialogRef.close();
    }
}
