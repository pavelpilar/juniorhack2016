#include <UTFT.h>
#include <Servo.h>

extern uint8_t SmallFont[];

int lastY;
int lastLight;

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
  * Servo-D9
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
  
  Serial.begin(9600);
}

void loop()
{  
  //Temperature
  int temp = (analogRead(A0) * (5000/1024) - 500) / 10 + /* calibration constant */ 2; 
  int newY = 148-(138/50.0*temp);

  myGLCD.print("Temp: ", 1, 1);
  myGLCD.print(String(temp), 40, 1);
  
  if(newY < lastY)
  {
    myGLCD.setColor(0,0,0);
    myGLCD.fillRect(91, lastY-15, 109, newY);
    myGLCD.setColor(255, 255, 255);
  } else {
    myGLCD.drawRect(90, 10, 110, 148);  
    myGLCD.fillRect(91, newY, 109, 147);
  }
  lastY = newY;

  //Light
  int light = analogRead(A1);

  if(light < 600)
    myGLCD.print("Night", 1, 15);
  else {
    if(lastLight < 600) {
      myGLCD.setColor(0,0,0);
      myGLCD.fillRect(1, 15, 40, 40);
      myGLCD.setColor(255,255,255);
    }
    myGLCD.print("Day", 1, 15);
  }
  lastLight = light;

  if(Serial.available()) {
    byte[] bytes = new byte[2];
    Serial.readBytes(bytes, 2);
    switch(bytes[0] {
      
    }
  }
    

  delay(200);
}



