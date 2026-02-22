# Changelog

All notable changes to this project are documented in this file.

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
