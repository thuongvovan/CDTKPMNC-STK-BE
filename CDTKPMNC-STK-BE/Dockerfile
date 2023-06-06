#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
VOLUME ["/app/Upload"]
VOLUME ["/app/wwwroot"]
ENV ASPNETCORE_ENVIRONMENT="Testing"
ENV UPLOAD_REQUEST_PATH="/Upload"
ENV UPLOAD_DIRECTORY="/app/Upload"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# COPY ["CDTKPMNC-STK-BE/CDTKPMNC-STK-BE.csproj", "CDTKPMNC-STK-BE/"]
COPY ["CDTKPMNC-STK-BE.csproj", "CDTKPMNC-STK-BE/"]
RUN dotnet restore "CDTKPMNC-STK-BE/CDTKPMNC-STK-BE.csproj"
WORKDIR "/src/CDTKPMNC-STK-BE"
COPY . .
# WORKDIR "/src/CDTKPMNC-STK-BE"
RUN dotnet build "CDTKPMNC-STK-BE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CDTKPMNC-STK-BE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CDTKPMNC-STK-BE.dll"]