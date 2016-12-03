/**
 * Created by lukasfrajt on 02/12/2016.
 */

import {Component, OnInit} from "@angular/core";
import {Device} from "./model/Device";
import {DataService} from "./app.service";


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
  private boxFour: string = "Settings";

  private interiorDevice: Device;
  private devices: Device[];
  private exteriorDevice: Device;
  errorMessage: string;



  constructor(private dataService: DataService){
    this.exteriorDevice.datum = new Date(1988, 3, 15);
    this.exteriorDevice.id_senzoru= "TESt";
    this.exteriorDevice.teplota= "30";
    this.exteriorDevice.vlhkost= "40";
  }



  ngOnInit():void {

    this.getLastDeviceInterior();
    this.getData();
  }
  getData()
  {
    this.dataService.getDevices().subscribe( devices => this.devices = devices,
      error =>  this.errorMessage = <any>error);
    console.log(this.devices);
    console.log(this.errorMessage);
  }
  getLastDeviceInterior()
  {
    this.dataService.getLastItem().subscribe(device => this.interiorDevice = device,
      error =>  this.errorMessage = <any>error);

  }



}

