# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY mas.sln .
COPY mas/mas.csproj mas/
COPY mas.Client/mas.Client.csproj mas.Client/
COPY DTOs/*.cs DTOs/
COPY Models/ Models/
COPY Repositories/ Repositories/
COPY Services/ Services/
COPY Mappings/ Mappings/

# Restore dependencies
RUN dotnet restore mas/mas.csproj

# Copy everything else
COPY . .

# Build and publish
WORKDIR /src/mas
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "mas.dll"]
