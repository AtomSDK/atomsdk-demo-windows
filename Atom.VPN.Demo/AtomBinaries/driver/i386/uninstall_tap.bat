@echo off
@cd /d %~dp0
echo Removing TAP driver...
devcon.exe remove tap0901 >NUL
echo TAP Driver Uninstall completed successfully!
exit
