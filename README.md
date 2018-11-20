# MyWeatherApp
Console application to get the current weather or weather forecast (up to 5 days ahead) for any location.

This app uses api.openweathermap.org for weather data. Please get your own APPID if use this code (it's free).

Free APPID being used here allows to send queries to the API once per 10 mnutes only. So responses are being cashed:
- app uses the cash only for the current date, the older cash automatically being deleted;
- for current weather queries app uses the cash only if you cannot get the actual info because the limit of queries is exceeded;
- for forecast queries app used the cash during the day when the forecast was being gotten.

There are the following comand line arguments available:
--help - shows the manual;
--location - sets the city. Example: --location London. Without this parameter the app will use default city if set;
-d - sets number of days ahead. Example: -d 1. -d value can be from 0 (today) to 5 (five days ahead). Negative numbers will be replaced by 0. If not specified, the app wil show the current weather for today.
-f - sets chosen city as default. Example: --location London -f.
All the parameters are optional, except for --location in case when the default city is not set.
