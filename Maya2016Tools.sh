#!/bin/bash

set -e

cd "`dirname "$0"`"
LIBRARY_ROOT="`dirname "$0"`"
export LIBRARY_ROOT

MAYA_TOOL_PATH=$LIBRARY_ROOT/Maya
export MAYA_TOOL_PATH
MAYA_PYTHON_PATH=$MAYA_TOOL_PATH/Python
export MAYA_PYTHON_PATH

MAYA_SCRIPT_PATH=$MAYA_SCRIPT_PATH:$MAYA_PYTHON_PATH
export MAYA_SCRIPT_PATH
MAYA_PLUG_IN_PATH=$MAYA_PLUG_IN_PATH:$MAYA_TOOL_PATH/Plugins
export MAYA_PLUG_IN_PATH
XBMLANGPATH=$XBMLANGPATH:$MAYA_TOOL_PATH/Icons
export XBMLANGPATH

MAYA_SDK_HEADER=/Applications/Autodesk/maya2016/devkit/include/maya
export MAYA_SDK_HEADER

MAYA_LOCATION=/Applications/Autodesk/maya2016/Maya.app/Contents
export MAYA_LOCATION

MAYA_LIBRARY_PATH=$MAYA_LOCATION/MacOS
export MAYA_LIBRARY_PATH

MAYA_FRAMEWORK_PATH=$MAYA_LOCATION/Frameworks
export MAYA_FRAMEWORK_PATH

MAYA_SCRIPT_PATH=$MAYA_SCRIPT_PATH:$GEL_MAYA2015_PLUGIN_ROOT/Scripts
export MAYA_SCRIPT_PATH
MAYA_PLUG_IN_PATH=$MAYA_PLUG_IN_PATH:$GEL_MAYA2015_PLUGIN_ROOT/Plugins2015_OSX
export MAYA_PLUG_IN_PATH
XBMLANGPATH=$XBMLANGPATH:$GEL_MAYA2015_PLUGIN_ROOT/Icons
export XBMLANGPATH

echo $MAYA_LOCATION
echo $MAYA_LIBRARY_PATH
echo $MAYA_FRAMEWORK_PATH

MAYA_EXE_PATH=$MAYA_LIBRARY_PATH/Maya
$MAYA_EXE_PATH