/**
 * Created by lukasfrajt on 02/12/2016.
 */


import {Injectable} from "@angular/core";
import {Http, Response} from "@angular/http";
import { Observable }     from 'rxjs/Observable';
import {Device} from "./model/Device";
import {Settings} from "./model/Settings";
import {Security} from "./model/Security";
@Injectable()

export class DataService {

  private deviceUrl = 'http://tymc15.jecool.net/www/api/vyber?key=t4m15&pocet=30';  // URL to web API

  constructor (private http: Http) {}

  getDevices (): Observable<Device[]> {
    return this.http.get(this.deviceUrl)
      .map(this.extractData)
      .catch(this.handleError);
  }
  getLastItem(): Observable<Device[]> {
    this.deviceUrl ='http://tymc15.jecool.net/www/api/vyber?key=t4m15&pocet=1';
    return this.http.get(this.deviceUrl)
      .map(this.extractData)
      .catch(this.handleError);

  }
  getOutDevices(): Observable<Device[]> {
    this.deviceUrl ='http://tymc15.jecool.net/www/api/venkovni?key=t4m15';
    return this.http.get(this.deviceUrl)
      .map(this.extractData)
      .catch(this.handleError);
  }

  getOutSettings(): Observable<Settings[]> {
    this.deviceUrl ='http://tymc15.jecool.net/www/api/nastaveni?key=t4m15&volba=1&nid=1';
    return this.http.get(this.deviceUrl)
      .map(this.extractData)
      .catch(this.handleError);

  }

  getSecurity(): Observable<Security[]>{
    this.deviceUrl ='http://tymc15.jecool.net/www/api/magnet?sec=2';
    return this.http.get(this.deviceUrl)
      .map(this.extractData)
      .catch(this.handleError);
  }

  private extractData(res: Response) {
    let body = res.json();
    return body;
  }

  private handleError (error: Response | any) {
    // In a real world app, we might use a remote logging infrastructure
    let errMsg: string;
    if (error instanceof Response) {
      const body = error.json() || '';
      const err = body.error || JSON.stringify(body);
      errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    return Observable.throw(errMsg);
  }
}


