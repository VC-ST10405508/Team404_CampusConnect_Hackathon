# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project folder
COPY Team404_CampusConnect_Hackathon/ ./Team404_CampusConnect_Hackathon/

# Restore & publish using the correct path
RUN dotnet restore "./Team404_CampusConnect_Hackathon/Team404_CampusConnect_Hackathon.csproj"
RUN dotnet publish "./Team404_CampusConnect_Hackathon/Team404_CampusConnect_Hackathon.csproj" -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Team404_CampusConnect_Hackathon.dll"]
