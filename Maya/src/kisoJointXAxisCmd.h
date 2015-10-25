#pragma once

#include <maya/MPxCommand.h>
#include <maya/MSelectionList.h>

namespace kiso {
namespace maya {
class JointXAxisCmd final : public MPxCommand {
 public:
  static const std::string kCommandName;
  static void* CreateInstance();
  static MSyntax CreateSyntax();

  explicit JointXAxisCmd() {}
  ~JointXAxisCmd() override {}

  bool isUndoable() const override { return false; }
  MStatus doIt(const MArgList& args) override;

 private:
  MStatus ParseArgs(const MArgList& args);
  MSelectionList selections_;
};
}
}