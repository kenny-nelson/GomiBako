@ECHO OFF

SET LIBRARY_ROOT=%~dp0
ECHO LIBRARY_ROOT: %LIBRARY_ROOT%

SET MAYA_TOOL_PATH=%LIBRARY_ROOT%\Maya
SET MAYA_PYTHON_PATH=%MAYA_TOOL_PATH%\Python

SET MAYA_SCRIPT_PATH=%MAYA_SCRIPT_PATH%;%MAYA_PYTHON_PATH%\Scripts
SET MAYA_PLUG_IN_PATH=%MAYA_PLUG_IN_PATH%;%MAYA_TOOL_PATH%\Plugins
SET XBMLANGPATH=%XBMLANGPATH%;%MAYA_TOOL_PATH%\Icons

FOR /F "tokens=2* skip=2" %%A IN ('REG QUERY "HKLM\SOFTWARE\Autodesk\Maya\2016\Setup\InstallPath" /reg:64 /v MAYA_INSTALL_LOCATION') DO SET MAYA2016_LOCATION=%%B
ECHO MAYA2016_LOCATION=%MAYA2016_LOCATION%

"%MAYA2016_LOCATION%bin\maya.exe"
pause