/**
 * Created by lukasfrajt on 02/12/2016.
 */

import {Component, OnInit} from "@angular/core";
import {Device} from "./model/Device";
import {DataService} from "./app.service";
import './rxjs-operators';
import {Settings} from "./model/Settings";


@Component({
  moduleId: module.id,
  selector: 'home-component',

  templateUrl: 'templates/home.component.html',
  styleUrls: ['styles/home.component.scss']

})
export class HomeComponent implements OnInit {

  private boxOne: string = "Interior";
  private boxTwo: string = "Exterior";
  private boxThree: string = "Settings";
  private boxFour: string = "Settings";

  private exteriorDevice: Device[];
  private devices: Device[];
  private interior: Device[];
  private settings: Settings[];


  errorMessage: string;
  private firstTime = true;

  constructor(private dataService: DataService ) {


  }

  ngOnInit(): void {
    this.getFirstTime();
    if(this.firstTime == false)
    {
      this.getLastItem();
      this.getOutDevice();
      this.getSetings();
    }



  }

  getFirstTime() {
    if(this.firstTime == true){
      this.dataService.getOutSettings().subscribe(settings => this.settings = settings,
        error => this.errorMessage = <any>error);
      this.dataService.getOutDevices().subscribe(devices => this.exteriorDevice = devices,
        error => this.errorMessage = <any>error);
      this.dataService.getLastItem().subscribe(device => this.interior = device,
        error => this.errorMessage = <any>error);
      this.firstTime = false;
    }


}



  getSetings() {
    setInterval(() => {
      this.dataService.getOutSettings().subscribe(settings => this.settings = settings,
        error => this.errorMessage = <any>error);
    }, 3000);
  }
  getOutDevice() {
    setInterval(() => {
      this.dataService.getOutDevices().subscribe(devices => this.exteriorDevice = devices,
        error => this.errorMessage = <any>error);
    }, 3000);
  }

  getLastItem() {
    setInterval(() => {
      this.dataService.getLastItem().subscribe(device => this.interior = device,
        error => this.errorMessage = <any>error);
    }, 3000);
  }


}

