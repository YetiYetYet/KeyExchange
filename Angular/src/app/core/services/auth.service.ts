import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Observable} from "rxjs";
import {RegisterDto} from "../../pages/register/register-dto";

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const authUri = environment.apiUrl + "/Auth"

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(authUri + '/login', { username: username, password: password}, httpOptions);
  }

  register(firstname: string | undefined, lastname: string | undefined,
           discord: string | undefined, phoneNumber: string | undefined,
           username: string, email: string, password: string, confirmPassword: string): Observable<any> {
    return this.http.post(authUri + '/register', {
      firstname,
      lastname,
      discord,
      phoneNumber,
      username,
      email,
      password,
      confirmPassword
    }, httpOptions);
  }

  registerWithDto(registerDto: RegisterDto | undefined): Observable<any> {
    if(registerDto == undefined) {
      throw new Error("registerDto is required and it actually undefined")
    }
    return this.http.post(authUri + '/register', {
      registerDto,
    }, httpOptions);
  }
}
