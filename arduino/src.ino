#include <UTFT.h>

extern uint8_t SmallFont[];

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
  myGLCD.setBackColor(0,0,0);
  
}

void loop()
{  
  myGLCD.print("Test", 0, 0);
}



