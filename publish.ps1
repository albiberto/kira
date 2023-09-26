[System.Environment]::SetEnvironmentVariable('DOTNET_ENVIRONMENT','Production',[System.EnvironmentVariableTarget]::User)

dotnet publish .\src\Kira\Kira.csproj -c Release -o dist --self-contained true -r win-x64 -p:PublishSingleFile=true  
