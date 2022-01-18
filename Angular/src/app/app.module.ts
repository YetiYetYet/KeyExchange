import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {MaterialModule} from "./material.module";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './pages/home/home.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import {AppRoutingModule} from "./app-rooting.module";
import {SharedModule} from "./shared/shared.module";
import { ContactComponent } from './pages/contact/contact.component';
import { GameComponent } from './pages/game/game.component';
import {HttpClientModule} from "@angular/common/http";
import { UserComponent } from './pages/user/user.component';
import { UserProfilesComponent } from './pages/user/user-profiles/user-profiles.component';
import { GameCardComponent } from './pages/game/game-card/game-card.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { EqualValidator } from './core/Validators/equal-validator.directive';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NotFoundComponent,
    ContactComponent,
    GameComponent,
    UserComponent,
    UserProfilesComponent,
    GameCardComponent,
    RegisterComponent,
    LoginComponent,
    EqualValidator,
  ],
  imports: [
    BrowserModule,
    MaterialModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
