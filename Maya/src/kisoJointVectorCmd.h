#pragma once

#include <maya/MPxCommand.h>
#include <maya/MDagPath.h>

namespace kiso {
namespace maya {
class JointVectorCmd final : public MPxCommand {
 public:
  static const std::string kCommandName;
  static void* CreateInstance();
  static MSyntax CreateSyntax();

  explicit JointVectorCmd() : is_world_space_(false) {}
  ~JointVectorCmd() override {}

  bool isUndoable() const override { return false; }
  MStatus doIt(const MArgList& args) override;

 private:
  MStatus ParseArgs(const MArgList& args);
  MDagPath parent_path_;
  MDagPath child_path_;
  bool is_world_space_;
};
}
}