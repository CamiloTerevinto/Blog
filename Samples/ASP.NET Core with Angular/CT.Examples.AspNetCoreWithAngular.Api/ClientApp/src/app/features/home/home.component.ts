import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/features/auth/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public isUserAdmin: boolean;

  constructor(private readonly authService: AuthService) { }

  ngOnInit(): void {
    this.isUserAdmin = this.authService.isUserAdmin();
  }
}
