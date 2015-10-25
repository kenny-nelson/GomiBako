#include "kisoExportCmd.h"
#include "kisoJointXAxisCmd.h"
#include <maya/MFnPlugin.h>

MStatus initializePlugin(MObject obj) {
  MFnPlugin plugin(obj, "KisoTools", "1.0", "2015");

  MStatus status;
  {
    status = plugin.registerCommand(kiso::maya::ExportCmd::kCommandName.c_str(),
                                    kiso::maya::ExportCmd::CreateInstance,
                                    kiso::maya::ExportCmd::CreateSyntax);
    if (!status) {
      status.perror("registerCommand");
      return status;
    }
  }

  {
    status =
        plugin.registerCommand(kiso::maya::JointXAxisCmd::kCommandName.c_str(),
                               kiso::maya::JointXAxisCmd::CreateInstance,
                               kiso::maya::JointXAxisCmd::CreateSyntax);
    if (!status) {
      status.perror("registerCommand");
      return status;
    }
  }

  return status;
}

MStatus uninitializePlugin(MObject obj) {
  MFnPlugin plugin(obj);
  MStatus status;
  {
    status =
        plugin.deregisterCommand(kiso::maya::ExportCmd::kCommandName.c_str());

    if (!status) {
      status.perror("deregisterCommand");
      return status;
    }
  }

  {
    status = plugin.deregisterCommand(
        kiso::maya::JointXAxisCmd::kCommandName.c_str());

    if (!status) {
      status.perror("deregisterCommand");
      return status;
    }
  }

  return status;
}
