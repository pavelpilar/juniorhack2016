/**
 * Created by lukasfrajt on 02/12/2016.
 */

import {Component, OnInit} from "@angular/core";
import {Device} from "./model/Device";
import {DataService} from "./app.service";
import './rxjs-operators';
import {forEach} from "@angular/router/src/utils/collection";


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
  public devices: Device[];
  public interier: Device[];
  //public exterior: Device = {id_senzoru: "temp", teplota: "28", vlhkost: "25", datum: new Date()};

  errorMessage: string;



  constructor(private dataService: DataService){
  }

  ngOnInit():void {
    this.getData();
    this.getLastDeviceInterior()
  }
  getData()
  {
    this.dataService.getDevices().subscribe( devices => this.devices = devices,
      error =>  this.errorMessage = <any>error);
  }
  getLastDeviceInterior()
  {
    this.dataService.getLastItem().subscribe( device => this.interier = device,
      error =>  this.errorMessage = <any>error);
  }



}

