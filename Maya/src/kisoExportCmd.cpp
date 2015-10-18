#include "kisoExportCmd.h"

#include <maya/MArgDatabase.h>
#include <maya/MSyntax.h>

namespace kiso {
namespace maya {

const char* ExportCmd::kCommandName = "kisoExportCmd";

void* ExportCmd::CreateInstance() { return new ExportCmd; }

MSyntax ExportCmd::CreateSyntax() {
  MSyntax syntax;

  syntax.useSelectionAsDefault(true);
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

  arg_data.getObjects(selections_);

  MStatus status;
  return status;
}
}
}