# SourceGen.RemoveReferences

## Goal of the Library
Legacy code has legacy dependencies. These can be hard to break from and in-place updates usually result in a big-bang release that are almost certain to introduce unexpected issues.

The focus of this will be on DTO projects that include enums and mostly POCO objects.
The ideal outcome is a consumer of this sourcegen library can create a new _project_ that has the existing DTOs copied, re-namespaced, and purged of any unwanted dependencies. This new project can be shared with clients and they can migrate on their own accord over time.

## Outstanding TODOs
1. Actually remove the unwanted dependencies in source
2. Confirm the project dependency being used to generate the DTOs can be hidden on publish
3. Make the reference removal rules configurable 
4. Setup for distribution/consumption
5. Support for more complex code? 
