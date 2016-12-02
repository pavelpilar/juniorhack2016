/**
 * Created by lukasfrajt on 02/12/2016.
 */
import { Input, Component } from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'alert-items',
  template: `<p *ngFor="let alert of alerts">
  <ngb-alert [type]="alert.type" (close)="closeAlert(alert)">{{ alert.message }}</ngb-alert>
</p>
<p>
  <button type="button" class="btn btn-primary" (click)="AddAlert()">Reset</button>
</p>`,
})
export class AlertComponent {
  @Input()
  public alerts: Array<IAlert> = [];

  private backup: Array<IAlert>;
  private id: number;

  constructor() {
    this.alerts.push({
      id: 1,
      type: 'success',
      message: 'This is an success alert',
    }, {
      id: 2,
      type: 'info',
      message: 'This is an info alert',
    }, {
      id: 4,
      type: 'danger',
      message: 'This is a danger alert',
    });
    this.backup = this.alerts.map((alert: IAlert) => Object.assign({}, alert));
  }

  public closeAlert(alert: IAlert) {
    const index: number = this.alerts.indexOf(alert);
    this.alerts.splice(index, 1);
  }

  public AddAlert(type: string, message: string){
    this.id++;
    this.alerts.push({ id: 5,type: type, message: message,
    })
    console.log(this.alerts);

  }

  public reset() {
    this.alerts = this.backup.map((alert: IAlert) => Object.assign({}, alert));
  }
}


export interface IAlert {
  id: number;
  type: string;
  message: string;

}
