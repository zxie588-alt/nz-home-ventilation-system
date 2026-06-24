/*
  Smart NZ Ventilation System - ESP32 commissioning logger skeleton

  Purpose:
  - Read CO2, temperature, RH and optional duct pressure.
  - Print a CSV stream that can be imported into the Streamlit dashboard.
  - Keep the code simple enough to adapt to available sensors.

  Suggested sensors:
  - Sensirion SCD40/SCD41 for CO2, temperature and RH over I2C.
  - Optional differential pressure sensor for duct/static pressure.

  This file is a portfolio/test-rig starting point. Confirm library names,
  wiring and calibration offsets for the actual hardware used.
*/

#include <Arduino.h>
#include <Wire.h>

const int FAN_PWM_PIN = 25;
const int NORMAL_PWM = 130;
const int BOOST_PWM = 220;

const int CO2_BOOST_THRESHOLD_PPM = 1000;
const int CO2_RETURN_THRESHOLD_PPM = 850;
const int RH_BOOST_THRESHOLD_PERCENT = 65;
const int RH_RETURN_THRESHOLD_PERCENT = 60;

String mode = "normal";

struct SensorFrame {
  uint32_t timestamp_ms;
  int co2_ppm;
  float temp_C;
  float rh_percent;
  float duct_dp_Pa;
};

SensorFrame readSensors() {
  SensorFrame frame;
  frame.timestamp_ms = millis();

  // Replace these placeholder values with real sensor library calls.
  // Keep the output format stable so the dashboard can read the CSV.
  frame.co2_ppm = 780;
  frame.temp_C = 20.0;
  frame.rh_percent = 56.0;
  frame.duct_dp_Pa = 18.0;

  return frame;
}

void updateMode(const SensorFrame &frame) {
  if (frame.co2_ppm >= CO2_BOOST_THRESHOLD_PPM || frame.rh_percent >= RH_BOOST_THRESHOLD_PERCENT) {
    mode = "boost";
  } else if (frame.co2_ppm <= CO2_RETURN_THRESHOLD_PPM && frame.rh_percent <= RH_RETURN_THRESHOLD_PERCENT) {
    mode = "normal";
  }

  int pwm = mode == "boost" ? BOOST_PWM : NORMAL_PWM;
  analogWrite(FAN_PWM_PIN, pwm);
}

void setup() {
  Serial.begin(115200);
  Wire.begin();
  pinMode(FAN_PWM_PIN, OUTPUT);
  analogWrite(FAN_PWM_PIN, NORMAL_PWM);

  Serial.println("timestamp_ms,mode,co2_ppm,temp_C,rh_percent,duct_dp_Pa,fan_pwm");
}

void loop() {
  SensorFrame frame = readSensors();
  updateMode(frame);
  int pwm = mode == "boost" ? BOOST_PWM : NORMAL_PWM;

  Serial.print(frame.timestamp_ms);
  Serial.print(",");
  Serial.print(mode);
  Serial.print(",");
  Serial.print(frame.co2_ppm);
  Serial.print(",");
  Serial.print(frame.temp_C, 1);
  Serial.print(",");
  Serial.print(frame.rh_percent, 1);
  Serial.print(",");
  Serial.print(frame.duct_dp_Pa, 1);
  Serial.print(",");
  Serial.println(pwm);

  delay(15000);
}
