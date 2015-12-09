Param($what)
if ($what -eq "producer" -or $what -eq "all") {
	start .\Producer\bin\Debug\Producer.exe
}
if ($what -eq "consumer" -or $what -eq "all") {
	start .\Consumer\bin\Debug\Consumer.exe
}