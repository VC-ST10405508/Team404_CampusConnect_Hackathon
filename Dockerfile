# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Team404_CampusConnect_Hackathon.csproj"
RUN dotnet publish "./Team404_CampusConnect_Hackathon.csproj" -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Team404_CampusConnect_Hackathon.dll"]
