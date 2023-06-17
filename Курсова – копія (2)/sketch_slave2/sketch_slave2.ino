#include <Wire.h>

const byte SLAVE_ADDRESS = 25;
const byte MESSAGE_LENGTH = 3;  // Adjusted length for sensor data only
const byte MOTOR_PIN = 8;
#define TABLE_SIZE 256

// Lookup table for CRC calculation
uint8_t crc8Table[TABLE_SIZE];
byte currentTemperature = 0;

// Timer interrupt service routine
ISR(TIMER1_COMPA_vect) {
  if (currentTemperature >= 30) {
    digitalWrite(MOTOR_PIN, HIGH);
    delay(500);  // Adjust the delay as needed for desired motor speed
    digitalWrite(MOTOR_PIN, LOW);
    delay(500);  // Adjust the delay as needed for desired motor speed
  }
}

void generateCRCTable() {
  uint8_t poly = 0x2F;
  for (int i = 0; i < TABLE_SIZE; ++i) {
    uint8_t crc = i;
    for (int j = 0; j < 8; ++j) {
      if (crc & 0x80) {
        crc = (crc << 1) ^ poly;
      } else {
        crc <<= 1;
      }
    }
    crc8Table[i] = crc;
  }
}

void setWriteModeRS485() {
  PORTD |= 1 << PD2;
  delay(1);
}

// Interrupt on transmission complete
ISR(USART_TX_vect) {
  PORTD &= ~(1 << PD2);  // Set to receive mode
}

int writeDataToMaster(byte sensorData) {
  unsigned short checkSumCRC = calculate_crc8(sensorData);
  byte message[MESSAGE_LENGTH] = { SLAVE_ADDRESS, sensorData, checkSumCRC };

  // Send the message including the sensor data
  for (int i = 0; i < MESSAGE_LENGTH; i++) {
    byte byteToSend = message[i];
    Serial.write(byteToSend);
  }
}

void setup() {
  // Set up Timer/Counter 1
  cli();  // Disable interrupts
  // Set the prescaler to 64, resulting in 250 kHz clock (16 MHz / 64)
  TCCR1B |= (1 << CS11) | (1 << CS10);
  // Set the compare match value for a 1 ms interval
  OCR1A = 250;
  // Enable the compare match interrupt
  TIMSK1 |= (1 << OCIE1A);
  // Enable global interrupts
  sei();

  Wire.begin();  // Initialize I2C communication
  generateCRCTable();
  delay(200);
  // En_slave - Set to output + low level
  DDRD = 0b00000110;
  PORTD = 0b11111001;

  // Initialize UART0
  Serial.begin(14400, SERIAL_8N1);
  UCSR0B |= (1 << UCSZ02) | (1 << TXCIE0);
  UCSR0A |= (1 << MPCM0);  // Multiprocessor mode

  pinMode(MOTOR_PIN, OUTPUT);

  delay(10);
}

void loop() {
  if (Serial.available()) {
    byte inByte = Serial.read();
    if (SLAVE_ADDRESS == inByte) {
      UCSR0A &= ~(1 << MPCM0);
      setWriteModeRS485();

      // Read data from the TCN75A sensor
      byte sensorData = readSensorData();

      writeDataToMaster(sensorData);
      delay(200);
    }
  }
}

uint8_t calculate_crc8(const uint8_t data) {
  uint8_t crc = 0xFF;
  crc = crc8Table[crc ^ data];
  return crc;
}

byte* readSensorData() {
  Wire.beginTransmission(0x48);
  Wire.write(0x00);
  Wire.endTransmission();
  Wire.requestFrom(0x48, 1);
  byte sensorData = Wire.read();
  currentTemperature = sensorData;
  return sensorData;
}
