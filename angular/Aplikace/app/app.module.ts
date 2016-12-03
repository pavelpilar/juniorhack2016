import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent }  from './app.component';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NgbdAlertCloseable} from "./alert.component";
import {JsonpModule, HttpModule} from "@angular/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MenuComponent} from "./menu.component";
import {AppRoutingModule} from "./app.routing";
import {HomeComponent} from "./home.component";
import {ExteriorComponent} from "./exterior.component";
import {InteriorComponent} from "./interior.component";
import {HistoryComponent} from "./history.component";
import {SettingsComponent} from "./settings.component";
import {DataService} from "./app.service";



@NgModule({
  imports:      [ BrowserModule, AppRoutingModule,HttpModule, FormsModule, JsonpModule,ReactiveFormsModule,  NgbModule.forRoot()],
  declarations: [ AppComponent, NgbdAlertCloseable,MenuComponent, HomeComponent, ExteriorComponent, InteriorComponent, HistoryComponent,SettingsComponent ],
  bootstrap:    [ AppComponent ],
  providers: [NgbdAlertCloseable, DataService]
})
export class AppModule {

}
