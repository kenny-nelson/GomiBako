#include "kisoJointXAxisCmd.h"

#include <maya/MArgDatabase.h>
#include <maya/MSyntax.h>
#include <maya/MItSelectionList.h>
#include <maya/MDagPath.h>
#include <maya/MFnIkJoint.h>
#include <maya/MEulerRotation.h>
#include <maya/MVector.h>
#include <maya/MMatrix.h>
#include <maya/MGlobal.h>

namespace kiso {
namespace maya {

const std::string JointXAxisCmd::kCommandName("kisoJointXAxis");

void* JointXAxisCmd::CreateInstance() { return new JointXAxisCmd(); }

MSyntax JointXAxisCmd::CreateSyntax() {
  MSyntax syntax;

  syntax.useSelectionAsDefault(true);
  syntax.setObjectType(MSyntax::kStringObjects, 1);
  syntax.enableQuery(true);
  syntax.enableEdit(false);

  return syntax;
}

MStatus JointXAxisCmd::doIt(const MArgList& args) {
  MStatus status = ParseArgs(args);
  if (status != MS::kSuccess) {
    std::cerr << "Invalid argument.\n";
    return status;
  }

  MItSelectionList joint_list_iter(selections_);
  joint_list_iter.setFilter(MFn::kJoint);

  MDoubleArray resultValues;
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

    resultValues.append(x_axis.x);
    resultValues.append(x_axis.y);
    resultValues.append(x_axis.z);

#if defined(DEBUG)
    std::cout << "Joint:" << dag_path.fullPathName().asChar() << " : "
              << x_axis[0] << "," << x_axis[1] << "," << x_axis[2] << "\n";
#endif
  }

    setResult(resultValues);

  return status;
}

MStatus JointXAxisCmd::ParseArgs(const MArgList& args) {
  MArgDatabase arg_data(syntax(), args);
  MStringArray string_objects;
  auto status = arg_data.getObjects(string_objects);

  if (string_objects.length() > 0) {
    for (auto index = 0; index < string_objects.length(); ++index) {
      auto object_name = string_objects[index];
      status = MGlobal::getSelectionListByName(object_name, selections_);
      if (status != MS::kSuccess) {
        std::cerr << "Invalid argument.\n";
        return status;
      }
    }
  } else {
    status = MGlobal::getActiveSelectionList(selections_);
    if (status != MS::kSuccess) {
      std::cerr << "Invalid argument.\n";
      return status;
    }
  }

  return status;
}

}  // namespace maya
}  // namespace kiso