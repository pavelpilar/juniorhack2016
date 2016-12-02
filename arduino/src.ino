#include <UTFT.h>

extern uint8_t SmallFont[];

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
void setup()
{  
  // LCD Setup
  myGLCD.InitLCD(PORTRAIT);
  myGLCD.setFont(SmallFont);
  myGLCD.clrScr();
}

void loop()
{  
  temp = (analogRead(A0) * (5000/1024) - 500) / 10 + /* kalibrační konstanta */ 4; 
  
  myGLCD.setBackColor(0,0,0);
  myGLCD.print("Temp: ", 1, 1);
  myGLCD.print(String(temp), 40, 1);

  for(int i = 0; i < 6; i++) {
    int y = 10 + (138/5) * i;
    myGLCD.print(String((5-i)*10), 65, y - 5);
    myGLCD.drawLine(80, y, 85, y);
  }

  myGLCD.drawRect(90, 10, 110, 148);
  myGLCD.fillRect(91, 147-(148/50*temp), 109, 147);

  delay(100);
}



