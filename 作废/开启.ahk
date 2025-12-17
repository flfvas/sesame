#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.



; AutoHotkey 1.1 - 无操作一分钟后自动开启飞行模式
; 适用于 Windows 10

#Persistent
#SingleInstance Force
SetBatchLines, -1

; 配置参数
idleTime := 120000  ; 无操作时间(毫秒)，60000 = 1分钟
checkInterval := 40000  ; 检查间隔(毫秒)，5000 = 5秒 

; 飞行模式状态标志
airplaneModeEnabled := false

; 设置定时器，定期检查空闲时间
SetTimer, CheckIdleTime, %checkInterval%

return

CheckIdleTime:
    ; 获取系统空闲时间(毫秒)
    idle := A_TimeIdlePhysical
    
    ; 如果空闲时间超过设定值且飞行模式未开启
    if (idle >= idleTime && !airplaneModeEnabled) {
        EnableAirplaneMode()
        airplaneModeEnabled := true
        
        ; 显示提示
        TrayTip, 飞行模式, 检测到无操作超过1分钟，已自动开启飞行模式, 5, 1
    }
    ; 如果有操作且飞行模式已开启，重置标志
    else if (idle < idleTime && airplaneModeEnabled) {
        airplaneModeEnabled := false
    }
return

EnableAirplaneMode() {
    ; 保存当前鼠标位置
    MouseGetPos, originalX, originalY
    
    ; 使用 CoordMode 确保坐标为屏幕绝对坐标
    CoordMode, Mouse, Screen
    
    ; 第一次点击：坐标 (1871, 1044)
    Click, 1871, 1044
    
    ; 延时 500 毫秒
    Sleep, 500
    
    ; 第二次点击：坐标 (1744, 756)
    Click, 1744, 756
    
    ; 可选：恢复鼠标到原位置（如果需要的话，可以取消注释）
    ; MouseMove, %originalX%, %originalY%, 0
}

; 可选: 添加热键手动触发飞行模式
^!F1::  ; Ctrl+Alt+F1 手动开启飞行模式
    EnableAirplaneMode()
    TrayTip, 飞行模式, 已手动开启飞行模式, 3, 1
return

; 可选: 添加热键退出脚本
^!F12::  ; Ctrl+Alt+F12 退出脚本
    MsgBox, 4, 退出确认, 确定要退出自动飞行模式脚本吗？
    IfMsgBox Yes
        ExitApp
return

; 托盘菜单
Menu, Tray, Tip, 自动飞行模式监控运行中
Menu, Tray, NoStandard
Menu, Tray, Add, 当前状态: 监控中, MenuHandler
Menu, Tray, Disable, 当前状态: 监控中
Menu, Tray, Add
Menu, Tray, Add, 立即开启飞行模式, ManualEnable
Menu, Tray, Add, 退出, MenuExit
return

MenuHandler:
return

ManualEnable:
    EnableAirplaneMode()
    TrayTip, 飞行模式, 已手动开启飞行模式, 3, 1
return

MenuExit:
    ExitApp
return
