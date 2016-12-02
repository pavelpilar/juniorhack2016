/**
 * Created by lukasfrajt on 02/12/2016.
 */
import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from "./home.component";
import {ExteriorComponent} from "./exterior.component";
import {InteriorComponent} from "./interior.component";
import {SettingsComponent} from "./settings.component";
import {HistoryComponent} from "./history.component";


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home',  component: HomeComponent},
  { path: 'exterior', component: ExteriorComponent },
  { path: 'interior', component: InteriorComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'history', component: HistoryComponent }
];
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
