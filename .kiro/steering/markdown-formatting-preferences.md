---
inclusion: fileMatch
fileMatchPattern: "*.md,*.mdc"
---
# Markdown Formatting Preferences

When generating Markdown content, please adhere to the following formatting rules.

## List Item Markers

Use hyphens (`-`) for unordered list items instead of asterisks (`*`).

## Spacing After Markers

Ensure there is exactly one space after the list item marker (hyphen or number) and before the content.
Avoid extra spaces.

✔️ Correct:

```markdown
- First item
- Second item

1. Numbered item one
2. Numbered item two
```

❌ Incorrect:

```markdown
*   First item (uses asterisk and extra spaces)
*   Second item

1.  Numbered item one (extra spaces)
2.  Numbered item two
```

## Header Spacing

Ensure there is only one single empty line immediately following any header line (`#`, `##`, `###`, etc.).

✔️ Correct:

```markdown
# Main Title

Some text here.

## Subheading

More text.
```

❌ Incorrect:

```markdown
# Main Title
Some text here. (Missing empty line)

## Subheading
More text. (Missing empty line)
```

❌ Incorrect:

```markdown
# Main Title


Some text here. (Two empty lines instead of one)

## Subheading


More text. (Two empty lines instead of one)
```

## New Sentence Line

Each sentence should start from a new line.

## Italics/Emphasis

Use single underscores (`_text_`) for italics/emphasis instead of single asterisks (`*text*`).
Keep double asterisks (`**text**`) for bold formatting intact - do not change them to underscores.

✔️ Correct:

```markdown
This is _correct emphasis_.
This is **correct bold formatting**.
This text has both _emphasis_ and **bold** formatting.
```

❌ Incorrect:

```markdown
This is *incorrect emphasis*.
```

## List Item and Continuation Line Indentation

- Use 4 spaces to indent nested list items relative to their parent item.
- Continuation lines for _any_ list item (top-level or nested) should be indented by an _additional_ 4 spaces relative to the indentation level of the list item itself.
    - For example:
        - If a top-level list item (e.g., `1.` or `-`) starts at column 0, its continuation line must start at column 4.
        - If this top-level item has a nested item, that nested item will start at column 4 (indented by 4 spaces relative to its parent).
        - A continuation line for _that_ nested item would then start at column 8 (the nested item's 4-space indent + 4 additional spaces for its continuation).

✔️ Correct:

```markdown
- Top-level list item that is long enough to span
    multiple lines for demonstration purposes.
    This is its continuation line, correctly indented by 4 spaces.
- Another top-level item.
    - Nested item (indented by 4 spaces relative to its parent)
        that also spans multiple lines.
        Continuation of nested item (indented by an additional 4 spaces,
        making it 8 spaces from the original line start).
    - Another nested item.
        This is its first line of continuation.
        This is its second line of continuation.
1. Numbered top-level list item.
    Its continuation line is here.
    - A nested item under the numbered list.
        And its continuation line.
```

❌ Incorrect (Example 1: Top-level continuation line incorrect indent):

```markdown
1. Here goes list item.
   It's continuation indented by only three spaces.

- Another top-level item.
  This continuation line is not indented by 4 spaces.
```

❌ Incorrect (Example 2: Insufficient nested indent):

```markdown
- Top-level item
  - Nested item (only 2 spaces indent)
```

❌ Incorrect (Example 3: Nested continuation line not further indented by 4 spaces):

```markdown
- Top-level item
    - Nested item (correct 4 spaces indent)
      Continuation of nested item (incorrectly aligned with text, should be 4 more spaces in)
```
