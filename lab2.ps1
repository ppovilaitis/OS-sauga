
 # sukuria registry key su scripto paleidimo pradžios data
 $scriptDate = Get-Date
 New-Item -Path HKCU:\SOFTWARE\PowershellScriptRunTime -Name RunTime -Force
 Set-Item -Path HKCU:\SOFTWARE\PowershellScriptRunTime\RunTime -Value "$scriptDate"

 # pagal vartotojo nurodytus žodžius atlieka proceso paiešką ir suteikia galimybę jį išjungti arba tęsti  
 Write-Host 'Select one of the fallowing:'
 Write-Host '[1] Search process by its full name'
 Write-Host '[2] Search process by half name'
 $choice = Read-Host -Prompt 'Enter ur choice'

 if ($choice -eq 1) #pagal pilna proceso pavadinima
{ $processName = Read-Host -Prompt 'Please enter process name that you want to search'
Write-Host "Searching for indicated process name" -ForegroundColor Green

if((Get-Process -Name $processName -ea SilentlyContinue) -eq $Null){ 
        Write-Host "Indicated process is not currently running" -BackgroundColor Red
}

else{ 
    Write-Host "Found these $processName processes running:" -ForegroundColor Green
    Get-Process -Name $processName
    $userAnswer = Read-Host -Prompt 'Do you wish to stop process? Y-yes N-no'
    if ($userAnswer -eq 'Y') {
    Stop-Process -Name $processName -Force
    Write-Host "Process stopped" -ForegroundColor Green
    }
 }
 }
 if ($choice -eq 2) #pagal proceso vardo dali
 { $processName = Read-Host "`Enter half process name" -ErrorAction SilentlyContinue
 if ((Get-Process -ProcessName $processName* -IncludeUserName -ErrorAction SilentlyContinue) -eq $Null )
{
    [System.Windows.MessageBox]::Show('Process is not running')
}
else{ 
    Write-Host "Found these $processName processes running:" -ForegroundColor Green
    Get-Process -ProcessName $processName* -IncludeUserName -ErrorAction SilentlyContinue
    $userAnswer = Read-Host -Prompt 'Do you wish to stop process? Y-yes N-no'
    if ($userAnswer -eq 'Y') {
    Stop-Process -Name $processName* -Force
    Write-Host "Process stopped" -ForegroundColor Green
    }
   }
 }


  # kas 30 sekundžių sukuria failą su atrinktu procesu VM, bei kai failų kiekis didesnis nei 5 - šalina paskutinį ir pakeičia nauju
   For (){
    $currentDate = Get-Date -Format "dd-MM-yyyy_hh_mm_ss"
    $allProcesses = Get-Process
    $date = Get-Date
    $cutoffDate = $date.AddMinutes(-2)
    $filteredProcess = $allProcesses | Where-Object {$_.VM -gt 10000} | select name, id, handles | Out-File "C:\Temp\FilteredProcessList$($currentDate).txt"
    Write-Host "Processes filtered to file" -ForegroundColor Green
    Write-Host "Next process filter in 30 seconds..." -ForegroundColor Green
     Start-Sleep -s 30
  
  Get-ChildItem -Path C:\temp\ -File | Where-Object LastWriteTime -LT $cutoffDate | Remove-Item -Force -Verbose
  }
