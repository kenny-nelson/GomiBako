#pragma once

#include <maya/MPxCommand.h>
#include <maya/MSelectionList.h>
#include <map>

namespace kiso {
namespace maya {
class CheckMeshCmd final : public MPxCommand {
 public:
  typedef MStatus (CheckMeshCmd::*ModeFunction)();

  static const std::string kCommandName;
  static void* CreateInstance();
  static MSyntax CreateSyntax();

  explicit CheckMeshCmd();
  ~CheckMeshCmd() override {}

  bool isUndoable() const override { return false; }
  MStatus doIt(const MArgList& args) override;

 private:
  MStatus ParseArgs(const MArgList& args);

  MStatus ExecuteNotHardEdges();
  void CalculateNotHardEdges(MSelectionList* p_out_not_hard_edges,
                             const MDagPath& dag_path) const;

  std::map<std::string, ModeFunction> mode_function_map_;
  MSelectionList selections_;
  MString mode_;
  double tolerance_;
};
}
}