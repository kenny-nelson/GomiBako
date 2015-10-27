#include "kisoExportCmd.h"

#include <maya/MArgDatabase.h>
#include <maya/MSyntax.h>
#include <maya/MItSelectionList.h>
#include <maya/MDagPath.h>
#include <maya/MFnIkJoint.h>
#include <maya/MEulerRotation.h>
#include <maya/MVector.h>
#include <maya/MMatrix.h>

namespace kiso {
namespace maya {

namespace {
const char* kFileName = "-f";
const char* kFileNameLong = "-file";
}

    const std::string ExportCmd::kCommandName("kisoExport");

void* ExportCmd::CreateInstance() { return new ExportCmd(); }

MSyntax ExportCmd::CreateSyntax() {
  MSyntax syntax;

  syntax.useSelectionAsDefault(true);

  syntax.addFlag(kFileName, kFileNameLong, MSyntax::kString);

  syntax.setObjectType(MSyntax::kSelectionList, 1);
  syntax.enableQuery(true);
  syntax.enableEdit(false);

  return syntax;
}

MStatus ExportCmd::doIt(const MArgList& args) {
  MArgDatabase arg_data(syntax(), args);
  std::cout << "ExportCmd Start\n";
  std::cout << "ExportCmd End\n";
  std::cout << std::flush;

  if (arg_data.isFlagSet(kFileName)) {
    MString file_name;
    if (arg_data.getFlagArgument(kFileName, 0, file_name)) {
      std::cout << file_name.asChar() << "\n";
    }
  }

  arg_data.getObjects(selections_);

  MItSelectionList joint_list_iter(selections_);
  joint_list_iter.setFilter(MFn::kJoint);
  for (; !joint_list_iter.isDone(); joint_list_iter.next()) {
    MDagPath dag_path;
    joint_list_iter.getDagPath(dag_path);
    MFnIkJoint joint(dag_path);
    double orientation[3];
    MTransformationMatrix::RotationOrder orientation_order;
    joint.getOrientation(orientation, orientation_order);
    double rotation[3];
    MTransformationMatrix::RotationOrder rotation_order;
    joint.getRotation(rotation, rotation_order);

    MTransformationMatrix matrix;

    matrix.addRotation(orientation, orientation_order, MSpace::kWorld);
    matrix.addRotation(rotation, rotation_order, MSpace::kWorld);

    MVector x_axis(1.0, 0.0, 0.0);

    x_axis *= matrix.asMatrix();
    x_axis.normalize();

    std::cout << "Joint:" << dag_path.fullPathName().asChar() << " : "
              << x_axis[0] << "," << x_axis[1] << "," << x_axis[2] << "\n";
  }

  MStatus status;
  return status;
}

}  // namespace maya
}  // namespace kiso