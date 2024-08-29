import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserClient } from '../web-api-client';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm!: FormGroup;
  submitted: boolean = false;

  constructor(
    private formbuilder: FormBuilder,
    private router: Router,
    private toastr: ToastrService,
    private userClient: UserClient
  ) {}

  ngOnInit() {
    this.registerForm = this.formbuilder.group(
      {
        firstName: ['', [Validators.required, this.noWhitespaceValidator]],
        Email: [
          '',
          [
            Validators.required,
            Validators.email,
            Validators.pattern(
              '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$'
            ),
          ],
        ],
        PhoneNumber: [
          '',
          [
            Validators.required,
            Validators.pattern('^((\\+91-?)|0)?[0-9]{10,13}$'),
            Validators.minLength(10),
            Validators.maxLength(13),
          ],
        ],
        Password: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
          ],
        ],
        ConfirmPassword: ['', Validators.required],
      },
      {
        validator: this.ConfirmPasswordValidator('Password', 'ConfirmPassword'),
      }
    );
  }
  get f(): { [key: string]: AbstractControl } {
    return this.registerForm.controls;
  }

  signUp() {
    debugger
    this.submitted = true;
    if (this.registerForm.invalid) {
      return;
    }
    var userDetail = {
      UserName: this.registerForm.value.firstName,
      Email: this.registerForm.value.Email,
      PhoneNumber: this.registerForm.value.PhoneNumber,
      Password: this.registerForm.value.Password,
    };
    this.userClient
      .register(
        this.registerForm.value.firstName,
        this.registerForm.value.Email,
        this.registerForm.value.PhoneNumber,
        this.registerForm.value.Password
      )
      .subscribe(
        (response) => {
          this.submitted = false;
          this.registerForm.reset();
          this.toastr.success('Register Successfully.', 'Success');
          this.registerForm.patchValue({
            firstName: '',
            Email: '',
            PhoneNumber: 0,
            password: '',
          });
          this.router.navigateByUrl('login');
        },
        (error) => {
          console.log(error);
        }
      );
  }

  ConfirmPasswordValidator(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      let control = formGroup.controls[controlName];
      let matchingControl = formGroup.controls[matchingControlName];
      if (
        matchingControl.errors &&
        !matchingControl.errors['confirmPasswordValidator']
      ) {
        return;
      }
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ confirmPasswordValidator: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }
  noWhitespaceValidator(control: FormControl): ValidationErrors | null {
    const value = control.value || '';

    if (/^\s*$/.test(value)) {
      return { whitespace: true };
    }
    return null;
  }
}
