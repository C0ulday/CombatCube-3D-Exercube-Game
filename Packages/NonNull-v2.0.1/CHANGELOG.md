# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

## [2.0.1] - 2020-06-25
### Fixed
- Silent editor crash when using members of (inherited) generic types
- Property drawer errors on 'NonNull' fields that use custom serialized types
- Accidentally filtering only for components


## [2.0.0] - 2020-06-20
Complete rewrite of the core logic of this package.

### Added
- Added repository to package.json
- Added dependency to _com.unity.settings-manager_
- Setting provider and custom package settings
- Filtering mechanism that reflects and filters types that have decorators before object and asset search starts (without blocking the editor context)
- Ability to include asset and prefab files in the search for `null` fields

### Changed
- Package identifier (name) changed to `com.koboldgames.non-null`
- Changes the packages namespace to `Koboldgames.NonNull`
- Restructured the whole package
- `UnityEngine.Object` search through `FindObjectsOfType()` as it is usually faster than a recursive scene rootobjects search
- Changed minimal Unity version compatibility to _2018.1_

### Removed
- `NonNull` namespace


## [1.0.9] - 2020-06-12
### Changed
- Improved display name


## [1.0.8]
### Changed
- Improved readme


## [1.0.7]
### Removed
- Removes development project from package


## [1.0.6]
### Changed
- Now more closely adheres to Unity packager structure conventions


## [1.0.5]
### Changed
- Updated LICENSE file


## [1.0.4]
### Fixed
- Sets minimal Unity version to _2017.4_


## [1.0.3]
### Added
- Added author information


## [1.0.2]
### Removed
- Removed obsolete release notes folder

### Fixed
- Adds _.meta_ files


## [1.0.1]
### Added
- Adds warning if `NonEmpty` is used on unsupported fields as this may break custom editors


[Unreleased]: https://gitlab.com/koboldgames/NonNull/compare/v2.0.1...master
[2.0.1]: https://gitlab.com/koboldgames/NonNull/compare/v2.0.0...v2.0.1
[2.0.0]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.9...v2.0.0
[1.0.9]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.8...v1.0.9
[1.0.8]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.7...v1.0.8
[1.0.7]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.6...v1.0.7
[1.0.6]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.5...v1.0.6
[1.0.5]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.4...v1.0.5
[1.0.4]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.3...v1.0.4
[1.0.3]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.2...v1.0.3
[1.0.2]: https://gitlab.com/koboldgames/NonNull/compare/v1.0.1...v1.0.2
[1.0.1]: https://gitlab.com/koboldgames/NonNull/tree/v1.0.1
