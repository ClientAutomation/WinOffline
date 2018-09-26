# WinOffline
WinOffline is a utility designed to simplify CA IT Client Manager (ITCM) product self-patching and help automate common maintenance procedures, in order to make your day run much smoother.

## License
> Not licensed, use at your own risk.  WinOffline is available on an as-is basis.  CA does not actively maintain or support this utility.  If you have a problem or question, we recommend filing a GitHub Issue.

## Requirements
> .NET Framework 4.0 or newer.

## Multi-Purpose Application
> **Interactive mode** -- WinOffline runs locally and displays a graphical user interface.

> **Command Line mode** -- WinOffline executes locally via the command line interface.

> **Software Delivery mode** -- WinOffline is registered as a software package inside ITCM Software Delivery and runs silently without any user interaction on agents.

## WinOffline Debug Log
> During execution -- **%windir%\temp\WinOffline.txt**  
After completion -- **..\CA\DSM\WinOffline\<timestamp\>.txt**

## WinOffline Exit Codes
> 1- Basic process initialization failed.   
2- Environment variable initialization failed.  
3- Windows registry initialization failed.  
4- ITCM common configuration initialization failed.  
5- Another instance of WinOffline is currently in-progress.  
6- WinOffline debug log initialization failed.  
7- Startup switch initialization failed.  
8- External resource extraction failed.   
9- WinOffline execution failed, a review of the log file is required.  
10- WinOffline executed successfully, but an ITCM patching error has occurred.  
Everything Else- .NET Framework is not installed or there are version compatibility issues.  

## Deviations from ApplyPTF functionality
> * When removing a patch that's applied, instead of marking the patch as "backed out" in the history file, WinOffline will erase the previous record showing the patch was ever applied.
> * When applying the same patch multiple times, WinOffline will create an incremental number of folders in the ..\DSM\REPLACED folder.
> * When removing a patch, WinOffline will seek the latest incremental folder matching the patch name, from the ..\DSM\REPLACED folder.
> * WinOffline ignores the following tags in a JCL file: COMPONENT, SUPERSEDE, RELEASE, GENLEVEL, VERSION, PREREQS, MPREREQS, COREQS, MCOREQS.
> * If a patch is already applied, WinOffline will not prompt or require a switch to re-apply a patch.
> * WinOffline does not prompt or require a switch when a patch is applying new files.
> * WinOffline supports JCL files without FILE replacement tags, to facilitate patches that only will run a command/batch script. For example, here's a JCL file that will only run a command script:
```
PLATFORM:WINDOWS
PRODUCT:DTSVMG
SYSCMD:ScriptA.cmd
```

## Custom Supported JCL tags
VERSIONCHECK
> Designate patch is ITCM version specific. The patch will be SKIPPED if the version does not match.

> Examples:  
VERSIONCHECK:12.9.0.338  
VERSIONCHECK:14.0.0.199  
VERSIONCHECK:14.0.1000.194  
VERSIONCHECK:14.0.2000.255  

Sample JCL file with VERSIONCHECK:
```
PLATFORM:WINDOWS
PRODUCT:DTSVMG
FILE:bin\cfsmcapi.dll
FILE:bin\cfmessenger.dll
VERSIONCHECK:12.9.0.338
```

DEPENDS
> Designate external file dependencies. Useful when delivering scripts to be run by WinOffline that have dependent files.

Sample JCL file with DEPENDS:
```
PLATFORM:WINDOWS
PRODUCT:DTSVMG
FILE:bin\cacertutil.exe
SYSCMD:MyBatchScript.cmd
DEPENDS:HelperScript.cmd
```

WinOffline supports multiple dependencies, either separated by comma or listed individually:

> DEPENDS:SomeFile1.cmd,SomeFile2.dat,SomeFile3.cmd  
--or--  
DEPENDS:SomeFile1.cmd  
DEPENDS:SomeFile2.dat,SomeFile3.cmd

JCL file requirements and formatting:
> * Each JCL file must contain a PRODUCT code.
> * Each JCL file must contain at least one instruction tag (PRESYSCMD, FILE, or SYSCMD).
> * WinOffline supportd JCLs having multiple insturction tags (PRESYSCMD, FILE, or SYSCMD).
> * WinOffline always runs the PRESYSCMD tags, in the order they are found, before performing FILE replacements.
> * WinOffline always runs the SYSCMD tags, in the order they are found, after performing FILE replacements.
> * Similar to the DEPENDS tag, WinOffline also supports multiple comma separated items in the PRESYSCMD or SYSCMD tags.

Here's a sample JCL file:
```
PLATFORM:WINDOWS
PRODUCT:BITCM
PRESYSCMD:ScriptA.cmd
DEPENDS:ScriptC.cmd
FILE:bin\_SomeFile.txt
FILE:bin\_SomeFile2.txt
SYSCMD:ScriptB.cmd
SYSCMD:ScriptF.cmd,ScriptG.cmd
DEPENDS:ScriptD.cmd,ScriptE.cmd
VERSIONCHECK:14.0.1000.194
```

## WinOffline Command Line Switches

-gethistory
> Report agent patch history only. CAF is not recycled and no patch operations are performed. Available in Software Delivery mode only.

