/**
 * Created by lukasfrajt on 02/12/2016.
 */
import {Component} from "@angular/core";


@Component({
  moduleId: module.id,
  selector: 'exterior-component',
  template: '{{name}}',
  //templateUrl: 'templates/exterior.component',
  //styleUrls: ['styles/exterior.component.scss']

})
export class ExteriorComponent{
  name:string ="ExteriePage";

}
