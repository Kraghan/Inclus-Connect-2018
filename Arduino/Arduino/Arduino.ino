// CONSTANTS
const int LUM = A1;
const int MIC = A3;
const int BTN = 4;
const int PIEZO = A2;
const int CALIB_LUM = 25; //light calibration constant
const int CALIB_PIEZO = 10; //piezoelectric calibration constant
// VARIABLES
int valLum = 0;
int valLumPrev = 0;
int valMic = 0;
int valBtn = 0;
int valPiezo = 0;
bool stateLum = 1;
bool stateMic = 0;
bool statePiezo = 0;
String state = "";

// SETUP
void setup() {
  pinMode(LUM, INPUT);
  pinMode(MIC, INPUT);
  pinMode(BTN, INPUT);
  pinMode(PIEZO, INPUT);
  Serial.begin(9600);
}

// PROGRAM
/*Return the light state: on / off*/
bool checkLum(int pin){
  valLumPrev = valLum;
  valLum = analogRead(pin);
  //Serial.print("LUM : ");
  //Serial.println(analogRead(LUM));
  if(valLum <= valLumPrev - CALIB_LUM){stateLum = 0;}
  if(valLum >= valLumPrev + CALIB_LUM){stateLum = 1;}
  //Serial.println(stateLum);
  return stateLum;
}

/*Return the button state: on / off*/
bool checkBtn(int pin){
  valBtn = digitalRead(pin);
  //Serial.print("BTN : ");
  //Serial.println(digitalRead(BTN));
  return valBtn;
}

/*Return the microphone state: on / off*/
bool checkMic(int pin){
  valMic = analogRead(pin);
  //Serial.print("MIC : ");
  //Serial.println(analogRead(MIC));
  if(valMic > 500){stateMic = 1;}
  else{stateMic = 0;}
  return stateMic;
}

/*Return the piezoelectric state: on / off*/
bool checkPiezo(int pin){
  valPiezo = analogRead(pin);
  //Serial.print("PIEZO : ");
  //Serial.println(analogRead(PIEZO));
  if(valPiezo > CALIB_PIEZO){statePiezo = 1;}
  else{statePiezo = 0;}
  return statePiezo;
}

void loop() {
  // LUM;BTN;MIC;PIEZO
  state = "";
  state += checkLum(LUM);
  state += ";";
  state += checkBtn(BTN);
  state += ";";
  state += checkMic(MIC);
  state += ";";
  state += checkPiezo(PIEZO);
  Serial.flush();
  Serial.println(state);
}
