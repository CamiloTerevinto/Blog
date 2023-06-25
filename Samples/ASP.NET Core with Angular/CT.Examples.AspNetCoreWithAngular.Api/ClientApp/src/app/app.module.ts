import { DatePipe } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

// Base modules/components
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './features/home/home.component';

// Auth components
import { LoginComponent } from './features/auth/components/login/login.component';
import { SessionFinishedDialog } from './features/auth/components/session-finished/session-finished-dialog';
import { SignUpComponent } from './features/auth/components/signup/signup.component';
import { AuthInterceptor } from './features/auth/services/auth.interceptor';

// User components
import { EditUserDialogComponent } from './features/users/components/edit-user-dialog/edit-user-dialog.component';
import { UsersComponent } from './features/users/components/users/users.component';
import { UsersEffects } from './features/users/store/users.effects';
import { usersReducer } from './features/users/store/users.reducer';

import { MatNativeDateModule } from '@angular/material/core';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        SignUpComponent,
        HomeComponent,
        UsersComponent,
        EditUserDialogComponent,
        SessionFinishedDialog
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MatInputModule,
        MatButtonModule,
        MatCardModule,
        MatSidenavModule,
        MatListModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatDialogModule,
        MatSnackBarModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatIconModule,
        MatMenuModule,
        MatDatepickerModule,
        MatNativeDateModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule,
        StoreModule.forRoot({
            'Users': usersReducer,
        }),
        EffectsModule.forRoot([
            UsersEffects,
        ])
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        DatePipe,
        { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { disableClose: true } }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
