FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build-base
RUN apt-get update -y && apt install openjdk-21-jre -y
RUN dotnet tool install --global dotnet-sonarscanner

FROM build-base AS build
COPY ["CT.Examples.SonarqubeDocker.Api/CT.Examples.SonarqubeDocker.Api.csproj", "CT.Examples.SonarqubeDocker.Api/"]
COPY ["CT.Examples.SonarqubeDocker.Tests/CT.Examples.SonarqubeDocker.Tests.csproj", "CT.Examples.SonarqubeDocker.Tests/"]
COPY CT.Examples.SonarqubeDocker.sln .

RUN dotnet restore

COPY . .

ENV PATH="${PATH}:/root/.dotnet/tools"
ENV LANG=C.UTF-8 LC_ALL=C.UTF-8
ARG sonarqubecmd
RUN ${sonarqubecmd}
RUN dotnet build "CT.Examples.SonarqubeDocker.sln" -c Release --no-restore

RUN dotnet test --no-build -c Release --collect "XPlat Code Coverage;Format=cobertura,opencover" --logger "trx;LogFileName=test_results.trx" --results-directory /testresults /p:CoverletOutput=/testresults/coverage/ CT.Examples.SonarqubeDocker.Tests/CT.Examples.SonarqubeDocker.Tests.csproj;exit 0
ARG SonarqubeToken
RUN dotnet sonarscanner end /d:sonar.token=${SonarqubeToken}

FROM build AS publish
RUN dotnet publish "CT.Examples.SonarqubeDocker.Api/CT.Examples.SonarqubeDocker.Api.csproj" -c Release -o /app/publish --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS runtime
WORKDIR /app
COPY --from=publish /app/publish ./
COPY --from=build /testresults ./testresults
EXPOSE 80
ENTRYPOINT ["dotnet", "CT.Examples.SonarqubeDocker.Api/CT.Examples.SonarqubeDocker.Api.dll"]