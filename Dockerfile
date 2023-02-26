#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MovieApplication.csproj", "."]
RUN dotnet restore "./MovieApplication.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MovieApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieApplication.dll"]

# Install SQL Server on the image
# Accept the license agreement, set the SA password, and enable TCP/IP
RUN apt-get update && \
    apt-get install -y curl && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2019.list > /etc/apt/sources.list.d/mssql-server.list && \
    apt-get update && \
    apt-get install -y mssql-server && \
    ACCEPT_EULA=Y MSSQL_SA_PASSWORD=YourStrong!Passw0rd /opt/mssql/bin/sqlservr-setup --accept-eula --set-sa-password && \
    /opt/mssql/bin/mssql-conf set tcpport 1433 && \
    /opt/mssql/bin/mssql-conf set sqlagent.enabled true && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Set the connection string to the database
ENV ConnectionStrings__DefaultConnection "Server=localhost,1433;Database=MovieDB;User Id=sa;Password=YourStrong!Passw0rd;"

# Expose port 80 for the web application and port 1433 for SQL Server
EXPOSE 80
EXPOSE 1433

# Run the SQL Server service and the application
CMD /opt/mssql/bin/sqlservr & dotnet YourAppName.dll
