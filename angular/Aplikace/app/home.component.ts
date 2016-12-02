/**
 * Created by lukasfrajt on 02/12/2016.
 */

import {Component} from "@angular/core";


@Component({
  moduleId: module.id,
  selector: 'home-component',
  //template: '{{name}}',
  templateUrl: 'templates/home.component.html',
  styleUrls: ['styles/home.component.scss']

})
export class HomeComponent{
  name:string ="HomePage";

}

