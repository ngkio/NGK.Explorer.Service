FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY ./Explorer.Service.WebApi/publish/. .
ENTRYPOINT ["dotnet", "Explorer.Service.WebApi.dll"]

