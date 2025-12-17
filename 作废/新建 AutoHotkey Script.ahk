; AutoHotkey 1.1 脚本
; 检查 WiFi 是否连接并弹出提示


    ; 调用 netsh 并直接捕获输出
    RunWait, %ComSpec% /c netsh wlan show interfaces > "%A_Temp%\wifi_status.txt",, Hide
    FileRead, wifiStatus, %A_Temp%\wifi_status.txt

    ; 去掉大小写和空格差异，方便匹配
    wifiStatus := StrReplace(wifiStatus, "`r")
    wifiStatus := StrReplace(wifiStatus, "`n", "`n")

    ; 判断是否包含 "State" 行
    if InStr(wifiStatus, "State                   : connected")
    {
        MsgBox, 64, WiFi状态, WiFi 已连接！
    }
    else if InStr(wifiStatus, "State                   : disconnected")
    {
        MsgBox, 48, WiFi状态, WiFi 未连接！
    }
    else
    {
        MsgBox, 48, WiFi状态, 无法确定WiFi状态，请检查网卡或命令输出。
    }
