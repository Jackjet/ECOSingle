set shell=CreateObject("Wscript.shell") 

b=shell.RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft .NET Framework 4 Client Profile\VersionMajor")

if b<>"4" then  
     shell.Run "notepad"
end if
Wscript.quit