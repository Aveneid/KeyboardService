/**
Based on CustomKeypad library shetch.
**/
#include <Keypad.h>
const byte ROWS = 2; 
const byte COLS = 4; 
char hexaKeys[ROWS][COLS] = {
  {'0','1','2','3'},
  {'4','5','6','7'},
};
byte rowPins[ROWS] = {3,4}; 
byte colPins[COLS] = {5,6,7,8}; 
Keypad customKeypad = Keypad( makeKeymap(hexaKeys), rowPins, colPins, ROWS, COLS); 
void setup(){
  Serial.begin(9600);
}
void loop(){
  char customKey = customKeypad.getKey();
  if (customKey){
    Serial.println(customKey);
  }
}
