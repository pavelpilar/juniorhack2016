import {Input, Component, OnInit} from '@angular/core';
import {NgbdAlertCloseable, IAlert} from "./alert.component";
import Alert = webdriver.Alert;
import {DataService} from "./app.service";
import {Device} from "./model/Device";
// Add the RxJS Observable operators.
import './rxjs-operators';


@Component({
  moduleId: module.id,
  selector: 'my-app',
  template: `
    <navbar></navbar>
    <router-outlet></router-outlet>
    <ngbd-alert-closeable [alerts]="alerts"></ngbd-alert-closeable>
    <button class="btn btn-primary" (click)="AddAlert()">Test</button>
    <ul>
  <li *ngFor="let device of devices">{{device.id_senzoru}}</li>
</ul>`,
})
export class AppComponent implements OnInit{
  private alerts: Array<IAlert> = [];


  constructor(private dataService: DataService){}


  private AddAlert()
  {
    this.alerts.push({id:5, message: "test", type: "danger"},{id:5, message: "oajoi", type: "danger"})

  }
  ngOnInit(){


  }


}
export interface IAlert {
  id: number;
  type: string;
  message: string;
}





