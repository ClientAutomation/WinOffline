Imports System.Management

Public Class Globals

    ' WinOffline and System Globals
    Public Shared CommandLineArgs As String() = Nothing                         ' Command line arguments passed to the application.
    Public Shared AppVersion As String = "2018.08.30"                           ' Version string of the current build.
    Public Shared ProcessName As String = Nothing                               ' Fullpath including filename of the application process.
    Public Shared ProcessShortName As String = Nothing                          ' Filename of the application process.
    Public Shared ProcessFriendlyName As String = Nothing                       ' Friendly name of the application process.
    Public Shared DotNetVersion As String = Nothing                             ' Microsoft .NET framework version.
    Public Shared ProcessFilePath As String = Nothing                           ' Directory of the process. (e.g. C:\SomeDirectory)
    Public Shared AttachedtoConsole As Boolean = False                          ' Flag: Attached to console.
    Public Shared HostName As String = Nothing                                  ' Hostname of computer executing the application.
    Public Shared ProcessID As Integer = Nothing                                ' The process ID (PID) of the application process.
    Public Shared ParentProcessName As String = Nothing                         ' Friendly name of the immediate parent process.
    Public Shared ParentProcessTree As New List(Of String)                      ' List of all parent process names.
    Public Shared ProcessIdentity As Security.Principal.WindowsIdentity         ' The identity currently executing the process.
    Public Shared DispatcherReturnCode As Integer = 0                           ' Return code from dispatcher.
    Public Const THREAD_REST_INTERVAL As Integer = 50                           ' Default rest interval for threads.

    ' Embedded Assembly Globals
    Public Shared MyAssembly As System.Reflection.Assembly = Nothing            ' Stores the application code that is currently running.
    Public Shared MyRoot As String = Nothing                                    ' Stores the name of the assembly.
    Public Shared MyResourceStream As System.IO.Stream = Nothing                ' A stream for reading the embedded resource.
    Public Shared MyBuffer() As Byte = Nothing                                  ' A buffer to store each byte of the embedded resource.
    Public Shared MyFileStream As System.IO.FileStream = Nothing                ' A filestram to write the embedded resource to disk.

    ' Logger Globals
    Public Shared WindowsBase As String = Nothing                               ' Path to base Windows folder (e.g. %windir%).
    Public Shared WindowsTemp As String = Nothing                               ' Path to a system temporary directory (e.g. %windir%\temp).
    Public Shared WinOfflineTemp As String = Nothing                            ' Path to WinOffline temporary directory (e.g. %windir%\temp\WinOffline).
    Public Shared DebugStreamWriter As System.IO.StreamWriter = Nothing         ' The debug stream writer for the applicaiton log file.
    Public Shared AdditionalDebug As New ArrayList                              ' Array for reading in parallel execution debuging for later merge.

    ' Client Automation Globals
    Public Shared WorkingDirectory As String = Nothing                          ' The working directory of the application process.
    Public Shared ITCMInstalled As Boolean = Nothing                            ' Client automation installation flag.
    Public Shared ITCMVersion As String = Nothing                               ' Registry: ITCM version.
    Public Shared CAFolder As String = Nothing                                  ' Registry: CA folder.
    Public Shared DSMFolder As String = Nothing                                 ' Registry: DSM folder.
    Public Shared SharedCompFolder As String = Nothing                          ' Registry: Shared components folder.
    Public Shared ITCMFunction As String = Nothing                              ' Registry: ITCM function.
    Public Shared CAPKIFolder As String = Nothing                               ' Registry: CAPKI folder.
    Public Shared HostUUID As String = Nothing                                  ' Registry: HostUUID.
    Public Shared SDFolder As String = Nothing                                  ' Registry: USD installation folder.
    Public Shared CAMFolder As String = Nothing                                 ' Registry: CAM installation folder.
    Public Shared SSAFolder As String = Nothing                                 ' Registry: SSA installation folder.
    Public Shared DTSFolder As String = Nothing                                 ' Registry: DTS installation folder.
    Public Shared EGCFolder As String = Nothing                                 ' Registry: EGC installation folder.
    Public Shared PMLAFolder As String = Nothing                                ' Registry: Perf lite agent installation folder.
    Public Shared PMLAVersion As String = Nothing                               ' Registry: Perf lite agent version.
    Public Shared DMPrimerFolder As String = Nothing                            ' Registry: DMPrimer installation folder.
    Public Shared CurrentJobOutputID As String = Nothing                        ' File: Software delivery job output ID.
    Public Shared CachedJobOutputID As String = Nothing                         ' File: Cached software delivery job output ID.
    Public Shared JobOutputFolder As String = Nothing                           ' File: Software delivery job output folder.
    Public Shared JobOutputFile As String = Nothing                             ' File: Software delivery job output file.
    Public Shared SDLibraryFolder As String = Nothing                           ' Comstore: Path to the SD library.
    Public Shared ENCFunction As String = Nothing                               ' Comstore: ENC functionality description.
    Public Shared ENCGatewayServer As String = Nothing                          ' Comstore: ENC gateway server.
    Public Shared ENCServerTCPPort As String = Nothing                          ' Comstore: ENC server TCP port.
    Public Shared DomainManager As String = Nothing                             ' Comstore: Domain manager for client.
    Public Shared ScalabilityServer As String = Nothing                         ' Comstore: Scalability server for client.
    Public Shared FeatureList As New ArrayList                                  ' Registry: List of DSM plugins/features.
    Public Shared ITCMComstoreVersion As String = Nothing                       ' Comstore: ITCM version.
    Public Shared DatabaseServer As String = Nothing                            ' Comstore: Database server.
    Public Shared DatabaseInstance As String = Nothing                          ' Comstore: Database instance.
    Public Shared DatabasePort As String = Nothing                              ' Comstore: Database port.
    Public Shared DatabaseName As String = Nothing                              ' Comstore: Database name.
    Public Shared DatabaseUser As String = Nothing                              ' Comstore: Database username.

    ' Application Logic Globals
    Public Shared SDBasedMode As Boolean = False                                ' Flag: Execution is software delivery BASED.
    Public Shared WriteSDJobOutput As Boolean = False                           ' Flag: Write job output for software delivery.
    Public Shared DirtyFlag As Boolean = False                                  ' Flag: The current execution recovered from a prior failed execution.
    Public Shared FinalStage As Boolean = False                                 ' Flag: Current execution is the last stage-- finialize the debug log.
    Public Shared RunningAsSystemIdentity As Boolean = False                    ' Flag: Process is running under system identity.
    Public Shared PatchErrorDetected As Boolean = False                         ' Flag: A patch error was recorded in StageII.
    Public Shared StageICompleted As Boolean = False                            ' Flag: StageI has been completed.
    Public Shared StageIICompleted As Boolean = False                           ' Flag: StageII has been completed.
    Public Shared StageIIICompleted As Boolean = False                          ' Flag: StageIII has been completed.
    Public Shared RebootOnTermination As Boolean = False                        ' Flag: Reboot upon termination of WinOffline.

    ' User Switch Globals
    Public Shared RemovePatchSwitch As Boolean = False                          ' Switch: Remove patches (instead of apply).
    Public Shared CleanupLogsSwitch As Boolean = False                          ' Switch: Cleanup various log folders.
    Public Shared ResetCftraceSwitch As Boolean = False                         ' Switch: Reset cftrace level.
    Public Shared RemoveCAMConfigSwitch As Boolean = False                      ' Switch: Remove CAM configuration file.
    Public Shared CleanupAgentSwitch As Boolean = False                         ' Switch: Perform agent cleanup.
    Public Shared CleanupCertStoreSwitch As Boolean = False                     ' Switch: Certificate store cleanup.
    Public Shared CleanupServerSwitch As Boolean = False                        ' Switch: Perform scalability server cleanup.
    Public Shared CleanupSDLibrarySwitch As Boolean = False                     ' Switch: Perform software library cleanup.
    Public Shared CheckSDLibrarySwitch As Boolean = False                       ' Switch: Analyze software library without making changes.
    Public Shared SkipCAFStartUpSwitch As Boolean = False                       ' Switch: Skip CAF startup.
    Public Shared SkipCAM As Boolean = False                                    ' Switch: Don't stop CAM service.
    Public Shared SkipDMPrimer As Boolean = False                               ' Switch: Don't stop DMPrimer service.
    Public Shared SkiphmAgent As Boolean = False                                ' Switch: Don't stop hmAgent.
    Public Shared LaunchGuiSwitch As Boolean = False                            ' Switch: Launch DSM Explorer.
    Public Shared SimulateCafStopSwitch As Boolean = False                      ' Switch: Simulate recycling CAF.
    Public Shared SimulatePatchSwitch As Boolean = False                        ' Switch: Simulate patch operations.
    Public Shared SimulatePatchErrorSwitch As Boolean = False                   ' Switch: Simulate a patching error.
    Public Shared RemoveHistoryBeforeSwitch As Boolean = False                  ' Switch: Remove patch history file, BEFORE patch operations.
    Public Shared RemoveHistoryAfterSwitch As Boolean = False                   ' Switch: Remove patch history file, AFTER patch operations.
    Public Shared DumpCazipxpSwitch As Boolean = False                          ' Switch: Extract cazipxp.exe utility and exit without changes.
    Public Shared RegenHostUUIDSwitch As Boolean = False                        ' Switch: Regenerate HostUUID.
    Public Shared SDSignalRebootSwitch As Boolean = False                       ' Switch: Signal software delivery reboot during stage III.
    Public Shared RebootSystemSwitch As Boolean = False                         ' Switch: Initiate a reboot upon completion of WinOffline.
    Public Shared UninstallITCM As Boolean = False                              ' Switch: Perform normal uninstall of ITCM.
    Public Shared RemoveITCM As Boolean = False                                 ' Switch: Perform normal uninstall of ITCM, and additional cleanup.
    Public Shared KeepHostUUIDSwitch As Boolean = False                         ' Switch: Retain HostUUID registry key when performing -removeitcm.
    Public Shared DisableENCSwitch As Boolean = False                           ' Switch: Disable ENC.
    Public Shared GetHistorySwitch As Boolean = False                           ' Switch: Get patch history.
    Public Shared StopCAFSwitch As Boolean = False                              ' Switch: Stop the CAF service on-demand.
    Public Shared StartCAFSwitch As Boolean = False                             ' Switch: Start the CAF service on-demand.
    Public Shared DbUser As String = Nothing                                    ' Database username.
    Public Shared DbPassword As String = Nothing                                ' Database password.
    Public Shared DbTestConnectionSwitch As Boolean = False                     ' Switch: Perform database test connection.
    Public Shared MdbOverviewSwitch As Boolean = False                          ' Switch: Provide database overview.
    Public Shared MdbCleanAppsSwitch As Boolean = False                         ' Switch: Perform mdb cleanapps script against database.
    Public Shared LaunchAppSwitch As Boolean = False                            ' Switch: Launch specified application.
    Public Shared LaunchAppContext As String = Nothing                          ' Application launch context (e.g. System/All/User/Console).
    Public Shared LaunchAppFileName As String = Nothing                         ' Filename of application to launch (e.g. %windir%\system32\notepad.exe)
    Public Shared LaunchAppArguments As String = Nothing                        ' Arguments to pass to launch application.

    ' GUI Globals
    Public Shared WinOfflineExplorer As WinOfflineUI = Nothing                  ' WinOffline UI for interactive application.
    Public Shared ProgressGUI As ProgressUI = Nothing                           ' Progress UI for interactive execution.
    Public Shared ProgressUIThread As System.Threading.Thread = Nothing         ' Thread for progress GUI execution.
    Public Shared DebugLogSummary As String = Nothing                           ' Summary for debug log. (Froward order)
    Public Shared ProgressGUISummary As String = Nothing                        ' Summary for the progress GUI. (Reverse order)

End Class