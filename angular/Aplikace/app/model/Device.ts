/**
 * Created by lukasfrajt on 02/12/2016.
 */
export class Device {

  constructor(private id:string, private tmp:string, private wetness: string, private lightness: string){}

  public getId() {
    return this.id;
  }

  public getTemperature(){
    return this.tmp;
  }

  public getWetness()
  {
    return this.wetness;
  }

  public getLightness(){
    return this.lightness;
  }



}
