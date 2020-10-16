#!/usr/bin/env bash

folder="/root/.nuget/NuGet/"
file="/root/.nuget/NuGet/NuGet.Config"

# -x 参数判断 $folder 是否存在
if [[ ! -d "$folder" ]]; then
  mkdir -p "$folder"
fi

# -f 参数判断 $file 是否存在
if [[ ! -f "$file" ]]; then
  cp NuGet.Config ${file}
fi

dotnet restore ./Explorer.Service.WebApi/Explorer.Service.WebApi.csproj
dotnet publish ./Explorer.Service.WebApi/Explorer.Service.WebApi.csproj -o publish -c Release