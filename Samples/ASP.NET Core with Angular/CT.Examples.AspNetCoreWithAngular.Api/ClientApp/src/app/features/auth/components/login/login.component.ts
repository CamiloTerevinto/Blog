import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { LogInModel } from '../../models/models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  public hasLoginFailed: boolean;

  constructor(private readonly authService: AuthService, private readonly router: Router ) { 
    this.loginForm = new FormGroup({
      'email': new FormControl<string>('', [ Validators.email, Validators.required ]),
      'password': new FormControl<string>('', [ Validators.required ])
    });
  }

  ngOnInit(): void {
  }

  public onSubmit() {
    if (this.loginForm.invalid){
      return;
    }

    this.hasLoginFailed = false;

    const loginModel: LogInModel = this.loginForm.value;

    this.authService.login(loginModel)
      .subscribe(data => {
        if (data === true) {
          this.router.navigateByUrl('');
        } else {
          this.hasLoginFailed = true;
        }
      });
  }

}
