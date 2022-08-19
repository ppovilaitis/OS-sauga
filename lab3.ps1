New-Item -Path 'C:\lab3_Prano_Povilaicio\' -ItemType Directory

$currentDate = Get-Date -Format "dd-MM-yyyy_hh_mm_ss"

#parodo kompiuterio varda
$computerName = hostname | Out-File "C:\lab3_Prano_Povilaicio\ComputerInformation$($currentDate).txt"

#parodo kompiuterio disko parametrus
$diskParameters = get-wmiobject -class win32_logicaldisk | Out-File -FilePath "C:\lab3_Prano_Povilaicio\ComputerInformation$($currentDate).txt" -Append

#parodo vidutini CPU naudojima esamu laiku
$cpuUsage = Get-WmiObject Win32_Processor | Measure-Object -Property LoadPercentage -Average | Select Average | Out-File -FilePath "C:\lab3_Prano_Povilaicio\ComputerInformation$($currentDate).txt" -Append

#parodo visa ir laisva vieta kiekviename is esamu disku (MB)
$disksSpace = Get-CimInstance -Class CIM_LogicalDisk | Select-Object @{Name="Size(MB)";Expression={$_.size/1MB}}, @{Name="Free Space(MB)";Expression={$_.freespace/1mb}}, @{Name="Free (%)";Expression={"{0,6:P0}" -f(($_.freespace/1mb) / ($_.size/1mb))}}, DeviceID | Out-File -FilePath "C:\lab3_Prano_Povilaicio\ComputerInformation$($currentDate).txt" -Append

#iraso i faila tik veikiancius servisus
$activeServices = Get-Service | Where Status -eq "Running" | Out-File -FilePath "C:\lab3_Prano_Povilaicio\ActiveServices.txt"

#iraso i faila tik neveikiancius servisus
$stoppedServices = Get-Service | Where Status -eq "Stopped" | Out-File -FilePath "C:\lab3_Prano_Povilaicio\StoppedServices.txt"

#funkcija serviso isvedimui pagal vartotojo ivesta ieskomo serviso pavadinima
$serviceName = Read-Host -Prompt 'Please enter service name that you want to search'
Write-Host "Searching for indicated service name" -ForegroundColor Green

if((Get-Service $serviceName -ea SilentlyContinue) -eq $Null){ 
        Write-Host "Indicated service is not currently running" -BackgroundColor Red
}

else{ 
    Write-Host "Found these $serviceName services running:" -ForegroundColor Green
    Get-Service -Name $serviceName
    }

#Nuskaito ir isveda 10 paskutiniu ivykiu is Event Log
$lastEventLogs = Get-EventLog -list | Select-Object -First 10
Write-Host "10 last events from Event Log:" -ForegroundColor Green
$lastEventLogs

#funkcija perduodamos programos ivykiams
$processName = Read-Host -Prompt 'Please enter process name that you want to search'
$processNumber = Read-Host -Prompt 'Please enter number of events that you wish to see of the process'

if ((Get-EventLog -list | ? {$_.LogDisplayName -eq $processName}) -eq $Null){ 
        Write-Host "Indicated process is not currently running" -BackgroundColor Red
}

else{ 
    Get-EventLog -LogName $processName | Select-Object -Last $processNumber | Out-File -FilePath "C:\lab3_Prano_Povilaicio\EventLogPart_$($currentDate).txt"
    }
    
#iskviecia sysinternal psping programa, pingina google.com ir iraso i faila rezultata
C:\Users\DiAMOND\Desktop\ISKS\OS_ir_sauga\sysinternal\Psping.exe google.com | Out-File -FilePath "C:\lab3_Prano_Povilaicio\sysinternal.txt"

#sukuria kasdienine task siao scripto vykdymui
$action = New-ScheduledTaskAction -Execute 'Powershell.exe' -Argument "C:\lab3_Prano_Povilaicio\lab3.ps"

$trigger =  New-ScheduledTaskTrigger -Daily -At 10am

Register-ScheduledTask -Action $action -Trigger $trigger -TaskName "lab3"