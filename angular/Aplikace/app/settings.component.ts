/**
 * Created by lukasfrajt on 02/12/2016.
 */
import {Component} from "@angular/core";


@Component({
  moduleId: module.id,
  selector: 'settings-component',
  template: '{{name}}',
  //templateUrl: 'templates/settings.component',
  //styleUrls: ['styles/settings.component.scss']

})
export class SettingsComponent{
  name:string ="SettingsPage";

}
