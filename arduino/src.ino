#include <UTFT.h>
#include <Servo.h>

extern uint8_t SmallFont[];

int lastY;
int lastLight;
unsigned long sendTimer;
int windowState = 0;

UTFT myGLCD(ITDB18SP,6,7,3,4,5);    //TFT display
/*
 * LED-5V
 * SCK-D7 
 * SDA-D6 
 * AO-D5 
 * RST-D4 
 * CS-D3
 * 
 */

Servo servo;
 /*
  * Light-A1
  * Heat-D8
  * Servo-D9     
  * Ventilation-D10
  * Window Button-D13
  */
void setup()
{  
  // LCD Setup
  myGLCD.InitLCD(PORTRAIT);
  myGLCD.setFont(SmallFont);
  myGLCD.clrScr();

  //Temperature bar
  for(int i = 0; i < 6; i++) {
    int y = 10 + (138/5) * i;
    myGLCD.print(String((5-i)*10), 65, y - 5);
    myGLCD.drawLine(80, y, 85, y);
  }

  //Servo (window) setup
  servo.attach(9);
  servo.write(80);
  
  Serial.begin(115200);
  Serial.setTimeout(1500);
  sendTimer = millis();
  pinMode(2, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(10, OUTPUT);
}

void loop()
{  
  if(digitalRead(13) == LOW){
    if(windowState == 0){
      servo.write(40);
      windowState = 1;
    } else {
      servo.write(80);
      windowState = 0;
    }
  }
    
  //Temperature
  int temp = (analogRead(A0) * (5000/1024) - 500) / 10 + /* calibration constant */ 2; 
  int newY = 148-(138/50.0*temp);

  myGLCD.print("Temp: ", 1, 1);
  myGLCD.print(String(temp), 40, 1);
  
  if(newY < lastY)
  {
    myGLCD.setColor(0,0,0);
    myGLCD.fillRect(91, lastY-70, 109, newY);
    myGLCD.setColor(255, 255, 255);
  } else {
    myGLCD.drawRect(90, 10, 110, 148);  
    myGLCD.fillRect(91, newY, 109, 147);
  }
  lastY = newY;

  //Light
  int light = analogRead(A1);

  if(light < 600 && lastLight >= 600)
    myGLCD.print("Night", 1, 15);
  else if(light >= 600 && lastLight < 600) {
      myGLCD.setColor(0,0,0);
      myGLCD.fillRect(1, 15, 40, 40);
      myGLCD.setColor(255,255,255);
      myGLCD.print("Day", 1, 15); 
  }   
  lastLight = light;

  if(Serial.available()) {
    byte bytes[2];
    Serial.readBytes(bytes, 2);
    
    bytes[0] = bytes[0]-0x30;
    bytes[1] = bytes[1]-0x30;
    
    switch(bytes[0]) {
      case 0: //Window
        if(bytes[1] == 1) {
          servo.write(40);
          windowState = 1;
        } else {
          servo.write(80);
          windowState = 0;
        }
        break;
      case 1: //Ventilation
        if(bytes[1] == 1)
          digitalWrite(10, HIGH);
        else
          digitalWrite(10, LOW);
        break;
      case 2: //Heating
        if(bytes[1] == 1)
          digitalWrite(8, HIGH);
        else
          digitalWrite(8, LOW);
        break;        
    }
  } 

  if(millis() - sendTimer >= 4000) {
    sendTimer = millis();
    Serial.write(temp);
    Serial.write(light < 600 ? 0 : 1);
  }

  if(windowState == 1)
    digitalWrite(2, HIGH);
  else
    digitalWrite(2, LOW);
    
  delay(200);

}



