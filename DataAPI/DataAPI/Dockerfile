# Use the official Microsoft .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the source code and build the app
COPY . .
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose the port the app listens on (adjust if needed)
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "DataAPI.dll"]