#include <UTFT.h>

extern uint8_t SmallFont[];

int lastY;
int temp;

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

 /*
  * 
  * 
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
}

void loop()
{  
  temp = (analogRead(A0) * (5000/1024) - 500) / 10 + /* calibration constant */ 2; 
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
  

  delay(200);
}



