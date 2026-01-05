using System;
using System.Collections.Generic;
using System.Text;

namespace OldPhonePad;

public static class OldPhonePadDecoder
{
    private static readonly IReadOnlyDictionary<char, string> KeyMap = new Dictionary<char, string>
    {
        ['2'] = "ABC",
        ['3'] = "DEF",
        ['4'] = "GHI",
        ['5'] = "JKL",
        ['6'] = "MNO",
        ['7'] = "PQRS",
        ['8'] = "TUV",
        ['9'] = "WXYZ",
    };

    /// <summary>
    /// Decodes multi-tap old phone keypad input into text.
    /// Rules:
    /// - Digits 2-9 map to letters and repeated presses cycle through letters.
    /// - Space indicates a pause/commit between characters.
    /// - '*' is backspace (deletes the last output character).
    /// - '#' is send/termination; always present and ends parsing.
    /// </summary>
    public static string Decode(string input)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        var output = new StringBuilder();

        char? currentKey = null;
        int pressCount = 0;
        bool sawTerminator = false;

        void CommitBuffer()
        {
            if (currentKey is null) return;

            var key = currentKey.Value;
            if (!KeyMap.TryGetValue(key, out var letters))
                throw new ArgumentException($"Unsupported key '{key}'. Only digits 2-9 are supported.", nameof(input));

            var index = (pressCount - 1) % letters.Length;
            output.Append(letters[index]);

            currentKey = null;
            pressCount = 0;
        }

        void ApplyBackspace()
        {
            // Commit any buffered character before deleting.
            CommitBuffer();
            if (output.Length > 0) output.Length -= 1;
        }

        foreach (var ch in input)
        {
            if (ch == '#')
            {
                CommitBuffer();
                sawTerminator = true;
                break;
            }

            if (ch == ' ')
            {
                CommitBuffer();
                continue;
            }

            if (ch == '*')
            {
                ApplyBackspace();
                continue;
            }

            if (ch >= '2' && ch <= '9')
            {
                if (currentKey is null)
                {
                    currentKey = ch;
                    pressCount = 1;
                }
                else if (currentKey.Value == ch)
                {
                    pressCount += 1;
                }
                else
                {
                    CommitBuffer();
                    currentKey = ch;
                    pressCount = 1;
                }

                continue;
            }

            throw new ArgumentException($"Invalid character '{ch}' in input.", nameof(input));
        }

        if (!sawTerminator)
            throw new ArgumentException("Input must contain '#' terminator.", nameof(input));

        return output.ToString();
    }
}
