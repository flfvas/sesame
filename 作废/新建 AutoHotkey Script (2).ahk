#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.




CheckWiFi:
    ; 调用 netsh wlan show interfaces 获取当前 WiFi 状态
    RunWait, %ComSpec% /c netsh wlan show interfaces > "%A_Temp%\wifi.txt",, Hide
    FileRead, wifiStatus, %A_Temp%\wifi.txt

    ; 检查是否包含 "State : connected"
    if InStr(wifiStatus, "State                   : connected")
    {
        ; 已连接 WiFi，不提示
    }
    else
    {
        MsgBox, 48, WiFi 状态, 当前没有连接任何 WiFi！
    }
return