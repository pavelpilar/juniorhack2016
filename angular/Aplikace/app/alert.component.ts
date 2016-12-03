import {Input, Component, OnInit, OnChanges} from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'ngbd-alert-closeable',
  styleUrls: ['./styles/alert.component.css'],
  template: `<p *ngFor="let alert of alerts" class="alerts">
                <ngb-alert [type]="alert.type" [class.smazat]="danger === alert.type" (close)="closeAlert(alert)">{{ alert.message }}</ngb-alert>
            </p>
            `,

})
export class NgbdAlertCloseable {

  @Input()
  public alerts: Array<IAlert> = [];
  public second: Array<IAlert> = [];
  private id: number = 0;
  private danger ="danger"


  constructor() {
  this.id = 0;
    this.alerts.push({
      id: 1,
      type: 'success',
      message: 'This is an success alert',
    });

  }

  public closeAlert(alert: IAlert) {
    const index: number = this.alerts.indexOf(alert);
    this.alerts.splice(index, 1);
  }


}

export interface IAlert {
  id: number;
  type: string;
  message: string;
}
