/**
 * Created by lukasfrajt on 02/12/2016.
 */

import {Component, OnInit} from "@angular/core";
import {Device} from "./model/Device";


@Component({
  moduleId: module.id,
  selector: 'home-component',
  templateUrl: 'templates/home.component.html',
  styleUrls: ['styles/home.component.scss']

})
export class HomeComponent implements OnInit{

  private boxOne: string ="Interior";
  private boxTwo: string ="Exterior";
  private boxThree: string ="History";
  private boxFour: string = "Settings"

  private interiorDevice: Device;
  private exteriorDevice: Device;

  ngOnInit():void {
    this.interiorDevice = new Device("5", "28", "10", "40");
    this.exteriorDevice = new Device("5", "28", "10", "40")
  }

}

