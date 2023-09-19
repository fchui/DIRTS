#define LDRpin A0
int ledPins[] = {9, 10, 11};
 //change accordingly
int LDR_value=0;
int index;
void setup()
{
  Serial.begin(9600);
  for (index = 0; index <3; index++)
  {
    pinMode(ledPins[index], OUTPUT);
  }

  pinMode(LDRpin, INPUT);
}
void loop()
{
  int value = 0;
  int delayTime = 10;
  digitalWrite(ledPins[0], HIGH);  // turn LED on
                       // pause to slow down
    value = analogRead(A0);
    LDR_value = LDR_value + value;
    Serial.println(LDR_value);
    delay(delayTime); 
    digitalWrite(ledPins[0], LOW);
  //oneAtaTime();
}
