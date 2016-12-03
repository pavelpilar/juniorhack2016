import {Input, Component, OnInit} from '@angular/core';
import {NgbdAlertCloseable, IAlert} from "./alert.component";
import Alert = webdriver.Alert;
import {DataService} from "./app.service";
import {Device} from "./model/Device";
// Add the RxJS Observable operators.



@Component({
  moduleId: module.id,
  selector: 'my-app',
  template: `
    <navbar></navbar>
    <router-outlet></router-outlet>
    <ngbd-alert-closeable [alerts]="alerts"></ngbd-alert-closeable>
    <button class="btn btn-primary" (click)="AddAlert()">Test</button>
    `,
})
export class AppComponent {
  private alerts: Array<IAlert> = [];


  constructor(private dataService: DataService){}


  private AddAlert()
  {
    this.alerts.push({id:5, message: "test", type: "danger"},{id:5, message: "oajoi", type: "danger"})

  }



}
export interface IAlert {
  id: number;
  type: string;
  message: string;
}





