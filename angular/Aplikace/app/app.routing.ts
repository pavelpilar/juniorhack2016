/**
 * Created by lukasfrajt on 02/12/2016.
 */
import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from "./home.component";
import {ExterierComponent} from "./exterier.component";
import {InterierComponent} from "./interier.component";
import {SettingsComponent} from "./settings.component";
import {HistoryComponent} from "./history.component";


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home',  component: HomeComponent},
  { path: 'exterier', component: ExterierComponent },
  { path: 'interier', component: InterierComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'history', component: HistoryComponent }
];
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
