#include "kisoJointAxisCmd.h"

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

namespace {

const std::string kSpaceName("-s");
const std::string kSpaceNameLong("-space");
const std::string kSpaceWorldName("world");

const std::string kIsSymmetricJointName("-isj");
const std::string kIsSymmetricJointNameLong("-is_symmetric_joint");
}

const std::string JointAxisCmd::kCommandName("kisoJointAxis");

void* JointAxisCmd::CreateInstance() { return new JointAxisCmd(); }

MSyntax JointAxisCmd::CreateSyntax() {
  MSyntax syntax;

  syntax.addFlag(kSpaceName.c_str(), kSpaceNameLong.c_str(), MSyntax::kString);
  syntax.addFlag(kIsSymmetricJointName.c_str(),
                 kIsSymmetricJointNameLong.c_str(), MSyntax::kBoolean);

  syntax.useSelectionAsDefault(true);
  syntax.setObjectType(MSyntax::kStringObjects, 1);
  syntax.enableQuery(true);
  syntax.enableEdit(false);

  return syntax;
}

MStatus JointAxisCmd::doIt(const MArgList& args) {
  MStatus status = ParseArgs(args);
  if (status != MS::kSuccess) {
    MGlobal::displayError("Invalid argument.");
    return status;
  }

  MItSelectionList joint_list_iter(selections_);
  joint_list_iter.setFilter(MFn::kJoint);

  MDoubleArray resultValues;
  for (; !joint_list_iter.isDone(); joint_list_iter.next()) {
    MDagPath dag_path;
    joint_list_iter.getDagPath(dag_path);

    MTransformationMatrix matrix;
    if (is_world_space_) {
      matrix = dag_path.inclusiveMatrix();
    } else {
      MFnIkJoint joint(dag_path);
      double orientation[3];
      MTransformationMatrix::RotationOrder orientation_order;
      joint.getOrientation(orientation, orientation_order);
      double rotation[3];
      MTransformationMatrix::RotationOrder rotation_order;
      joint.getRotation(rotation, rotation_order);

      MTransformationMatrix matrix;

      matrix.addRotation(orientation, orientation_order, MSpace::kObject);
      matrix.addRotation(rotation, rotation_order, MSpace::kObject);
    }

    MVector x_axis(1.0, 0.0, 0.0);
    MVector y_axis(0.0, 1.0, 0.0);
    MVector z_axis(0.0, 0.0, 1.0);

    x_axis *= matrix.asMatrix();
    y_axis *= matrix.asMatrix();
    z_axis *= matrix.asMatrix();
    x_axis.normalize();
    y_axis.normalize();
    z_axis.normalize();

    if (is_symmetric_joint_) {
      if (is_world_space_) {
        x_axis[0] = -x_axis[0];
        y_axis[0] = -y_axis[0];

        z_axis[1] = -z_axis[1];
        z_axis[2] = -z_axis[2];
      } else {
        x_axis[2] = -x_axis[2];
        y_axis[2] = -y_axis[2];

        z_axis[0] = -z_axis[0];
        z_axis[1] = -z_axis[1];
      }
    }

    for (int i = 0; i < 3; ++i) {
      resultValues.append(x_axis[i]);
    }

    for (int i = 0; i < 3; ++i) {
      resultValues.append(y_axis[i]);
    }

    for (int i = 0; i < 3; ++i) {
      resultValues.append(z_axis[i]);
    }

#if defined(DEBUG)
    MString message("Joint: ");
    message += "[";
    message += x_axis[0];
    message += ", ";
    message += x_axis[1];
    message += ", ";
    message += x_axis[2];
    message += "] ";

    message += "[";
    message += y_axis[0];
    message += ", ";
    message += y_axis[1];
    message += ", ";
    message += y_axis[2];
    message += "] ";

    message += "[";
    message += z_axis[0];
    message += ", ";
    message += z_axis[1];
    message += ", ";
    message += z_axis[2];
    message += "] ";
    message += dag_path.fullPathName();
#endif
    break;
  }

  setResult(resultValues);

  return status;
}

MStatus JointAxisCmd::ParseArgs(const MArgList& args) {
  MArgDatabase arg_data(syntax(), args);

  if (arg_data.isFlagSet(kSpaceName.c_str())) {
    MString space_name;
    auto status = arg_data.getFlagArgument(kSpaceName.c_str(), 0, space_name);
    if (status != MS::kSuccess) {
      MGlobal::displayError("Invalid argument.");
      return status;
    }

    if (space_name == kSpaceWorldName.c_str()) {
      is_world_space_ = true;
    }
  }

  if (arg_data.isFlagSet(kIsSymmetricJointName.c_str())) {
    bool is_symmetric_joint;
    auto status = arg_data.getFlagArgument(kIsSymmetricJointName.c_str(), 0,
                                           is_symmetric_joint);

    if (status != MS::kSuccess) {
      MGlobal::displayError("Invalid argument.");
      return status;
    }

    is_symmetric_joint_ = is_symmetric_joint;
  }

  {
    MStringArray string_objects;
    auto status = arg_data.getObjects(string_objects);

    if (string_objects.length() > 0) {
      for (auto index = 0; index < string_objects.length(); ++index) {
        auto object_name = string_objects[index];
        status = MGlobal::getSelectionListByName(object_name, selections_);
        if (status != MS::kSuccess) {
          MGlobal::displayError("Invalid argument.");
          return status;
        }
      }
    } else {
      status = MGlobal::getActiveSelectionList(selections_);
      if (status != MS::kSuccess) {
        MGlobal::displayError("Invalid argument.");
        return status;
      }
    }
  }

  return MS::kSuccess;
}

}  // namespace maya
}  // namespace kiso