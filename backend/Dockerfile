FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

WORKDIR /src
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build -o /app -r linux-x64 /p:PublishReadyToRun=true
RUN dotnet publish -o /publish -r linux-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
COPY --from=build  /publish /app
WORKDIR /app
EXPOSE 5011
# HEALTHCHECK CMD curl --fail http://localhost || exit 1  

CMD ["./GptBoxApi"]