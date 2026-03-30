# Changelog

All notable changes to this project are documented in this file.

## 2.0.1.0

### Changed
- Bumped package version to `2.0.1.0` in `MeetingRequest.csproj`.
- Updated NuGet packaging metadata to include `GeneratePackageOnBuild`, `PackageReadmeFile`, and ensure `LICENSE` and `README.md` are included in the package.

### Documentation
- Packaging metadata updated so `README.md` and `LICENSE` are distributed in the NuGet package.

### Notes / Migration
- No public API changes; consumers can upgrade to `2.0.1.0` without code changes. Update package references as needed.

## 2.0.0.0

### Changed
- Made string escaping helper `ReplaceForCal` null-safe and more robust (normalizes newlines, escapes backslashes/commas/semicolons).
- Replaced `DateTime` with `DateTimeOffset` for calendar date/time properties to preserve timezone/offset information:
  - `Event.StartTime` -> `DateTimeOffset`
  - `Event.EndTime` -> `DateTimeOffset?`
  - `SpecificDateTimeAlarm.SpecificDateTime` -> `DateTimeOffset`
- Clarified calendar date format constant in `FormatHelper` (UTC iCal format used for output).

### Added
- GitHub Actions workflow `.github/workflows/publish-nuget.yml` to build, pack and publish NuGet packages on merged pull requests (requires `NUGET_API_KEY` secret).

### Documentation
- Updated `README.md` (English) and license reference to `LICENSE` (MIT).

### Notes / Migration
- Changing from `DateTime` to `DateTimeOffset` affects serialized representations and consumers that relied on `DateTime` semantics. If other projects consume the package, verify compatibility and update consumers as needed.
- Recommended: add unit tests for iCalendar generation (escaping and timezone correctness) and enable nullable reference types for improved safety.

### Previous
- Initial package metadata present in `MeetingRequest.csproj` (version `2.0.0.0`).
