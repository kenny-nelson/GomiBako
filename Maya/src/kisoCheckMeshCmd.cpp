#include "kisoCheckMeshCmd.h"

#include <maya/MArgDatabase.h>
#include <maya/MSyntax.h>
#include <maya/MItSelectionList.h>
#include <maya/MDagPath.h>
#include <maya/MVector.h>
#include <maya/MGlobal.h>
#include <maya/MFnMesh.h>
#include <maya/MItMeshEdge.h>
#include <maya/MFloatVectorArray.h>

#include <set>

namespace kiso {
namespace maya {

namespace {

const std::string kToleranceName("-t");
const std::string kToleranceNameLong("-tolerance");

const std::string kModeName("-m");
const std::string kModeNameLong("-mode");

const std::string kModeNotHardEdgeName("not_hard_edge");
}

const std::string CheckMeshCmd::kCommandName("kisoCheckMesh");

void* CheckMeshCmd::CreateInstance() { return new CheckMeshCmd(); }

MSyntax CheckMeshCmd::CreateSyntax() {
  MSyntax syntax;

  syntax.addFlag(kToleranceName.c_str(), kToleranceNameLong.c_str(),
                 MSyntax::kDouble);

  syntax.addFlag(kModeName.c_str(), kModeNameLong.c_str(), MSyntax::kString);

  syntax.useSelectionAsDefault(true);
  syntax.setObjectType(MSyntax::kStringObjects, 1);
  syntax.enableQuery(true);
  syntax.enableEdit(false);

  return syntax;
}

CheckMeshCmd::CheckMeshCmd() : tolerance_(0.000001) {
  mode_function_map_.insert(
      std::make_pair(kModeNotHardEdgeName, &CheckMeshCmd::ExecuteNotHardEdges));
}

MStatus CheckMeshCmd::doIt(const MArgList& args) {
  MStatus status = ParseArgs(args);
  if (status != MS::kSuccess) {
    std::cerr << "Invalid argument.\n";
    return status;
  }

  auto mode_function = mode_function_map_[mode_.asChar()];
  return (this->*mode_function)();
}

MStatus CheckMeshCmd::ParseArgs(const MArgList& args) {
  MArgDatabase arg_data(syntax(), args);

  {
    if (arg_data.isFlagSet(kToleranceName.c_str())) {
      auto status =
          arg_data.getFlagArgument(kToleranceName.c_str(), 0, tolerance_);
      if (status != MS::kSuccess) {
        std::cerr << "Invalid argument.\n";
        return status;
      }
    }
  }

  {
    if (arg_data.isFlagSet(kModeName.c_str())) {
      auto status = arg_data.getFlagArgument(kModeName.c_str(), 0, mode_);
      if (status != MS::kSuccess) {
        std::cerr << "Invalid argument.\n";
        return status;
      }
    } else {
      return MS::kInvalidParameter;
    }
  }

  {
    if (mode_function_map_.find(mode_.asChar()) == mode_function_map_.end()) {
      return MS::kInvalidParameter;
    }
  }

  {
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
  }

  return MS::kSuccess;
}

MStatus CheckMeshCmd::ExecuteNotHardEdges() {
  MItSelectionList mesh_list_iter(selections_);
  mesh_list_iter.setFilter(MFn::kMesh);

  MSelectionList not_hard_edges_list;
  for (; !mesh_list_iter.isDone(); mesh_list_iter.next()) {
    MDagPath dag_path;
    mesh_list_iter.getDagPath(dag_path);
    CalculateNotHardEdges(&not_hard_edges_list, dag_path);
  }

  MStringArray results;
  not_hard_edges_list.getSelectionStrings(results);
  setResult(results);

  return MS::kSuccess;
}

void CheckMeshCmd::CalculateNotHardEdges(MSelectionList* p_out_not_hard_edges,
                                         const MDagPath& dag_path) const {
#if defined(DEBUG)
  std::cout << "Mesh:" << dag_path.fullPathName().asChar() << "\n";
#endif
  MFnMesh mesh(dag_path);

  MFloatVectorArray normals;
  mesh.getNormals(normals);

  MFloatVectorArray cleanup_normals;
  MIntArray cleanup_normal_ids;

  cleanup_normals.append(normals[0]);
  auto normal_count = normals.length();
  for (unsigned int normal_index = 0; normal_index < normal_count;
       ++normal_index) {
    auto& normal = normals[normal_index];

    auto cleanup_normal_count = cleanup_normals.length();
    bool has_cleanup_normal_index = false;
    for (unsigned int cleanup_normal_index = 0;
         cleanup_normal_index < cleanup_normal_count; ++cleanup_normal_index) {
      auto& cleanup_normal = cleanup_normals[cleanup_normal_index];
      if (normal.isEquivalent(cleanup_normal, static_cast<float>(tolerance_))) {
        cleanup_normal_ids.append(cleanup_normal_index);
        has_cleanup_normal_index = true;
        break;
      }
    }

    if (!has_cleanup_normal_index) {
      cleanup_normals.append(normal);
      cleanup_normal_ids.append(cleanup_normals.length() - 1);
    }
  }

  MItMeshEdge mesh_edge_iter(dag_path);
  for (; !mesh_edge_iter.isDone(); mesh_edge_iter.next()) {
    auto edge_index = mesh_edge_iter.index();
    int2 edge_vertex_indices;
    mesh.getEdgeVertices(edge_index, edge_vertex_indices);

    MIntArray connected_face_indices;
    mesh_edge_iter.getConnectedFaces(connected_face_indices);

    auto& edge_start_index = edge_vertex_indices[0];
    auto& edge_end_index = edge_vertex_indices[1];
    std::set<int> edge_start_normal_ids;
    std::set<int> edge_end_normal_ids;
    auto connected_face_count = connected_face_indices.length();
    if (connected_face_count == 1) {
      continue;
    }

    for (unsigned int connected_face_index = 0;
         connected_face_index < connected_face_count; ++connected_face_index) {
      auto& face_index = connected_face_indices[connected_face_index];

      MIntArray face_vertex_indices;
      mesh.getPolygonVertices(face_index, face_vertex_indices);

      MIntArray face_normal_indices;
      mesh.getFaceNormalIds(face_index, face_normal_indices);

      auto local_vertex_count = face_vertex_indices.length();
      bool has_edge_start_normal_index = false;
      bool has_edge_end_normal_index = false;
      for (unsigned int local_vertex_index = 0;
           local_vertex_index < local_vertex_count; ++local_vertex_index) {
        auto& global_vertex_index = face_vertex_indices[local_vertex_index];
        auto& global_normal_index = face_normal_indices[local_vertex_index];
        auto& cleanup_normal_index = cleanup_normal_ids[global_normal_index];

        if (edge_start_index == global_vertex_index) {
          edge_start_normal_ids.insert(cleanup_normal_index);
          has_edge_start_normal_index = true;
        } else if (edge_end_index == global_vertex_index) {
          edge_end_normal_ids.insert(cleanup_normal_index);
          has_edge_end_normal_index = true;
        }

        if (has_edge_start_normal_index && has_edge_end_normal_index) {
          break;
        }
      }
    }

    if (edge_start_normal_ids.size() == 1 && edge_end_normal_ids.size() == 1 &&
        !mesh_edge_iter.isSmooth()) {
      p_out_not_hard_edges->add(dag_path, mesh_edge_iter.edge());
    }
  }

  std::cout << "Cleanup Normals: " << cleanup_normals.length() << "\n";
}

}  // namespace maya
}  // namespace kiso