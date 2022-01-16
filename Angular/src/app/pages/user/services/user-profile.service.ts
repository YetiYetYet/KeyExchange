import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {UserProfileDto} from "../user-profile-dto";

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  uriPublicGame = "https://localhost:7096/UserProfil"
  constructor(private http: HttpClient) { }

  getAllUserProfiles(): Observable<UserProfileDto[]>{
    return this.http.get<UserProfileDto[]>(this.uriPublicGame+"/all").pipe(map(data => data))
  }

  getUserProfilesById(id: string | null): Observable<UserProfileDto>{
    return this.http.get<UserProfileDto>(this.uriPublicGame+"/"+ id).pipe(map(data => data))
  }
}
