msbuild .\MessageContracts\MessageContracts.sln
if (!(test-path -path lib)) {
	mkdir lib
}
.\nuget.exe pack .\MessageContracts\MessageContracts.csproj -OutputDirectory lib
.\nuget.exe restore .\Producer\Producer.sln
msbuild .\Producer\Producer.sln
.\nuget.exe restore .\Consumer\Consumer.sln
msbuild .\Consumer\Consumer.sln