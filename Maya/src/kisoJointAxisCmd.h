#pragma once

#include <maya/MPxCommand.h>
#include <maya/MSelectionList.h>

namespace kiso {
namespace maya {
class JointAxisCmd final : public MPxCommand {
 public:
  static const std::string kCommandName;
  static void* CreateInstance();
  static MSyntax CreateSyntax();

  explicit JointAxisCmd()
      : is_symmetric_joint_(false), is_world_space_(false) {}
  ~JointAxisCmd() override {}

  bool isUndoable() const override { return false; }
  MStatus doIt(const MArgList& args) override;

 private:
  MStatus ParseArgs(const MArgList& args);
  MSelectionList selections_;
  bool is_symmetric_joint_;
  bool is_world_space_;
};
}
}