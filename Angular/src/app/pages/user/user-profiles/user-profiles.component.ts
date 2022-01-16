import { Component, OnInit } from '@angular/core';
import {UserProfileService} from "../services/user-profile.service";
import {ActivatedRoute, Router} from "@angular/router";
import {UserProfileDto} from "../user-profile-dto";

@Component({
  selector: 'app-user-profiles',
  templateUrl: './user-profiles.component.html',
  styleUrls: ['./user-profiles.component.css']
})
export class UserProfilesComponent implements OnInit {

  userProfile: UserProfileDto | undefined;
  id: string = "";

  constructor(public userProfileService: UserProfileService, public route: ActivatedRoute, public rooter: Router) {

  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => this.id = <string>params.get("id"));
    this.userProfileService.getUserProfilesById(this.id).subscribe({
      next: data => this.userProfile = data,
      error: err => {
        console.log(err);
        this.rooter.navigate(['/404']);
      }
    });
  }
}
