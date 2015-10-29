#include "kisoJointVectorCmd.h"

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

const std::string kParentJointNodeName("-pjn");
const std::string kParentJointNodeNameLong("-parent_joint_node");

const std::string kChildJointNodeName("-cjn");
const std::string kChildJointNodeNameLong("-child_joint_node");
}

const std::string JointVectorCmd::kCommandName("kisoJointVector");

void* JointVectorCmd::CreateInstance() { return new JointVectorCmd(); }

MSyntax JointVectorCmd::CreateSyntax() {
  MSyntax syntax;

  syntax.addFlag(kSpaceName.c_str(), kSpaceNameLong.c_str(), MSyntax::kString);
  syntax.addFlag(kParentJointNodeName.c_str(), kParentJointNodeNameLong.c_str(),
                 MSyntax::kString);
  syntax.addFlag(kChildJointNodeName.c_str(), kChildJointNodeNameLong.c_str(),
                 MSyntax::kString);

  syntax.enableQuery(true);
  syntax.enableEdit(false);

  return syntax;
}

MStatus JointVectorCmd::doIt(const MArgList& args) {
  MStatus status = ParseArgs(args);
  if (status != MS::kSuccess) {
    MGlobal::displayError("Invalid Argument.");
    return status;
  }

  MFnIkJoint parent_joint(parent_path_);
  MFnIkJoint child_joint(child_path_);

  MVector result_vector;
  if (is_world_space_) {
    auto translation = parent_joint.getTranslation(MSpace::kWorld);
    auto child_translation = child_joint.getTranslation(MSpace::kWorld);
    result_vector = child_translation - translation;
  } else {
    double orientation[3];
    MTransformationMatrix::RotationOrder orientation_order;
    parent_joint.getOrientation(orientation, orientation_order);
    double rotation[3];
    MTransformationMatrix::RotationOrder rotation_order;
    parent_joint.getRotation(rotation, rotation_order);

    MTransformationMatrix matrix;
    matrix.addRotation(orientation, orientation_order, MSpace::kTransform);
    matrix.addRotation(rotation, rotation_order, MSpace::kTransform);

    auto child_translation = child_joint.getTranslation(MSpace::kTransform);

    child_translation *= matrix.asMatrix();
    result_vector = child_translation;
  }

  result_vector.normalize();

  MDoubleArray result_values;
  result_values.append(result_vector[0]);
  result_values.append(result_vector[1]);
  result_values.append(result_vector[2]);

  setResult(result_values);

  return MS::kSuccess;
}

MStatus JointVectorCmd::ParseArgs(const MArgList& args) {
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

  if (arg_data.isFlagSet(kParentJointNodeName.c_str())) {
    MString name;
    auto status =
        arg_data.getFlagArgument(kParentJointNodeName.c_str(), 0, name);

    if (status != MS::kSuccess) {
      MGlobal::displayError("Invalid Argument.");
      return status;
    }
    MSelectionList selections;
    status = MGlobal::getSelectionListByName(name, selections);
    if (status != MS::kSuccess) {
      MGlobal::displayError("Parent Joint Node Not Exist.");
      return MS::kNotFound;
    }

    MDagPath dag_path;
    status = selections.getDagPath(0, dag_path);
    if (status != MS::kSuccess) {
      MGlobal::displayError("Parent Joint Node Not Found.");
      return MS::kNotFound;
    }

    parent_path_ = dag_path;
  }

  if (arg_data.isFlagSet(kChildJointNodeName.c_str())) {
    MString name;
    auto status =
        arg_data.getFlagArgument(kChildJointNodeName.c_str(), 0, name);

    if (status != MS::kSuccess) {
      MGlobal::displayError("Invalid Argument.");
      return status;
    }
    MSelectionList selections;
    status = MGlobal::getSelectionListByName(name, selections);
    if (status != MS::kSuccess) {
      MGlobal::displayError("Child Joint Node Not Exist.");
      return MS::kNotFound;
    }

    MDagPath dag_path;
    status = selections.getDagPath(0, dag_path);
    if (status != MS::kSuccess) {
      MGlobal::displayError("Child Joint Node Not Found.");
      return MS::kNotFound;
    }

    child_path_ = dag_path;
  }

  if (!parent_path_.isValid() || !child_path_.isValid()) {
    MGlobal::displayError("Invalid Arguments.");
    return MS::kInvalidParameter;
  }

  bool has_child_node = false;
  auto child_count = parent_path_.childCount();
  for (unsigned int child_index = 0; child_index < child_count; ++child_index) {
    auto child = parent_path_.child(child_index);
    if (child == child_path_.node()) {
      has_child_node = true;
      break;
    }
  }

  if (!has_child_node) {
    MGlobal::displayError("Child Joint Node Not Found.");
    return MS::kInvalidParameter;
  }

  return MS::kSuccess;
}

}  // namespace maya
}  // namespace kiso