@echo off
@cd /d %~dp0
echo Removing TAP driver...
devcon.exe remove tap0901
echo TAP Driver Uninstall completed successfully!
exit
