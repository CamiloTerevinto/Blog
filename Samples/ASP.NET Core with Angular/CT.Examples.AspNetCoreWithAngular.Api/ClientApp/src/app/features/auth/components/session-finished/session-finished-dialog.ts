import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'session-finished-dialog',
    templateUrl: 'session-finished-dialog.html',
})
export class SessionFinishedDialog {
    constructor(
        private readonly dialogRef: MatDialogRef<SessionFinishedDialog>,
        private readonly router: Router
    ) { }

    onNoClick(): void {
        this.dialogRef.close();
    }

    onYesClick(): void {
        this.router.navigate(["/login"]);
        this.dialogRef.close();
    }
}