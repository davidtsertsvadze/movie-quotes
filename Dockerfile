# 1. Build ეტაპი .NET SDK-ის გამოყენებით
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# პროექტის ფაილების კოპირება და აღდგენა (Restore)
COPY *.sln .
COPY *.csproj .
RUN dotnet restore

# დანარჩენი კოდის კოპირება და Publish
COPY . .
RUN dotnet publish -c Release -o /app/out

# 2. Runtime ეტაპი უფრო მსუბუქი იმიჯით
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Render იყენებს პორტ 8080-ს
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# პროექტის გაშვება
ENTRYPOINT ["dotnet", "MovieQuotesAPI.dll"]