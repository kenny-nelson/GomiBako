#include "kisoExportCmd.h"
#include <maya/MFnPlugin.h>

MStatus initializePlugin(MObject obj) {
  MFnPlugin plugin(obj, "KisoTools", "1.0", "2015");

  MStatus status = plugin.registerCommand(kiso::maya::ExportCmd::kCommandName,
                                          kiso::maya::ExportCmd::CreateInstance,
                                          kiso::maya::ExportCmd::CreateSyntax);

  if (!status) {
    status.perror("registerCommand");
    return status;
  }

  return status;
}

MStatus uninitializePlugin(MObject obj) {
  MFnPlugin plugin(obj);
  MStatus status =
      plugin.deregisterCommand(kiso::maya::ExportCmd::kCommandName);

  if (!status) {
    status.perror("deregisterCommand");
    return status;
  }

  return status;
}
