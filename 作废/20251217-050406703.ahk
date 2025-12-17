#Persistent
#SingleInstance Force

; ==============================================================================
; 脚本初始化：注册电源通知
; ==============================================================================

; 定义 GUID_CONSOLE_DISPLAY_STATE (显示器状态 GUID) [cite: 7]
VarSetCapacity(GUID_CONSOLE_DISPLAY_STATE, 16)
DllCall("ole32\CLSIDFromString", "WStr", "{6FE69556-704A-47A0-8F24-C28D936FDA47}", "Ptr", &GUID_CONSOLE_DISPLAY_STATE) [cite: 7]

; 注册电源设置通知 [cite: 8]
hPowerNotify := DllCall("RegisterPowerSettingNotification", "Ptr", A_ScriptHwnd, "Ptr", &GUID_CONSOLE_DISPLAY_STATE, "UInt", 0) [cite: 8]

; 监听 Windows 消息 WM_POWERBROADCAST (0x218) [cite: 9]
OnMessage(0x218, "OnPowerBroadcast") [cite: 9]

; 用于标记是否是脚本刚启动时的第一次检测 [cite: 16]
global isFirstRun := true

Return

; ==============================================================================
; 消息处理函数
; ==============================================================================
OnPowerBroadcast(wParam, lParam) {
    ; PBT_POWERSETTINGCHANGE = 0x8013 (电源设置已改变) [cite: 10]
    If (wParam = 0x8013) {
        ; 获取数据里的状态值 [cite: 11]
        ; Data 的位置在 lParam + 20 [cite: 12]
        DisplayStatus := NumGet(lParam+0, 20, "UChar") 
        
        ; 1 = 屏幕开启 (On) [cite: 14]
        If (DisplayStatus = 1) {
            if (isFirstRun) {
                isFirstRun := false
                Return
            }

            ; 使用 SetTimer 异步处理检测逻辑，避免阻塞系统消息 
            SetTimer, CheckAndClick, -10
        }
    }
    Return 1
}

; ==============================================================================
; 核心逻辑：检测并点击
; ==============================================================================
CheckAndClick:
    ; --- 第一部分：检测 WiFi 状态 ---
    TempFile := A_Temp "\wifi_status.txt" [cite: 2]
    ; 静默执行 netsh 命令 
    RunWait, %ComSpec% /c "netsh wlan show interfaces > `"%TempFile%`"", , Hide [cite: 2]

    FileRead, Output, %TempFile% [cite: 3]
    FileDelete, %TempFile% [cite: 3]

    ; 检测是否包含“已连接”或“connected”关键词 
    if (InStr(Output, "已连接") or InStr(Output, " connected")) {
        ; 如果已连接，则执行点击函数
        OPENAIRMODE()
    }
Return

OPENAIRMODE() {
    CoordMode, Mouse, Screen
    ; 执行点击序列
    __ClickX:=1871
    __ClickY:=1044
    __ClickTimes:=1
    Click %__ClickX%, %__ClickY%, %__ClickTimes%
    
    Sleep, 200 ; 增加微小延迟确保系统响应界面

    __ClickX:=1744
    __ClickY:=756
    __ClickTimes:=1
    Click %__ClickX%, %__ClickY%, %__ClickTimes%
}