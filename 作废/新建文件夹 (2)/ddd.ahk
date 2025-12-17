; WiFi 连接通知脚本
; AutoHotkey 1.1 版本

#NoEnv
#SingleInstance Force
SetWorkingDir %A_ScriptDir%

; 接收命令行参数
param := A_Args[1]

; 如果接收到参数
if (param != "")
{
    if (param = "WiFiConnected")
    {
        MsgBox, 64, WiFi 连接成功, WiFi 已成功连接！`n`n来自 C# 程序的通知。, 5
    }
    else
    {
        MsgBox, 64, WiFi 通知, 接收到参数: %param%, 5
    }
}
else
{
    MsgBox, 48, 提示, 未接收到任何参数。
}

ExitApp