#pragma once

#include <maya/MPxCommand.h>
#include <maya/MSelectionList.h>

namespace kiso {
namespace maya {
class ExportCmd final : public MPxCommand {
 public:
  static const char* kCommandName;
  static void* CreateInstance();
  static MSyntax CreateSyntax();

  ExportCmd() {}
  ~ExportCmd() {}

  bool isUndoable() const override { return false; }
  MStatus doIt(const MArgList& args) override;

 private:
  MSelectionList selections_;
};
}
}