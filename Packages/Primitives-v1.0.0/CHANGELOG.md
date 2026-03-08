# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

## [1.0.0] - 2020-08-17
Complete rewrite of the package. This version is in no way compatible with any older version. Upgrading from a `v0.x.x` to `v1.x.x` will break!

Reiterated on the concept of shared primitives (variables, collections and events) and redefined the structure of _ScriptableObject_ wrappers considering extendability, iterative workflows and performance.

### Added
- New shared variable core
- New shared collections core
- New shared events core
- Numeric variable type implementation
- State variable type implementation
- String variable type implementation
- Ready-to-use primitives for the most common types
- Serializable variable wrappers for easier use in code
- Added readme

### Changed
- Restructured whole package
- Renamed shared primitives and their base classes
- Exposed description field in shared primitives

### Removed
- Old shared primitives
- Removed unpublish.command
- Removed publish.command

### Fixed
- Throwed exception on `GetHashCode()` calls on shared variables


## [0.2.7] - 2020-07-28
### Changed
- Removed the inheritance of the generic shared events from the non-generic base-class because it caused serialization problems and encouraged incorrect API usage.


## [0.2.6] - 2020-06-22
### Added
- Added repository to package.json

### Changed
- Updated repositories


## [0.2.5] - 2020-06-10
### Changed
- `OnChange` action on `ASharedVariable` is now only called if `SetValueWithOnChange` is used.


## [0.2.4] - 2020-06-09
### Added
- Added `OnChange` Action to `ASharedVariable`
- Added changelog
- Added unpublish.command
- Added publish.command


[Unreleased]: https://gitlab.com/koboldgames/Primitives/compare/v1.0.0...master
[1.0.0]: https://gitlab.com/koboldgames/Primitives/compare/v0.2.7...v1.0.0
[0.2.7]: https://gitlab.com/koboldgames/Primitives/compare/v0.2.6...v0.2.7
[0.2.6]: https://gitlab.com/koboldgames/Primitives/compare/v0.2.5...v0.2.6
[0.2.5]: https://gitlab.com/koboldgames/Primitives/compare/v0.2.4...v0.2.5
[0.2.4]: https://gitlab.com/koboldgames/Primitives/tree/v0.2.4
