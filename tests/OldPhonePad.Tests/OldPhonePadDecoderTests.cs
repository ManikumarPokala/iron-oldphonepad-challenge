using System;
using Xunit;

namespace OldPhonePad.Tests;

public class OldPhonePadDecoderTests
{
    [Theory]
    [InlineData("33#", "E")]
    [InlineData("227*#", "B")]
    [InlineData("4433555 555666#", "HELLO")]
    [InlineData("222 2 22#", "CAB")]
    public void Decode_AssignmentExamples_ReturnExpected(string input, string expected)
    {
        var actual = global::OldPhonePad.OldPhonePadDecoder.Decode(input);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Decode_FinalExample_IsDeterministic()
    {
        // With keypad mapping and backspace rules, this decodes to "TURING".
        var actual = global::OldPhonePad.OldPhonePadDecoder.Decode("8 88777444666*664#");
        Assert.Equal("TURING", actual);
    }

    [Theory]
    [InlineData("2#", "A")]
    [InlineData("22#", "B")]
    [InlineData("222#", "C")]
    [InlineData("2222#", "A")]  // wrap-around for ABC
    [InlineData("7777#", "S")]  // PQRS
    [InlineData("77777#", "P")] // wrap-around for PQRS
    public void Decode_CyclesLettersCorrectly(string input, string expected)
    {
        var actual = global::OldPhonePad.OldPhonePadDecoder.Decode(input);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2*#", "")]
    [InlineData("**#", "")]
    [InlineData("227*#", "B")]
    public void Decode_Backspace_IsSafeAndCorrect(string input, string expected)
    {
        var actual = global::OldPhonePad.OldPhonePadDecoder.Decode(input);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("22 22#", "BB")]
    [InlineData("44   33#", "HE")] // 44->H, 33->E
    public void Decode_SpacesCommitBufferedCharacter(string input, string expected)
    {
        var actual = global::OldPhonePad.OldPhonePadDecoder.Decode(input);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Decode_ThrowsIfNoTerminator()
    {
        Assert.Throws<ArgumentException>(() => global::OldPhonePad.OldPhonePadDecoder.Decode("33"));
    }

    [Theory]
    [InlineData("1#")]
    [InlineData("0#")]
    [InlineData("A#")]
    public void Decode_ThrowsOnInvalidCharacters(string input)
    {
        Assert.Throws<ArgumentException>(() => global::OldPhonePad.OldPhonePadDecoder.Decode(input));
    }

    [Fact]
    public void Decode_ThrowsOnNull()
    {
        Assert.Throws<ArgumentNullException>(() => global::OldPhonePad.OldPhonePadDecoder.Decode(null!));
    }
}
