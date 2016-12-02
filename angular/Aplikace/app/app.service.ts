/**
 * Created by lukasfrajt on 02/12/2016.
 */


import {Injectable} from "@angular/core";
import {Http, Response} from "@angular/http";
import {Observable} from "rxjs";
import {Device} from "./model/Device";
@Injectable()

export class DataService {

  private deviceUrl = 'http://tymc15.jecool.net/www/api/?key=t4m15';  // URL to web API

  constructor (private http: Http) {}

  getDevices (): Observable<Device[]> {
    return this.http.get(this.deviceUrl)
      .map(this.extractData)
      .catch(this.handleError);
  }

  private extractData(res: Response) {
    let body = res.json();
    return body.data || { };
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
    console.error(errMsg);
    return Observable.throw(errMsg);
  }
}


