import { Component, OnInit } from '@angular/core';
import { Validators, FormControl, FormGroup, ValidatorFn, ValidationErrors, AbstractControl, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { SignUpModel } from '../../models/models';

interface SignUpForm {
    firstName: FormControl<string>;
    lastName: FormControl<string>;
    email: FormControl<string>;
    password: FormControl<string>;
    repeatPassword: FormControl<string>;
    phoneNumber: FormControl<string>;
}

@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.scss']
})
export class SignUpComponent {
    public loginForm: FormGroup<SignUpForm>;
    public hasSignUpFailed: boolean;

    constructor(private readonly authService: AuthService, private readonly router: Router) {
        const builder = new FormBuilder();
        this.loginForm = builder.nonNullable.group<SignUpForm>({
            firstName: new FormControl<string>('', [Validators.required]),
            lastName: new FormControl<string>('', [Validators.required]),
            email: new FormControl<string>('', [Validators.email, Validators.required]),
            password: new FormControl<string>('', [Validators.required]),
            repeatPassword: new FormControl<string>('', [Validators.required]),
            phoneNumber: new FormControl<string>('', [Validators.required])
        }, { validators: passwordsMatchValidator })
    }

    public onSubmit() {
        if (this.loginForm.invalid) {
            return;
        }

        this.hasSignUpFailed = false;

        const signUpModel: SignUpModel = this.loginForm.getRawValue();

        this.authService.registerAccount(signUpModel)
            .subscribe(data => {
                if (data === true) {
                    this.router.navigateByUrl('');
                } else {
                    this.hasSignUpFailed = true;
                }
            });
    }
}

const passwordsMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const pass = control.get('password');
    const confirmPass = control.get('repeatPassword');

    return pass && confirmPass && pass.value === confirmPass.value ? null : { passwordMismatch: true };
};