Option Explicit

Dim WshShell
Set WshShell = CreateObject("WScript.Shell")

Dim userInput
Do
    userInput = InputBox("Please select the game version for your region." & vbCrLf & vbCrLf & "Type the correct number:" & vbCrLf & "1 - GenshinImpact.exe [OS version]" & vbCrLf & "2 - YuanShen.exe [CN version for China]", "Genshin Stella Mod by Sefinek")
    If userInput <> "1" And userInput <> "2" And userInput <> "" And userInput <> "9" Then
        MsgBox "Incorrect number entered. Please enter 1, 2 or 9 to cancel.", vbCritical, "Genshin Stella Mod"
    ElseIf userInput = "" Then
        MsgBox "No input provided. Please enter 1, 2, or 9 to cancel.", vbCritical, "Genshin Stella Mod"
        userInput = "0"
    End If
Loop Until userInput = "1" Or userInput = "2" Or userInput = "9"

If userInput <> "0" And userInput <> "9" Then
    Dim fso, outFile, appDataPath
    Set fso = CreateObject("Scripting.FileSystemObject")
    appDataPath = WshShell.ExpandEnvironmentStrings("%APPDATA%\Genshin Stella Mod by Sefinek")
    If Not fso.FolderExists(appDataPath) Then
        fso.CreateFolder appDataPath
    End If
    Set outFile = fso.CreateTextFile(appDataPath & "\game-version.sfn", True)
    outFile.Write(userInput)
    outFile.Close
    MsgBox "Your game version has been saved! You may now close this.", vbInformation, "Genshin Stella Mod"
End If