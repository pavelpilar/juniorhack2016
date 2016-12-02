import { Input, Component } from '@angular/core';
import {AlertComponent, IAlert} from "./alert.component";

@Component({
  selector: 'my-app',
  template: `
    <navbar></navbar>
    <router-outlet></router-outlet>
    <alert-items [alerts] = alert></alert-items>`,
})
export class AppComponent{

  private alert: Array<IAlert> = [];
  private idAlert: number = 0;

  constructor(private Alerts: AlertComponent){};

  private AddAlert(message: string, type: string)
  {
    this.alert.push({id: this.idAlert, message: message, type: type});
    this.idAlert++;
    console.log(this.alert);
  }


}




