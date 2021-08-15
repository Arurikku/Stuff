#include "WiFi.h"
#include "ESPAsyncWebServer.h"
#include "SPIFFS.h"
#include <Adafruit_NeoPixel.h>

const char* ssid = "Epic";
const char* password = "Chickennugget0511$";
AsyncWebServer server(80);
Adafruit_NeoPixel pixels(16, 17, NEO_GRB + NEO_KHZ800);

void setup() {
  Serial.begin(115200);
  pixels.begin();
  pixels.clear();
  Serial.println();
  if (!SPIFFS.begin(true)) {
    Serial.println("An Error has occurred while mounting SPIFFS");
    return;
  }
  Serial.println("Setting up access point…");
  WiFi.softAP(ssid, password);
  IPAddress IP = WiFi.softAPIP();
  Serial.print("IP address: ");
  Serial.println(IP);
  server.on("/", HTTP_GET, [](AsyncWebServerRequest * request) {
    mainPage(request);
  });
  server.on("/style.css", HTTP_GET, [](AsyncWebServerRequest * request) {
    request->send(SPIFFS, "/style.css", "text/css");
  });
  server.on("/code.js", HTTP_GET, [](AsyncWebServerRequest * request) {
    request->send(SPIFFS, "/code.js", "text/javascript");
  });
  server.on("/toggle", HTTP_GET, [](AsyncWebServerRequest * request) {
    mainPage(request);
  });
  server.on("/sendData", HTTP_POST, [](AsyncWebServerRequest * request) {}, NULL, [](AsyncWebServerRequest * request, uint8_t *data, size_t len, size_t index, size_t total) {
    String msg = "";
    for (size_t i = 0; i < len; i++) {
      msg = msg + (char)data[i];
    }
    String sa[17];
    int r = 0, t = 0;
    Serial.println(msg);
    for (int i = 0; i < msg.length(); i++)
    {
      if (msg.charAt(i) == ';')
      {
        sa[t] = msg.substring(r, i);
        r = (i + 1);
        t++;
      }
    }
    for (int i = 0; i < 17; i++)
    {
      sa[i] = sa[i] + ",";
    }
    for (int i = 0; i < 17; i++)
    {
      r = 0;
      t = 0;
      int nums[4];
      for (int ii = 0; ii < sa[i].length(); ii++)
      {
        if (sa[i].charAt(ii) == ',')
        {
          nums[t] = sa[i].substring(r, ii).toInt();
          r = (ii + 1);
          t++;
        }
      }
      pixels.setPixelColor(i, pixels.Color(nums[0], nums[1], nums[2]));
      pixels.setBrightness(nums[3]);-
    }
     pixels.show();
    request->send(200);

  });
  server.onNotFound(notFound);
  server.begin();
}

void loop() {

}

void mainPage(AsyncWebServerRequest* request)
{
  request->send(SPIFFS, "/index.html", String(), false);
}
void notFound(AsyncWebServerRequest* request)
{
  request->send(200, "text/plain", "ERROR 404 - Not Found");
}
