@echo off
@cd /d %~dp0
echo Removing old TAP driver...
devcon.exe remove tap0901 
echo Installing TAP driver...
devcon.exe install OemWin2k.inf tap0901 
echo TAP Driver Re-Installation completed successfully!
exit