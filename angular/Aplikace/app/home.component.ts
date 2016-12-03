/**
 * Created by lukasfrajt on 02/12/2016.
 */

import {Component, OnInit} from "@angular/core";
import {Device} from "./model/Device";
import {DataService} from "./app.service";
import './rxjs-operators';
import {forEach} from "@angular/router/src/utils/collection";

import {Observable} from 'rxjs/Rx';
import {Settings} from "./model/Settings";


@Component({
  moduleId: module.id,
  selector: 'home-component',

  templateUrl: 'templates/home.component.html',
  styleUrls: ['styles/home.component.scss']

})
export class HomeComponent implements OnInit{

  private boxOne: string ="Interior";
  private boxTwo: string ="Exterior";
  private boxThree: string ="Settings";
  private boxFour: string = "Settings";

  private exteriorDevice: Device[];
  private devices: Device[];
  private interior: Device[];
  private settings: Settings[];


  errorMessage: string;




  constructor(private dataService: DataService){

  }

  ngOnInit():void {
    this.getData();
    this.get();
    this.getOutDevice();
    this.getSetings();




  }
  autoloader():void {
    function myFunction() {
      setInterval(function () {
        alert("Hello");
      }, 3000);
    }
  }

  getData()
  {
    this.dataService.getDevices().subscribe( devices => this.devices = devices,
      error =>  this.errorMessage = <any>error);
  }
  getLastDeviceInterior()
  {
    this.dataService.getLastItem().subscribe( device => this.interior = device,
      error =>  this.errorMessage = <any>error);
  }
  getOutDevice(){
    this.dataService.getOutDevices().subscribe( devices => this.exteriorDevice = devices,
      error =>  this.errorMessage = <any>error);

  }
  getTest(){

  }
  getSetings() {
    this.dataService.getOutSettings().subscribe(settings => this.settings = settings,
      error => this.errorMessage = <any>error);
  }

    get()
    {
      setInterval(() => {
        this.dataService.getLastItem().subscribe( device => this.interior = device,
          error =>  this.errorMessage = <any>error);
      }, 100);
    }






}

