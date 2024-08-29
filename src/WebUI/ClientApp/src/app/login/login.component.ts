import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { LoginViewModel, UserClient } from '../web-api-client';
import { ActivateguardService } from '../activateguard.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginViewModel: LoginViewModel = new LoginViewModel();
  loginForm!: FormGroup;
  submitted: boolean = false;
  constructor(
    private router: Router,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private userClient: UserClient,
    private activateguardService: ActivateguardService
  ) {

    
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(100),
        ],
      ],
    });
    this.submitted = false;
  }
  login() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    this.loginViewModel.userName = this.loginForm.value.email;
    this.loginViewModel.password = this.loginForm.value.password;
    this.userClient.login(this.loginViewModel).subscribe(
      (response) => {
        sessionStorage['currentUser'] = JSON.stringify(response);
        this.toastr.success('login successfully', 'Success');
        this.activateguardService.login = true;
        this.router.navigateByUrl('/dashboard');
      },
      (error) => {
        console.log(error);
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.loginForm.controls;
  }
}
