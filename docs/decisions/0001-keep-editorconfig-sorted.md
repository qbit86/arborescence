# Keep lines within sections of EditorConfig sorted alphabetically

## Context and Problem Statement

Different projects have their EditorConfig files organized inconsistently.
The order of key-value pairs is arbitrary.
This makes it difficult to compare and merge settings.

## Decision Drivers

* Easy to compare and merge lists of key-value pairs
* Work nicely with version control systems
* Consistent and conventional look
* Easy to “normalize” the list of settings
* Easy to find desired setting

## Considered Options

* Leave the arbitrary order as-is and preserve the comments after generating EditorConfig in IDE
* Normalize EditorConfig by removing in-section comments and sorting key-value pairs alphabetically

## Decision Outcome

Chosen option: “Normalize EditorConfig by removing in-section comments and sorting key-value pairs alphabetically”, because it is easy to maintain and to come to agreement between different editors and IDEs.

The same applies for .gitignore and .gitattributes files: keep them sorted, remove groupings and comments.

### Negative Consequences

* Cannot attach relevant information in comments.

## References

* https://editorconfig.org
