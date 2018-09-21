Partial Public Class WinOffline

    Public Class HistoryVector

        Private ProductComponent As String
        Private HistoryEntries As New ArrayList

        Public Sub New(ByVal Component As String, ByVal Entries As ArrayList)
            ProductComponent = Component
            HistoryEntries = Entries
        End Sub

        Public Function IsEmpty() As Boolean
            If HistoryEntries.Item(0).Equals("NoHistory") Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsApplyAction() As Boolean
            Dim EntryString As String
            If HistoryEntries.Count > 0 Then
                ' Get the first element
                ' Example: "[Thu Mar 26 15:35:01 2015] - PTF Wizard installed T5J1378 (DTSVMG)"
                ' Example: "[Thu Apr 23 15:36:33 2015] - PTF Wizard backed out T55V065 (DTSVMG)"
                ' Example: "[Sun Feb 15 14:34:44 2015] - PTF Wizard installed INSTALL-ENC-CERTS (DTSVMG)"
                EntryString = HistoryEntries.Item(0)
                If EntryString.ToLower.Contains("installed") Then
                    Return True
                Else
                    Return False
                End If
            End If
            Return False
        End Function

        Public Function GetPatchName() As String
            Dim EntryString As String
            If HistoryEntries.Count > 0 Then
                If IsEmpty() Then Return ProductComponent
                ' Get the first element
                ' Example: "[Thu Mar 26 15:35:01 2015] - PTF Wizard installed T5J1378 (DTSVMG)"
                ' Example: "[Thu Apr 23 15:36:33 2015] - PTF Wizard backed out T55V065 (DTSVMG)"
                ' Example: "[Sun Feb 15 14:34:44 2015] - PTF Wizard installed INSTALL-ENC-CERTS (DTSVMG)"
                ' Exmaple: "Shared Components -- SC;NoHistory"
                EntryString = HistoryEntries.Item(0)
                If EntryString.ToLower.Contains("installed") Then
                    EntryString = EntryString.Substring(EntryString.IndexOf("installed") + 10)
                    EntryString = EntryString.Substring(0, EntryString.IndexOf("(") - 1)
                    Return EntryString
                ElseIf EntryString.ToLower.Contains("backed out") Then
                    EntryString = EntryString.Substring(EntryString.IndexOf("backed out") + 11)
                    EntryString = EntryString.Substring(0, EntryString.IndexOf("(") - 1)
                    Return EntryString
                Else
                    Return Nothing
                End If
            End If
            Return Nothing
        End Function

        Public Function GetInstalledFiles() As ArrayList
            Dim InstalledFiles As New ArrayList
            For Each p As String In HistoryEntries
                If p.ToLower.StartsWith("installedfile=") Then
                    p = p.Substring(p.IndexOf("=") + 1)
                    p = p.Substring(0, p.IndexOf(","))
                    If Not InstalledFiles.Contains(p) Then
                        InstalledFiles.Add(p)
                    End If
                End If
            Next
            Return InstalledFiles
        End Function

        Public Function GetInstallFilesRaw() As String
            Dim ReturnString As String = ""
            For Each p As String In HistoryEntries
                If p.ToLower.StartsWith("installedfile=") Then
                    ReturnString += p + Environment.NewLine
                End If
            Next
            Return ReturnString
        End Function

        Public Function GetProductComponent() As String
            Return ProductComponent
        End Function

        Public Function GetHeader() As String
            Return HistoryEntries.Item(0)
        End Function

        Public Overrides Function toString() As String
            Dim ReturnString As String = ""
            ReturnString += ProductComponent + Environment.NewLine()
            For Each strLine As String In HistoryEntries
                ReturnString += strLine + ";"
            Next
            Return ReturnString
        End Function

        Public Shared Function ReadCache(ByVal CallStack As String) As ArrayList

            Dim CacheFile As String = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-history.cache"
            Dim CacheReader As System.IO.StreamReader
            Dim strLine As String = ""
            Dim strLine2 As String = ""
            Dim strLine3 As String = ""
            Dim Counter As Integer = 0
            Dim CodeList As New ArrayList
            Dim HistoryEntries As New ArrayList
            Dim RestoredHistory As HistoryVector
            Dim RestoredManifest As New ArrayList
            Logger.WriteDebug(CallStack, "Open file: " + CacheFile)

            If Not System.IO.File.Exists(CacheFile) Then
                Logger.WriteDebug(CallStack, "Cache file does not exist.")
                Return Nothing
            End If

            CacheReader = New System.IO.StreamReader(CacheFile)

            Do While CacheReader.Peek() >= 0
                strLine = CacheReader.ReadLine
                strLine2 = CacheReader.ReadLine
                HistoryEntries = New ArrayList

                While strLine2.Contains(";") And strLine2.Length > 1
                    strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                    HistoryEntries.Add(strLine3)
                    strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
                End While

                RestoredHistory = New HistoryVector(strLine, HistoryEntries)
                RestoredManifest.Add(RestoredHistory)

                If RestoredHistory.IsEmpty Then
                    Logger.WriteDebug(CallStack, "Read: " + RestoredHistory.GetPatchName + ", NoHistory")
                ElseIf RestoredHistory.IsApplyAction Then
                    Logger.WriteDebug(CallStack, "Read: " + RestoredHistory.GetPatchName + ", Applied")
                Else
                    Logger.WriteDebug(CallStack, "Read: " + RestoredHistory.GetPatchName + ", Removed")
                End If
            Loop

            Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
            CacheReader.Close()

            Return RestoredManifest

        End Function

        Public Shared Sub WriteCache(ByVal CallStack As String, ByVal HistoryManifest As ArrayList)

            Dim CacheFile As String = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-history.cache"
            Dim CacheWriter As System.IO.StreamWriter
            Dim CurEvent As HistoryVector

            ' Flush prior cache file
            If System.IO.File.Exists(CacheFile) Then
                Utility.DeleteFile(CallStack, CacheFile)
            ElseIf Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                Logger.WriteDebug(CallStack, "Warning: Temporary folder already cleaned up!")
                Logger.WriteDebug(CallStack, "Warning: Cache file write aborted!")
                Return
            End If

            ' Create a new cache file
            Logger.WriteDebug(CallStack, "Write cache file: " + CacheFile)
            CacheWriter = New System.IO.StreamWriter(CacheFile, False)
            For n As Integer = 0 To HistoryManifest.Count() - 1
                CurEvent = DirectCast(HistoryManifest.Item(n), HistoryVector)
                If CurEvent.IsEmpty Then
                    Logger.WriteDebug(CallStack, "Write: " + CurEvent.GetPatchName + ", NoHistory")
                ElseIf CurEvent.IsApplyAction Then
                    Logger.WriteDebug(CallStack, "Write: " + CurEvent.GetPatchName + ", Applied")
                Else
                    Logger.WriteDebug(CallStack, "Write: " + CurEvent.GetPatchName + ", Removed")
                End If
                CacheWriter.WriteLine(CurEvent.toString)
            Next
            Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
            CacheWriter.Close()

        End Sub

    End Class

End Class