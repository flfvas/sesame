#Persistent
#SingleInstance Force

; ==============================================================================
; 脚本初始化：注册电源通知
; ==============================================================================

; 定义 GUID_CONSOLE_DISPLAY_STATE (显示器状态 GUID)
VarSetCapacity(GUID_CONSOLE_DISPLAY_STATE, 16)
DllCall("ole32\CLSIDFromString", "WStr", "{6FE69556-704A-47A0-8F24-C28D936FDA47}", "Ptr", &GUID_CONSOLE_DISPLAY_STATE)

; 注册电源设置通知
hPowerNotify := DllCall("RegisterPowerSettingNotification", "Ptr", A_ScriptHwnd, "Ptr", &GUID_CONSOLE_DISPLAY_STATE, "UInt", 0)

; 监听 Windows 消息 WM_POWERBROADCAST (0x218)
OnMessage(0x218, "OnPowerBroadcast")

; 用于标记是否是脚本刚启动时的第一次检测
global isFirstRun := true

Return

; ==============================================================================
; 消息处理函数
; ==============================================================================
OnPowerBroadcast(wParam, lParam) {
    ; PBT_POWERSETTINGCHANGE = 0x8013 (电源设置已改变)
    If (wParam = 0x8013) {
        
        ; 获取数据里的状态值: 0=Off, 1=On, 2=Dimmed
        DisplayStatus := NumGet(lParam+0, 20, "UChar") 

        If (DisplayStatus = 1) {
            ; 过滤掉脚本刚启动时的第一次状态推送
            if (isFirstRun) {
                isFirstRun := false
                Return
            }

            ; 屏幕点亮后，设置一个定时器去检测WiFi并点击
            ; 建议延迟 1000ms (1秒)，因为电脑刚唤醒时WiFi重连可能需要一瞬间
            SetTimer, CheckAndClick, -1000 
        }
    }
    Return 1
}

; ==============================================================================
; 核心逻辑流程
; ==============================================================================
CheckAndClick:
    ; 1. 检测 WiFi 状态
    if (IsWifiConnected()) {
        ; 2. 如果已连接，执行点击操作
        OPENAIRMODE()
    }
Return

; ==============================================================================
; 功能函数：WiFi 状态检测 (返回 True/False)
; ==============================================================================
IsWifiConnected() {
    ; 屏蔽黑色窗口，静默执行命令
    DetectHiddenWindows, On
    
    ; 使用 ComSpec (cmd.exe) 执行命令并将结果重定向到临时文件
    TempFile := A_Temp "\wifi_status_check.txt"
    RunWait, %ComSpec% /c "netsh wlan show interfaces > `"%TempFile%`"", , Hide

    ; 读取临时文件内容
    FileRead, Output, %TempFile%
    ; 删除临时文件
    FileDelete, %TempFile%

    ; 检测状态（兼容中英文系统）
    ; 如果输出中包含 "已连接" 或 " connected" (注意前面有个空格)
    if (InStr(Output, "已连接") or InStr(Output, " connected")) {
        Return True
    } else {
        Return False
    }
}

; ==============================================================================
; 功能函数：点击操作
; ==============================================================================
OPENAIRMODE()
{
    CoordMode, Mouse , Screen
    __ClickX:=1871
    __ClickY:=1044
    __ClickTimes:=1
    Click %__ClickX%, %__ClickY%, %__ClickTimes%
    
    Sleep, 100 ; 建议在两次点击中间加一点点间隔，防止操作太快失效
    
    __ClickX:=1744
    __ClickY:=756
    __ClickTimes:=1
    Click %__ClickX%, %__ClickY%, %__ClickTimes%
}