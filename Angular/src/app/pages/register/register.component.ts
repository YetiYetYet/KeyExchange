import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../core/services/auth.service";
import {FormControl, FormGroup, Validator, Validators} from "@angular/forms";
import {EqualValidator} from "../../core/Validators/equal-validator.directive";
import {RegisterDto} from "./register-dto";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm = new FormGroup({
    firstname: new FormControl('', []),
    lastname: new FormControl('', []),
    discord: new FormControl('', []),
    phoneNumber: new FormControl('', []),
    username: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    confirmEmail: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]),
    confirmPassword: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(20)])
  })

  isSuccessful = false;
  isSignUpFailed = false;
  errorMessage = '';
  registerDto: RegisterDto | undefined;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {

  }

  ngOnSubmit(): void {
    this.registerDto = this.registerForm.value
    this.authService.registerWithDto(this.registerDto).subscribe({
        next: data => {
          console.log(data);
          this.isSuccessful = true;
          this.isSignUpFailed = false;
        },
        error: err => {
          this.errorMessage = err.error.message;
          this.isSignUpFailed = true;
        }
      }
    );
  }
}
