static int pinPotentiometre = A0;
static int buttonPin = 2;
static int distanceListenPin = 3;
static int distanceTriggerPin = 4;
static int led1Pin = 9;
static int led2Pin = 10;
static int led3Pin = 11;

static int led1Value = 0;
static int led2Value = 0;
static int led3Value = 0;

static float led1Divisor = 1;
static float led2Divisor = 1;
static float led3Divisor = 1;

static bool button_on = false;

static int oldDistance = 0;
static int echoState = 0;
static bool echoPending = false;
static unsigned long echoTime = 0;
static int echoMillisBetween = 10;
static int echoEventBetween = 1000;
static unsigned long echoLastEventTime = 0;
static bool handDetected = false;

static unsigned long potentiometreTime = 0;
static int potentiometreUpdate = 100;

static unsigned long temperatureTime = 0;
static int temperatureUpdate = 100;

void setup() {

  pinMode(pinPotentiometre,INPUT);
  
  pinMode(led1Pin, OUTPUT); //alimentation de la diode 1
  pinMode(led2Pin, OUTPUT); //alimentation de la diode 2
  pinMode(led3Pin, OUTPUT); //alimentation de la diode 3
  
  pinMode(LED_BUILTIN, OUTPUT);
  // Internal pullup, no external resistor necessary
  pinMode(buttonPin,INPUT_PULLUP);
  // 115200 is a common baudrate : fast without being overwhelming
  Serial.begin(115200);

  pinMode(distanceListenPin, INPUT); // sortie du capteur distance
  pinMode(distanceTriggerPin, OUTPUT); //trigger du capteur distance

  // As the button is in pullup, detect a connection to ground
  attachInterrupt(digitalPinToInterrupt(buttonPin), process_button, FALLING);
  attachInterrupt(digitalPinToInterrupt(distanceListenPin), listen_echo, RISING);
}

void loop() {
  analogWrite(led1Pin, led1Value);
  analogWrite(led2Pin, led2Value);
  analogWrite(led3Pin, led3Value);

  process_potentiometre();
  
  process_distance();

  process_temperature();
}

// Process LED Value
void process_led(int n_led, float p){
  switch (n_led) {
    case 1:
      led1Value = (255 * p) / led1Divisor;
      break;
    case 2:
      led2Value = (255 * p) / led2Divisor;
      break;
    case 3:
      led3Value = (255 * p) / led3Divisor;
      break;
  }
}

void process_temperature(){
  if(millis() - temperatureTime >= temperatureUpdate)
  {
    int v = analogRead(A1);
    float temp = v * (5.0/1024) / 0.01;
    temp = temp - 273.15;
    Serial.print("tp:");
    Serial.println(temp);
    temperatureTime = millis();
  }
}

// Process button input
void process_button() {
  button_on = !button_on;
  if (button_on) {
    Serial.println("bt:on");
  } else {
    Serial.println("bt:off");
  }
}

// Processes distance sensor
void process_distance() {
  switch (echoState) {
    case 0 :
      digitalWrite(distanceTriggerPin, HIGH);
      echoTime = millis();
      ++echoState;
      break;
    case 1 :
      if(millis() - echoTime >= 10){
        digitalWrite(distanceTriggerPin, LOW);
        echoPending = true;
        ++echoState;
      }
      break;
    case 2 :
      if(!echoPending){
        echoTime = micros();
        ++echoState;
      }
      break;
    case 3 :
      if(digitalRead(3) == 0){
        int cm = ((micros() - echoTime)/2) / 29.1;  

        if(cm >= 0 && cm <= 50){
          //Serial.print("d:");
          //Serial.println(cm);
          if(handDetected && millis() - echoLastEventTime >= echoEventBetween){
            int delta = oldDistance - cm;
            if(delta >= 5){
              Serial.println("sw:l");
              echoLastEventTime = millis();
            }else if(delta <= -5){
              Serial.println("sw:r");
              echoLastEventTime = millis();
            }
          }
          oldDistance = cm;

          handDetected = true;
        }else{
          handDetected = false;
        }

        echoTime = millis();
        ++echoState;
      }
      break;
    case 4 :
      if(millis() - echoTime >= echoMillisBetween){
        echoState = 0;
      }
      break;
  }
}

void listen_echo () {
  echoPending = false;
}

// Handles incoming messages
// Called by Arduino if any serial data has been received
void serialEvent()
{
  String message = Serial.readStringUntil('\n');
  String head = message.substring(0, 1);
  String value = message.substring(2, message.length() - 1);
  if (head == "L") {
    if (value == "ON"){
      digitalWrite(13,HIGH);
    }else if (value == "OFF"){
      digitalWrite(13,LOW);
    }
  } else if (head == "V") {
    int n = value.substring(0, 1).toInt();
    float p = value.substring(2).toFloat();
    process_led(n, p);
  }
}

void process_potentiometre()
{
  if(millis() - potentiometreTime >= potentiometreUpdate)
  {
    int result = analogRead(pinPotentiometre);
    Serial.print("po:");
    Serial.println(result);
    potentiometreTime = millis();
  }
}
