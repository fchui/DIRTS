 #include <SoftwareSerial.h>

SoftwareSerial BTserial(10, 11); // RX | TX
int sensorPin1 = A0;
int sensorPin2 = A1;

int sensorValue = 0;
int sensorValue1 = 0;

void setup() {
  // put your setup code here, to run once:
  BTserial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  sensorValue = analogRead(sensorPin1);
  sensorValue1 = analogRead(sensorPin2);
  BTserial.print(sensorValue);
  BTserial.print(",");
  BTserial.print(sensorValue);
  BTserial.print(";");
  delay(100);
}
