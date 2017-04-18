Dim powerManager
Set powerManager = CreateObject("PowerStateManagement.PowerManager")
Dim result 
result = ""
result = result & "Last Sleep Time: " & powerManager.GetLastSleepTime() & vbCrLf
result = result & "Last Wake Time: " & powerManager.GetLastWakeTime() & vbCrLf
result = result & "Battery Present: " & powerManager.GetBatteryState() & vbCrLf

MsgBox result