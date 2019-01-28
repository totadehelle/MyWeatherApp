FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY MyWeatherApp/*.csproj ./MyWeatherApp/
WORKDIR /app/MyWeatherApp
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY MyWeatherApp/. ./MyWeatherApp/
WORKDIR /app/MyWeatherApp
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-runtime AS runtime
WORKDIR /app
COPY --from=build /app/MyWeatherApp/out ./
ENTRYPOINT ["dotnet", "MyWeatherApp.dll", "--location", "Minsk"]
