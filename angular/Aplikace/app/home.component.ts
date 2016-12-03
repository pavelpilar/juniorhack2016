/**
 * Created by lukasfrajt on 02/12/2016.
 */

import {Component, OnInit} from "@angular/core";
import {Device} from "./model/Device";
import {DataService} from "./app.service";
import './rxjs-operators';
import {Settings} from "./model/Settings";
import {Security} from "./model/Security";


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
  private security: Security[];

  private settingsGi: Settings[];

  private settingsWindow: number = 0;
  private settingsHeat: number = 0;


  errorMessage: string;
  private firstTime = true;

  private alerts: Array<IAlert> = [];


  private idAlert: number = 0;

  constructor(private dataService: DataService ) {


  }

  ngOnInit(): void {
    this.getFirstTime();
    if(this.firstTime == false)
    {
      this.getLastItem();
      this.getOutDevice();
      this.getSettings();
      this.getTest();
      this.getSecurity();
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
      this.dataService.getSecurity().subscribe(security => this.security = security,
        error => this.errorMessage = <any>error);
      this.firstTime = false;

    }


}
  getTest(){
    this.dataService.getDevices().subscribe(settings => {this.devices = settings; },
      error => this.errorMessage = <any>error);

  }




  getSettings() {
    setInterval(() => {
      this.dataService.getOutSettings().subscribe(settings => {this.settings = settings; this.settingsGi = this.settings;
          if(this.settingsWindow != this.settingsGi["0"]["otevreni_oken"])
          {
            if(this.settingsWindow == 0)
              this.AddAlert("Okno se otevírá", "warning");
            else if(this.settingsWindow == 1)
              this.AddAlert("Okno se zavírá", "warning");

          }
          if(this.settingsHeat != this.settingsGi["0"]["zapnuti_topeni"])
          {
            if(this.settingsHeat == 0)
              this.AddAlert("Topení bylo zapnuto", "warning");
            else if(this.settingsHeat == 1)
              this.AddAlert("Topeni bylo vypnuto", "warning");

          }

          this.settingsWindow = this.settingsGi["0"]["otevreni_oken"];



          this.settingsHeat = this.settingsGi["0"]["zapnuti_topeni"];

      },
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
  getSecurity() {
    setInterval(() => {
      this.dataService.getSecurity().subscribe(security => {this.security = security;
      if(this.security["0"]["security"] == 1){
        this.AddAlert("Poplach", "danger");
      }},
        error => this.errorMessage = <any>error);
    }, 3000);
  }

  private AddAlert(message: string, type: string)
  {
    this.alerts.push({id: this.idAlert, message: message, type: type});
    this.idAlert++;

  }


}

export interface IAlert {
  id: number;
  type: string;
  message: string;
}

