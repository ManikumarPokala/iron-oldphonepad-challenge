# Old Phone Pad Decoder (C#)

Production-ready solution for Iron Software's C# coding challenge: decode multi-tap input from an old phone keypad into text.

## Problem Summary
An old phone keypad maps digits to letters. Repeated presses on the same digit cycle through its letters. A space indicates a pause so the same key can be used again for the next character. `*` is backspace. `#` terminates input.

## Keypad Mapping
- 2 = ABC
- 3 = DEF
- 4 = GHI
- 5 = JKL
- 6 = MNO
- 7 = PQRS
- 8 = TUV
- 9 = WXYZ

## Rules
- Repeated digit presses select the corresponding letter (wrap-around supported).
- Space commits the current buffered character.
- `*` commits any buffered character, then deletes the last output character (safe on empty output).
- `#` commits any buffered character and ends parsing.
- Invalid characters throw `ArgumentException` (fail-fast).

## Design
The decoder maintains a buffered key run (`currentKey`, `pressCount`). On a space or a key change, it commits the buffered character. `*` commits any buffer then removes the last output character. `#` commits any buffer and stops parsing.

## How to Run
### Run tests
```bash
dotnet test
```

## Examples
- `33#` -> `E`
- `227*#` -> `B`
- `4433555 555666#` -> `HELLO`
- `222 2 22#` -> `CAB`
- `8 88777444666*664#` -> `TURING`

## AI Disclosure
Prompt details: [AI_PROMPT.md](AI_PROMPT.md)
