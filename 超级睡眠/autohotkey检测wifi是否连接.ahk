; 屏蔽黑色窗口，静默执行命令
DetectHiddenWindows, On

; 获取 WiFi 状态
CheckWifiStatus() {
    ; 使用 ComSpec (cmd.exe) 执行命令并将结果重定向到临时文件，避免窗口闪烁
    TempFile := A_Temp "\wifi_status.txt"
    RunWait, %ComSpec% /c "netsh wlan show interfaces > `"%TempFile%`"", , Hide

    ; 读取临时文件内容
    FileRead, Output, %TempFile%
    FileDelete, %TempFile%

    ; 检测状态（兼容中英文系统）
    if (InStr(Output, "已连接") or InStr(Output, " connected")) {
        ; 使用正则提取 SSID 名称
        RegExMatch(Output, "SSID\s*:\s*(.*)\r", Match)
        SSIDName := Trim(Match1)
        
        MsgBox, 64, 连接检测, WiFi 状态：已连接`n网络名称: %SSIDName%
    } else {
        MsgBox, 48, 连接检测, WiFi 状态：未连接
    }
}

; 执行检测
CheckWifiStatus()

; 执行完毕后退出脚本
ExitApp