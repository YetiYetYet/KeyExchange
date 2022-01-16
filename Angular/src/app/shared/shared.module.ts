import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HeaderComponent} from "./layout/header/header.component";
import {MaterialModule} from "../material.module";
import {RouterModule} from "@angular/router";
import {FooterComponent} from "./layout/footer/footer.component";
import { NavMenuComponent } from './layout/nav-menu/nav-menu.component';


const sharedComponents = [HeaderComponent, FooterComponent, NavMenuComponent]
@NgModule({
  declarations: [...sharedComponents],
  imports: [CommonModule, RouterModule, MaterialModule],
  exports: [...sharedComponents],
  //providers: [ErrorDialogService, LoadingDialogService],
  entryComponents: [...sharedComponents],
})
export class SharedModule { }
