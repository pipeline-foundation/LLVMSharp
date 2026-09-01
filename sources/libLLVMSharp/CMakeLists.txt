include(GenerateExportHeader)

set(CMAKE_CXX_VISIBILITY_PRESET "hidden")
set(CMAKE_VISIBILITY_INLINES_HIDDEN ON)
set(CMAKE_INCLUDE_CURRENT_DIR ON)

set(SOURCES
  LLVMSharp.cpp

  LLVMSharp.h
)

set(LLVM_SEARCH_PATHS
  ${PATH_TO_LLVM}
  ${PATH_TO_LLVM}/lib/cmake
  ${PATH_TO_LLVM}/lib/cmake/llvm
  ${PATH_TO_LLVM}/lib/cmake/clang
  ${PATH_TO_LLVM}/share/clang/cmake/
  ${PATH_TO_LLVM}/share/llvm/cmake/
)

if(MSVC)
  find_package(zstd CONFIG REQUIRED)

  if(TARGET zstd::libzstd AND NOT TARGET zstd::libzstd_static)
    add_library(zstd::libzstd_static ALIAS zstd::libzstd)
  endif()
endif()

find_package(Clang REQUIRED CONFIG
             PATHS ${LLVM_SEARCH_PATHS}
             NO_DEFAULT_PATH)

# libLLVMSharp does not select any DIA-backed objects from LLVM's static libraries.
if(MSVC AND NOT TARGET DIASDK::Diaguids)
  add_library(DIASDK::Diaguids INTERFACE IMPORTED)
endif()

# Distro static zstd libraries are not position-independent.
if(UNIX AND TARGET zstd::libzstd_static)
  find_library(LLVMSHARP_ZSTD_SHARED NAMES zstd libzstd)
  if(LLVMSHARP_ZSTD_SHARED)
    set_target_properties(zstd::libzstd_static PROPERTIES IMPORTED_LOCATION "${LLVMSHARP_ZSTD_SHARED}")
  endif()
endif()

add_library(LLVMSharp SHARED ${SOURCES})

if(NOT LLVM_ENABLE_RTTI)
  if(MSVC)
    target_compile_options(LLVMSharp PRIVATE /GR-)
  else()
    target_compile_options(LLVMSharp PRIVATE -fno-rtti)
  endif()
endif()

if(APPLE)
  target_link_libraries(LLVMSharp PRIVATE LLVM)
else()
  if(WIN32)
    target_link_libraries(LLVMSharp PRIVATE LLVM-C)
  endif()

  target_link_libraries(LLVMSharp PRIVATE LLVMDemangle)
  target_link_libraries(LLVMSharp PRIVATE LLVMAggressiveInstCombine)
  target_link_libraries(LLVMSharp PRIVATE LLVMAnalysis)
  target_link_libraries(LLVMSharp PRIVATE LLVMCFGuard)
  target_link_libraries(LLVMSharp PRIVATE LLVMInstCombine)
  target_link_libraries(LLVMSharp PRIVATE LLVMInstrumentation)
  target_link_libraries(LLVMSharp PRIVATE LLVMipo)
  target_link_libraries(LLVMSharp PRIVATE LLVMJITLink)
  target_link_libraries(LLVMSharp PRIVATE LLVMObjCARCOpts)
  target_link_libraries(LLVMSharp PRIVATE LLVMOrcJIT)
  target_link_libraries(LLVMSharp PRIVATE LLVMScalarOpts)
  target_link_libraries(LLVMSharp PRIVATE LLVMTransformUtils)
  target_link_libraries(LLVMSharp PRIVATE LLVMVectorize)
endif()

if(APPLE AND EXISTS "${PATH_TO_LLVM}/lib/libLTO.dylib")
  target_link_options(LLVMSharp PRIVATE "LINKER:-lto_library,${PATH_TO_LLVM}/lib/libLTO.dylib")
endif()

target_include_directories(LLVMSharp PRIVATE ${LLVM_INCLUDE_DIRS})

set_target_properties(LLVMSharp PROPERTIES
    PREFIX lib
    VERSION ${PROJECT_VERSION}
    SOVERSION ${PROJECT_VERSION}
)

generate_export_header(LLVMSharp
    EXPORT_MACRO_NAME "LLVMSHARP_LINKAGE"
    EXPORT_FILE_NAME LLVMSharp_export.h
)

set_property(TARGET LLVMSharp APPEND PROPERTY
    PUBLIC_HEADER LLVMSharp.h "${CMAKE_CURRENT_BINARY_DIR}/LLVMSharp_export.h"
)

include(GNUInstallDirs)

install(TARGETS LLVMSharp
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBDIR} COMPONENT runtime
    ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBDIR} COMPONENT development
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR} COMPONENT runtime
    PUBLIC_HEADER DESTINATION ${CMAKE_INSTALL_INCLUDEDIR} COMPONENT development
)
