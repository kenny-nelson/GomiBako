#!/bin/bash

set -e

cd "`dirname "$0"`"
LIBRARY_ROOT="`dirname "$0"`"
export LIBRARY_ROOT
echo LIBRARY_ROOT: $LIBRARY_ROOT

KISO_MAYA_PATH=$LIBRARY_ROOT/Maya
export KISO_MAYA_PATH
echo KISO_MAYA_PATH: $KISO_MAYA_PATH

MAYA_SDK_HEADER=/Applications/Autodesk/maya2015/devkit/include/maya
export MAYA_SDK_HEADER

MAYA_LOCATION=/Applications/Autodesk/maya2015/Maya.app/Contents
export MAYA_LOCATION

MAYA_LIBRARY_PATH=$MAYA_LOCATION/MacOS
export MAYA_LIBRARY_PATH

MAYA_FRAMEWORK_PATH=$MAYA_LOCATION/Frameworks
export MAYA_FRAMEWORK_PATH

echo $MAYA_LOCATION
echo $MAYA_LIBRARY_PATH
echo $MAYA_FRAMEWORK_PATH

XCODE_LOCATION=/Applications/Xcode.app/Contents
$XCODE_LOCATION/MacOS/Xcode