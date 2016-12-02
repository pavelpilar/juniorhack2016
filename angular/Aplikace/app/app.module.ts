import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent }  from './app.component';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {AlertComponent} from "./alert.component";
import {JsonpModule} from "@angular/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MenuComponent} from "./menu.component";
import {AppRoutingModule} from "./app.routing";
import {HomeComponent} from "./home.component";
import {ExterierComponent} from "./exterier.component";
import {InterierComponent} from "./interier.component";
import {HistoryComponent} from "./history.component";
import {SettingsComponent} from "./settings.component";

@NgModule({
  imports:      [ BrowserModule, AppRoutingModule, FormsModule, JsonpModule,ReactiveFormsModule,  NgbModule.forRoot()],
  declarations: [ AppComponent, AlertComponent,MenuComponent, HomeComponent, ExterierComponent, InterierComponent, HistoryComponent,SettingsComponent ],
  bootstrap:    [ AppComponent ],
  providers: [AlertComponent]
})
export class AppModule {

}