-remove [patch1,patch2,...]
> Removes patches from the agent. The agent history file(s) are scanned to find the associated product component and ensure the original files are available for restoration.

>Examples:  
-remove T5X0001  
-remove T5X0001,T5X0002

-cleanlogs
>1- Cleans up the DSM logs folder.   
2- Modifies the folder access control list (ACL) if Administrator permissions are missing.

-resetcftrace
> Runs a "cftrace -c reset".

-rmcamcfg
> 1- Saves a backup of any existing cam.cfg file, if present.  
2- Deletes the existing cam.cfg or cam.bak files, if present.

-cleanagent
> 1- Cleans up the ..\CA\DSM\Agent\units folder.  
2- Cleans up the ..\CA\DSM\SD\TMP\activate folder.  
3- Cleans up the ..\CA\DSM\dts\dta\status folder.  
4- Cleans up the ..\CA\DSM\dts\dta\staging folder.  
5- Recreates the ..\CA\DSM\dts\dta\status\index file.  

-cleanserver
> 1- Cleans up the ..\CA\DSM\SD\ASM\D folder.  
2- Cleans up the ..\CA\DSM\SD\ASM\LIBRARY\activate folder.\**  
3- Cleans up the ..\CA\DSM\dts\dta\status folder.  
4- Cleans up the ..\CA\DSM\dts\dta\staging folder.  
5- Recreates the ..\CA\DSM\dts\dta\status\index file.  

> **Removes any NTFS junction points created back to Domain Manager's software library, before deleting any files/folders.

-checklibrary
> Analyzes the software delivery library for consistency problems, and reports results, __without making any changes to the database, library.dct file or LIBRARY folder__.

-cleanlibrary
> Performs cleanup on the software delivery library, repairing consistency problems between the database, library.dct file and LIBRARY folder.

-cleancerts
> 1- Keeps a rolling backup, up to two copies, of the CBB folder.  
2- Cleans up formatting and syntax errors in both certstor.dat and cbbkstor.dat files.  
3- Cleans up any entry in certstor.dat, referencing a non-existent certificate file in certdb.  
4- Cleans up all itcm-anonymous certificate entries from the certstor.dat file.  
5- Cleans the cbbkstor.dat file of any private keys not matching with an entry in the certstor.dat file.  
6- Cleans the certdb folder, of any certificate not mating with an entry in the certstor.dat file.  
7- Runs "cacertutil list -v" to generate a new anonymous certificate.

-skipcafstartup
> Don't start the CAF service after performing all patching/maintenance operations.

-skipcam
> Don't stop/recycle the CAM service.  This may prevent some patches from successfully applying without the need for a reboot.

-skipdmprimer
> Don't stop/recycle the DMPrimer service.  This may prevent some patches from successfully applying without the need for a reboot.

-skiphm
> Don't stop/recycle the hmAgent service. This may prevent some patches from successfully applying without the need for a reboot.

-loadgui
> Launch DSM Explorer for all currently logged on users.

-simulatestop
> For debugging. Skips stopping/starting the CAF service. Used when testing WinOffline with dummy patches.

-simulatepatch
> For debugging. Allows WinOffline to go through all the motions without acutally making any changes via the patches.

-rmhistorybefore
> Deletes the %computername%.his file in the root of the DSM folder, and removes the REPLACED directory, BEFORE performing the current patch operation(s).

-rmhistoryafter
> Deletes the %computername%.his file in the root of the DSM folder, and removes the REPLACED directory, AFTER performing the current patch operation(s).

-dumpcazipxp
> Extract cazipxp.exe to the current working directory, and exit without making any changes.

-regenuuid
> 1- Deletes the HostUUID from the comstore.  
2- Deletes the HostUUID from the registry.

-signalreboot
> Signal a software delivery reboot request.

-reboot
> Reboot system after completion of the last stage of WinOffline.

-uninstallitcm
> Perform normal uninstallation of ITCM. Choose this option if other CA products are installed.

-removeitcm
> Perform full removal of ITCM. Choose this option if ITCM is the only CA product installed.  Will attempt removal of entire CA folder and ComputerAssociates registry key.

-keepuuid
> When performing full removal (-removeitcm), retain only the HostUUID registry key for future use.

-disableenc
> Disable the ENC client.

## WinOffline Database Maintenance Switches

__Note__: These switches are only valid on ITCM managers.

-testdbconn
> Tests connection to the database. Uses Windows authentication by default, unless (-dbuser) switch is also provided.

-dbuser \<username\>
> Authenticate via sql credentials using the provided account. The (-dbpassword) switch is optional, otherwise user will be prompted at runtime.

-dbpassword \<password\> (or -dbpasswd \<password\>)
> Provide password for provided sql account. Requires the (-dbuser) switch to be provided. If this switch is omitted, the user will be prompted.

-dbserver \<server name\>
> Provide a server host name, as required for connecting to the database.

-dbinstance \<instance name\>
> Provide a non-default instance name, as required for connecting to the database.

-dbport \<port\>
> Provide a non-default port number, as required for connecting to the database.

-mdboverview
> Output an overview of the ITCM manager's database.
