# Configure line endigs in a .gitattributes file

## Context and Problem Statement

To avoid problems in your diffs, you can configure either IDE or Git to properly handle line endings, so you can collaborate effectively with people who use different operating systems.

## Decision Drivers

* Cross-platform editing with different native line endings
* Configuration should apply automatically by cloning repository

## Considered Options

* `end_of_line` property in an EditorConfig file
* `* text=auto` entry in a .gitattributes file
* Just rely on a developer to set up their IDE

## Decision Outcome

Chosen option: “`* text=auto` entry in a .gitattributes file”, because this doesn't depend on EoL-settings on the developer's machine, even if they are somewhat enforced using the EditorConfig file.

### Positive Consequences

* All text files have native line endings after check-out without any friction between collaborators on different operating systems.

## References

* https://docs.github.com/en/free-pro-team@latest/github/using-git/configuring-git-to-handle-line-endings
* https://editorconfig.org
