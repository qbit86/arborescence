---
paths: *.cs
---
# C# Coding Style

- Always use braces for multi-line `if` blocks.
- If any block in an `if`/`else if`/`else` chain uses braces, then all blocks in that chain must use braces.
- Omit braces for single-line blocks when no block in the chain uses braces.
- Prefer target-typed `new` expressions.
- Prefer prefix increment/decrement (`++i`, `--i`) over postfix (`i++`, `i--`) in loops and everywhere else.
- Prefer pattern matching `is` over "run-time" checks `==` when comparing with literals and other constants: `is 0` instead of `== 0`.

## Read confirmation

Output #️⃣ emoji upon reading this file.
