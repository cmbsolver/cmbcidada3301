using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Utility.Character;
using Xunit;

namespace LiberPrimusAnalysisTool.Application.Tests;

public class TransposeRuneToLatinTests
{
    // Page Tests
    [Fact]
    public async Task Do_All_Runes_Translate()
    {
        // Arrange
        var originalRunes = "ᛝᛟᛇᛡᛠᚫᚦᚠᚢᚩᚱᚳᚷᚹᚻᚾᛁᛄᛈᛉᛋᛏᛒᛖᛗᛚᛞᚪᚣ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);

        // Assert
        foreach (var character in latinResult)
        {
            var xcahr = character.ToString();
            var assertion = !characterRepo.IsRune(xcahr, true);
            Assert.True(assertion);
        }
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes01()
    {
        // Arrange
        var originalRunes = "ᚪᛁᛗᛋᚾ•ᛋᛟᚱᚢᚹᛋᛚᛡ⊹ᛟᚪᚫᛝᛋᛞᛈᛏ•ᚳᚱᚦᛡ•ᚱᛒᚩᛞᚦᚠ•ᚣᛉᛁᛏ⊹ᛟᛁ•ᚠᛚᚩ•ᚠᛠ•ᚱᚩᛟᛗᚻᛗᚷᛈᚻ•ᚫᚻᚾᚩᚻᚣ•ᛟᛋᛚ•ᚾᚷ•ᚫᚣ•ᛟᚳᛒᛚᛄ•ᛝᛚᛟ•ᚫᛄᛠᚹ•ᛠᚦᚩ•ᛒᛟᚣ•ᚳᚠᚳᛄ•ᛚᚫ⊹ᚾ•ᚦᛈ•ᚢᛉ•ᛟᛉᚷ•ᛈᚠᛋᛇᚫᛟ•ᛝᛈᛇᚩᛖᚪ•ᚷᚫᛡᛝᚦᚩ•ᛈᚪᛟᚦᚱᛝᚫ•ᚳᛋᛒᛇᚣᚻ•ᛏᛉᛖᛚᚱ⊹ᚷᚹᚣ•ᛄᚠᛁᚾᛡᚳᚣᛠᛁᛡ•ᚩᚦ•ᛖᚳᚫᚳᛉᛡᛠ•ᚩᛚᚳ•ᚠᚱᛞᛝᛖᚢ•ᛞᚳᛚᛠᛋᛉᚳᚷᛡ⊹ᚹᛋᚦ•ᚠᛞᛝ•ᛁᛡᛗᚪᚫᚷ•ᚹᛋ•ᚾᛞ•ᚳᛈᚦᛉᛈᛠᛠ•ᚹᚢ•ᛠᚹ•ᚠᚹᛄᚣ•ᛉᛞᚹᚳᚷᚳᛟ•ᛞᛉᛟ•ᚱᛡᚷ•ᚾᛈᚪᚣᛈ•ᚳᚣᚻ•ᚠᛖᛄᛠᚾ•ᛟᚫ•ᚢᚪ•ᚻᚱ•ᛖᛠᚦᚠᛄᚪ•ᛚᛉᛋᛏ•ᛗᚠᛚᚠᛏ•ᚷᛁᚦ•ᚢᛚᚷ•ᛉᛠᛏᛋᛚᛄᛈ•ᛚᛉᛁ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes02()
    {
        // Arrange
        var originalRunes = "ᛟᛗ•ᚢ⊹ᚻᛏ•ᛒᛇᛚᛞᚻᛒᛗ•ᛠᚱᛒ•ᚾᚻᛒᛖᚷᛇ•ᛞᛚᚹᛇᛡᛈᚩ•ᚻᛖᛠ•ᚹᛁᚱᛁᚻ•ᚢᚦᚻᚣ•ᚾᛉᛒᚷᛄᛈᚢ•ᛝᛠᚠᚾᛁᛖᛞᛡᛝᚱ•ᛞᛒᛄᛡᛟᛗᛁ•ᚠᛏ•ᛄᛞᛁᚦᚱᛚᛋ•ᛖᛇᚩᚷᛒᛏᛞ•ᚦᚪᚾᚳᚣ•ᛡᛋᚦᛞ•ᛝᚠᛚᛖᚷᚻᚳ•ᛖᚩᛁᛏᚾᛉ•ᛈᛏᚠᚻᚱᛞᛖᚠᛏ•ᚫᚹᚻ•ᛒᚳ•ᚠ•ᛈᚪᛚᚢᛠᚾᛚᛄ•ᛄᚳᛚᚹᛠᛞᚢᛞᛇ•ᛠᛉᛞᚹᚻᛠ•ᚦᛡᚫᚳᛚᛏᚹᛖᛁᚳ•ᛈᛟᛞᚳ•ᚾᚻᚪ•ᚱᛁᚷᚦᛠᛖᛏᚷ•ᚦᚻᚩᛡᚹᚫᛄᛖ•ᛝᛠᛞ•ᚩᚫ•ᚪᛚ•ᛒᛄᚳᚢᛉᛏᚪᛒᛄᛈ•ᚠᛠ•ᚻᛞᚾᛡᚢᛈᛋᚢᚹ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes03A()
    {
        // Arrange
        var originalRunes = "ᛚᛄ•ᛇᚻᛝᚳᚦᛏᚫᛄᛏᛉᚻ•ᛏᚢᛟ␍ᛋᛈᚱᚷ•ᚣᚾᚪᚷᛇᛝᚾ•ᚹᚠᚣᚾᛒᛠᛡ•ᛈᚾᚣᚪᛋᛗᛒ•ᛡᛠᛡᛁ•ᚩᛒᚱᚾᛚᛠ•ᚱᛚᛚᛖᛒᚹᚾᚻᛗᚠᛟᛒ•ᛝ•ᚱᚪᛡᚷᛟᛇᛏᛗᛉ•ᛞᛇ•ᛗᚣᚻᛠ•ᛁᛚᛋ•ᚾᚹᚳᚠᛈᛗᛈᛚ•ᛠᛋᚦᚠᛟᛡ•ᚦᛖᚣ•ᚳᛄᛚᚳᛡᛗ•ᛒᛞᚳᛇ•ᛄᛁᛏᛟ•ᛞᛠᛖᛡᚾᛏ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes03B()
    {
        // Arrange
        var originalRunes = "ᛈᛞᚦ␍ᛇᛞᛇ•ᚫᛚᚳ•ᛡᛇ•ᛠᚻ•ᚹᛗᚣᚦᚢ•ᚻᛏᚦᚱᚻᛝ•ᛚᛝᛋ•ᚾᚫᛠᚷᛋᛚ•ᛋᛉᚩᚻᚹᛞᛗᛖᛗᚪᚠ•ᚳᚣᚳᚫᚾ•ᛏᚦᚷ•ᛁᛄᛁ•ᚳᛞᛡᛉ•ᚻᚫᚫᛠᚷ•ᛠᛝ•ᚠᛏᚩᚱᛞᚳᛇ•ᚠᚢᛉᛠᛒᚩ•ᛉᛁᚣᚷᛋᛋᛒᛠ•ᚩᛁᛈ•ᛁᛄᛁᚩᛖ•ᚻᛠᚻ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes04()
    {
        // Arrange
        var originalRunes = "ᛚᛡ•ᚣᛈᛉᛁᚹᛗᚳᛁ•ᛚᚷᚠᚾᛡᚳᛉ•ᛈᚩᚱᛡ•ᚻ•ᛄᛗ•ᛟᛉᛝ•ᚢᛗᛇᛠᚷᛝ•ᛝᚹᚳ•ᛚᛝᚢ•ᛉᛄᚠᛟᚢ•ᚷᛠ•ᛗᛉ•ᚪᚹ•ᛚᚢᛉᚫ•ᛗᛞᛝᚻᚱᚣ•ᚻᚪ•ᚷᛁᚠᚷᚳ•ᚫᛝᛄᛇᛉᛡ•ᚾᚦᛒᚢᛄᚱ⊹ᚹ•ᚷᛚᛟᚷ•ᚦᛇᚠ•ᚦᛠᛁ•ᛋᚷ•ᚷᚣ•ᛠᛡᛈ•ᛡᚫᛚ⊹ᚦᛠᛉᚫ•ᛖᛗᛖᛏᛟᛏ•ᛠᚳᚠ•ᚳᛠᚷ•ᚦ•ᛈᛁᚳᚾ⊹ᛇᚣᛝᛄᛝᛗᚹᚳᚾ•ᛒᚣᛠ•ᚩᛟᚷᚱ•ᛗᚱᛗᛈᛡᚹ•ᚫᛟᚦᛟ•ᛈᛉᛄᛚ•ᚱᛚᚱᛒᚪᛈᛏᛉᛚᛏ•ᛗᛉᛁ'ᚹ⊹ᛄᛋᛟᛗᚾᚱᛖᛒᛋ•ᚳᛏᛚᛟ•ᛋᛒᚠᛉᚦᚪᛠᚢ•ᛇᛉ•ᚱᚷᛏᛇᛠᛁᛄᛒᛟ•ᛉᚷᛄᛝ•ᛠᚦ•ᚱᛝᛒ•ᚾᚢᚪᛝᛒᛈᛋᛠ•ᛈᚹᚩᚻᛖ•ᚫᛇᚷᚾᚫᛋᛇ•ᚩᛈᛗ•ᛖᛉᛡᛒᚹ•ᚢᛖᛁᛞ•ᛈᚪᛇᚷᛋᚳᚷᛞᛈᚣ•ᛡᛚᚦᚱ•ᚳᚢᚠᛇᚦ•ᛉᛖᛚ•ᚢ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes05()
    {
        // Arrange
        var originalRunes = "ᚱᚫ•ᛉᚻᛄᚫᛗᛚᚠ•ᚳᛝᛞ•ᛁᛝᚩ•ᚳᛋᛟᛖᚣᛟᚻᚢ•ᚷᛞᚹᚪ•ᛖᛋᚷᛝᚠᛉ•ᛞᛉᛄ•ᛠᚻᛁ•ᚦᛈᛉᚣ•ᛡ•ᛇᛞᛇᛝᛇᛝ•ᛖᛠᛞᚱ•ᛚᛇᛏ•ᛉᛏᚣ•ᚱᛇ•ᛈᛝᛇᛈᚩᛁᛚᛖᚠ•ᛇᚫᚪ•ᚣᛝᚠᚣ•ᚠᛞᚾᛚ•ᛉᛏᚾᚫᛋ⊹ᛁᚩᚳᚢ•ᚣᛠᚾᛏᚷᚳᚪ•ᛉᛡᛇ•ᚦᛄᚣᛄᛚᛟᛖᛚ•ᚣ⊹ᛈᛡ•ᛖᚹᛟ•ᛇᚾᚪ•ᚻᛞᛇᛋ•ᚦᚣᛇᚦᛄᚦᚱᚢ•ᚳᛠᚪ•ᚢᛄᛡᛈ•ᚣᚫᛇᛋ•ᚻᛠᛏᚣᛞᚣᚫᚠᚻᚩ•ᛟᛗᛉᛟᛄᚷ•ᚢᛡᚱᛡᚳ•ᛁᚠᛟ•ᛁᛄᛈᛒ•ᛖᛝᚣᚦᚩᚫᚣ•ᛠᛉᛡᛖᛚ•ᛁᚱᚣᛞᛠᛄ•ᚫᚳ•ᛗᚷᛁᚫᚢᚪᚫ•ᛄᚪᚻᛈ•ᚠᛞᛚᛁᛠᛈᛟᚣᚩ•ᚢᛒᚷᛝᛟᚢᛝᛋᚢᚳ•ᛏᛞ⊹ᚫᛈᚩᛄ•ᛒᚻᚱᛁᚷᚻᛄ•ᚣᚹᛗᛇᚾᚫ•ᛞᛝᛇ•ᛟᛄᛝᚳᛖᛠ•ᛉᚪᚱᚣ•ᚪᚢᛏ•ᚳᛈᚳ•ᚩᛇᛟ•ᚫᛈ•ᛏ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes06A()
    {
        // Arrange
        var originalRunes = "ᛉᚳᛏᚻᛞᛇ•ᛉᛒᛠ•ᚫᚾᛄ•ᛠᚪᛒ•ᛖᛠᚹ•ᛡᛚ•ᚹᛁᛡᛋᛈᛚᚦᚪᛋᛄ•ᛡᛞᚣᚱᛞᛟ•ᚦᚱᛉᛟᚹ•ᚣᛞᛏ•ᚷᛚᛡᚻᚹᛗᚱ•ᛝᚠᚳ•ᚱᚫᛁᛒᚷᛈᚣ•ᛞᚪᚱᚪᛉᛟ•ᚢᚩᛁᛠ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes06B()
    {
        // Arrange
        var originalRunes = "ᚪᛏᛉᛒ•ᛗ•ᚷᛡᛋᛒ•ᛉᛇ␍ᚷᚾᛠᚫᚷᛝᛞ•ᛉᛖᛏᚩᚷᛡ•ᛝᚻᛏ•ᚳᛁᚣ•ᛄᛏ•ᛟᚩᚻᚱᛄ•ᚳᛖᛡᚩ•ᛞᚪᛏᚣᚢᚾᚱᛇ•ᚫᚫᛁᛖᚠᛝᚦᚻ•ᛉᛁᛟᛋᛁ•ᛗᚪ•ᚢᛄᚳᛋᚹᚾᚣ•ᚩᛈᛉᚱ•ᛚᚫᛟᛏᛡ•ᛄᛈᛗ•ᛞᛋᚠᛗ•ᛟᚹ•ᛞᛚᛏ•ᚷᚱ•ᚩᚢᛋᚻᚪ•ᚣᛇᛡᛚᚢᚻ•ᛈᚹᛄᛚᚷᛒ•ᛗᚢᛄᛗ•ᛇᚾᛇ⊹\"ᚫᛚᚪᛚᚷᚪ•ᛋ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes07()
    {
        // Arrange
        var originalRunes = "ᚻᛝ•ᛚᚦᛒ•ᛋᚳᚢᚳᚩᛡ\"•ᛚᚳᛄ•ᛉᚪᚾᛇᛉ⊹ᛠᛗᛈᚢ•ᛗᚠᛚᛠᛝ•ᛒᛉᛁ•ᛚᚦᚱ•ᛠᛡᛁᚳ•ᚩᛉᛖᛞᛡ•ᛏᛋᛗᛠᛄᛈ•ᛠᛟ•ᛡᚫᚦᚹᚻᛈᛇᚪᚷᛈᚻᛠ•ᚳᛚᛠᛈ•ᛡᚣᚾᛁ•ᛚᛡᛁᚳ•ᚫᛇᚾ•ᚫᚳᛡᚱᛡᛚᛞ•ᛒᛟᛝᛡ•ᛉᛗᛝ•ᚳᚻᛟᛠᚾᛈᚳᚦ•ᛁᛇᚦ•ᛇᚢᚩ•ᚦᛈᚪ•ᛡᛚᛟᚹᛡᛈ•ᛄᛗ•ᚷᛒᛈᛋᚾᛇ•ᛏᚩᚷᚢᚾᚫᛖ•ᚾᚣᛁᛖ•ᛞᛝ•ᛞᛝ•ᛚᚢᛚᛉ•ᚪᚾᛝ•ᛇᚪᛄ•ᚻ•ᛞᚹᛈᚫᚹᚫ•ᛇᛁᛚᛝ•ᚦᚾᚳ⊹ᛒᛁᛏ•ᛠᚳᚩᛇᛖᛝ•ᚳᚻᛟᚻᚫᛄ•ᛟᛉ•ᛁᚳᛖᛏᛋᚹᛖᚾᛡᚣᛄᛗ•ᛖᚳᚪ␍␍ᛞᚩ•ᛟᛏᚦᚫ⊹ᚳᚹᛄ•ᛉᛠ•ᚷᛠᛗ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes08()
    {
        // Arrange
        var originalRunes = "ᛉᛁᛉᛗ•ᚢᛉᛗᚳᚦᛈᚩᛒ•ᛡᚾᛏ•ᛠᛉ•ᛈᚱᚣ•ᚩᚳᛠᛗᛝᚷᛉᛚᚢ•ᛝᛁᛏᚩ•ᛄᚠᛝ•ᛋᛚᚾᛞ⊹ᚩᛗ•ᛇᚫ•ᚱᛞᚹᛏᛄᚦ•ᚣᚦᛋ•ᚫᚣᛖᛋᛉᛟᛒ•ᛠᚱᛇ•ᛈᛝᚢᛈ•ᚩᚦᛉ•ᚪᚻᛟᚱᛝᚢᛖᚱ•ᚣᛚᛉᛚ•ᛡᛚᚱ•ᛈᚹᛇᚾ•ᛠᚪᚱᛉᛝ•ᚣᛋᚻᚢᛚ•ᛋᚣ•ᚷᚾᚢ•ᛇᚫᚾᚾ•ᚩᚫᛖᛞ•ᚪᚩᛄᛡᚢᚪᛉ•ᚱᛉᛡᛟᛄ•ᛗᛁᛇᛚᛠᚻᚦᛗᛠᚣ⊹ᚷᛒᚳᛈᛉᚳ•ᚾᛟᛟᛋᚷᛗᛈᛖᛏᛚᚾᛄ•ᛄᚳᛝᚩ•ᛁᚹᛚᛠᛒ•ᚠᚪᛖ•ᛏᛝ•ᚾᛈᛠᚩᛏᚦ•ᚻᛝᛉᛈᚻᛈᚳᛈᚱᚢ•ᛚᚠᛖᛟ•ᚷᚪᛒᚠᛁᚫᚠᚢᛟ•ᛗᚠᚣᛝᛄᚳ•ᚻᛏᚠᛚᚫ•ᛖᚦᛋᛚᚩᚢ•ᚫᚩᚪᛗᛟᚢᚹᛇ⊹ᛒᚾᛋᛚᛝᛄᛟᚾ•ᛗᛚᛒ•ᛟᛏ•ᚾᛞᛒᚩᚾᚦᛡᚻᛟ•ᚱᛈᚾᚠᛈᛞ•ᛋᚩᛁᛠᚣᚾ•ᛇ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes09()
    {
        // Arrange
        var originalRunes = "ᚣᚹᚫᚷᛄ•ᛝᛗᚪᚹᛈ•ᚪᚢᚾ•ᛈᛡᛗᛖᛞᛟ⊹ᛁ•ᛉᛡᛗ•ᚠᛈᚩ•ᚦᛉᛞ•ᚩᛞ•ᛋᛈᛉᛡᚷ•ᛟᚻᚠᚦᛉᛄᛟᛋᚦᚣᚦ•ᛏᚻᛋᚣ•ᚻᛠᚷᛚᚫᚱᛏ•ᚢᛋᛟ•ᚦᚠᚠᚣᛟᛡ•ᛇᚳᚣᛒᛚᛝ•ᛠᚱᚻᛞ•ᛄᚣᛏᚫ•ᚻᛞᚳᛋ•ᛉᚠᛞ•ᚦᛗ•ᚳᛇᛝ•ᚫᚾᛡᛠᚹᛁᛡ•ᛒᛗᛝ•ᚷᛈᛁᚳ•ᛠᛚᚷᛉᚣᚣᚱᛄ•ᛉᛁᛄᚢ•ᛖᚣ•ᚪᛝᛈ•ᛡᚫᚳ•ᛖᛠᚹᛒᚦᛟᚠᛗ•ᚫᚱᚠᚩᛏ•ᛝᛉᛞ•ᛗᛖᛡ•ᚩᛈᛋ•ᛇᛞ•ᛇᛟᚫᚾ•ᚷᛗᚣᛁᚫᛁᛄ•ᛈᛄᚩᛡᚷ•ᛈᚳᛄ•ᛚᛖᛡᚻᛚᚷᚱᛇ•ᛟᚣ•ᛠᚣᛗᚹᚾᚹ•ᚠᛁᛄᚢᛗᚫᚾᚳᛗᛠᛁ⊹ᚩᛇ•ᛒᛚᛞ•ᚾᚹᚠᚾᛒᚱ•ᛋᛟᚦᛡ•ᚪᛡᛏᚷᚷᚹ•ᚪᛋᛡᚦᛋᚦᛋᚠᛗᚷᛞᛠ•ᛝᛈᚩᚪᚣᛝᛈᛋ•ᛟᚾᛇᚪᛖ•ᚻᚢᚷ•ᚩ•ᚢᚦᛏ•ᛒᚷᚣᛝᚠᚣᛁᚻ•ᚹᛡᛠᚱᚫᚹᛡᛞᚪᚦ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes10()
    {
        // Arrange
        var originalRunes = "ᚳ•ᛉᚢ•ᛈᛏᛋᚢᛖ•ᚷᚦᛡᛚ•ᛖᛋᛠᛝᛉᛈᛉ•ᚾᛟ•ᛞᛟᛒ•ᚾᚹᚢᛁᛇᛚᛞ•ᛁ•ᚦᚣᚷ•ᛟᛈᛡ•ᛖᚪ⊹ᚠᛋᛉᛞ•ᛖᚷᚦᛠ•ᚾᛋ•ᛞᛟᛗᛖ•ᛗᚾᛉ•ᚹᛒᛠᛈᛟ•ᛗᛉᚫ•ᛄᚩᛞᚻᛡᚷᚠ•ᚣᛗ•ᛁᚷᛉᚻᚹ•ᚾ•ᛋᛗᚷᛠ•ᚣᛚᚱᛄᛗᛉᚣ•ᛇᚱᚢᛟ•ᚣᚦᚢᛟᚩ•ᚱᚢᚹ•ᛁᛒᚳ⊹ᛠᛏᛞ•ᛚᛖᛋᛄ•ᚳᛟ•ᚷᛞᛡ•ᚢᚹᛝᚻᚫᚢᛈ•ᛏᛈᚩᚣ•ᚾᛇᚦᛟᛏᛇᚳᚠ•ᛒᚪᚾ•ᛗᚦᛝ•ᛟᛠᚢᛁᚪ•ᚾᚻᛝᛉᚩ•ᛇᛁᛡᚠᛟᛒᚦᚠ•ᛋᛒ•ᚠᛞᛇ•ᚩᚦᛏ•7•ᚷ•ᛚᛄᛖᚫ⊹ᚣᛁᚫᚹᚻ•ᚫᛏ⊹ᛁᛉ•ᛉᚻᛞᚩᛠ•ᚫᛋᛝᛚᛝ•ᛖᚩᚻᛗᚩᛟᛒᚦ•ᚱᛚᛋ•ᚳᚻ•ᚪᛡᚾᛇᚱᛉᚦ•ᚣᛉᚻ•ᛡᚾᚢ•ᛗᛉᚹ•ᛖᛈᛖ•ᚩᚳᛈᚳᛞᚪᛉᚢᛗᛝᛟ•ᛋᚾᛟᛉ•ᚠᚱᚳᛒᚢᛄᚱᚫᛝ•ᛒᛋᛟᛠᛡᚪᛚ•ᛏᛟᚾᚫᛟᚪ•ᛁ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes11()
    {
        // Arrange
        var originalRunes = "ᛡᛋᚳᛖ•ᚹᛒ⊹ᚾᛚᛝ•ᚦᚾᛁᛠ•ᛒᛡᚱᚠᛖᛁᚹ•ᚾᚠᛗᚢᚷᚾ•ᛄᛚᚳᚱ⊹ᛝᚣᛉᛋᚪᛟᚱᛉᚳ•ᛒᚫ•ᚠᚢᚪᛖᚪᚹ•ᛚᚾ•ᛄᛉ•ᚻᚦᛉ•ᛗᛚᚾᛖ•ᛏᛝᚦᚪᚩᚢᛗᚣ•ᚠᛝᚪ•ᚻᛡᛇᛡ•ᛚᛏᛁ•ᛇᛁ•ᚳᚢᚢᛖ•ᚳᛒ•ᚫᛇᚠᚦᚳᛚᚩᛉᛚᚩᛚ•ᚠᚳᛠ•ᚪᚠᛟᚫᚠ•ᚾᚳ•ᚢᛒᚱ•ᚾᛇᚩᛉ•ᛁᚳᛟ•ᛞᛉᛠᛝᚠᚱᛡᚳᛇ•ᛉᛟᛈᛗᛞᚳᚦᚹᛈ⊹ᛡᚻ•ᚾᚦᛇᛏᚹᛖᚢ•ᚫᛇᚦ•ᛝᛟᛏᚳᚷᛒᛠ•ᚪᚳᛒᚪᚩᚹᚫ•ᛉᚢ•ᚫᛖᛒ⊹ᛇᛏᚢᚩ•ᛟᛞᚠᚢᛋ⊹ᛡᛄᛗᚦᛠᛏᚪ•ᛒᚹᚣ•ᛏᛄᚻᚦᚫ•ᛚᚪᚱᚫᛟᚦᚩᚾᛟᛁᛖ•ᛡᚠᚷ•ᛋᚠᚦᛏ•ᛠᛡᛠᛁᚢᛡᛇᛝᛞ•ᛉᛏᚠᛒᚻᚢᛋᚳᚱᛇᚹ•ᛇᛈᛋᚢᛚᚪᛈᚢᚳᛖᚠᛞᛉ•ᚦᛠᛇᛝᚻ•ᚣᚱᛗ•ᛟᚾᛚ⊹ᛈᚹᛞᚱᛄ•ᚪᛝᛞ•ᛁᚦᛏᚷᚢᚹᚳᚻᛖᚩᚪᛖ•ᛉᚪᚢ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes12()
    {
        // Arrange
        var originalRunes = "ᚳᛁ•ᚱᚳᚹ•ᛠᛇᛏ•ᚦᚳᚻᚢ⊹ᛡᚹᛟ•ᚷᛇᛈ•ᚢᛈᚦ•ᚷᚣᚢᚪᛗ•ᚹᚳᛖᛝᚱᛠᛞᛏᚻ•ᛄᛁᛈᚻᚠᛉᛝᛈᚾ•ᛒᚳᚪᚷᛋᛟ•ᛉᛠᛈᚪᚩᚷᚠᚳᛡᛄ•ᛠᚢᚠᛋᛚ•ᚣᛚ•ᚢᛒ•ᛉ⊹ᚱᚣᚾ•ᛁᛠ•ᛚᚹᛋ•ᚠᚦᚪᛠ•ᛈᚷ⊹ᛏᚷᛡᛟᛠᛡᛒ•ᛉᛄᛒ•ᛖᚾ•ᛞᚠᛠᛗ•ᚦᚪᛗᚠᚪ⊹ᚻᛡ•ᛗᛁᛏᛟ•ᚻᚣᚹᛏ•ᚠᛒᛁ•ᚫᛖ•ᛝᛒ•ᛚᛏᛠᛉ•ᛟᛋᚾᛉ•ᚹᛏᛠᛏ•ᛖᚢᛡᛖ•ᛉᚾᛇ•ᛟᚳᚾᚠᚩᚾᚠ•ᚳᚪ•ᚷᚱᚩ•ᛠᚦᚹᚣ•ᛒᛁ•ᛝᛇᛟ•ᚣ•ᚷᛗᚩ•ᛁᚷᛄ•ᚩᛇ•ᚢᛁᛉᛝᚪᚱᛉ•ᛏᛄᛞᛈ•ᚾᛝᚷᛏᚢ•ᛚᚷᚳᛏ•ᚢᛒᛇ•ᛈᚩᚣᚢᛏ•ᛡᚫᛏᚹᛏᛇ•ᛡᚫᚫ•ᚦᛏᛝ⊹ᛠᚳᛁᛉᚻᚦᚣ•ᚻᛚᚾᛋᚱᛡᚫᛚᚫ•ᛖᚷᚻ•ᛞᚾᚻᛠ•ᚠᚪᚹᛖᚠᛄ•ᛒᛇᚱᚹᛏᛉᚾᛠᛖᛁ•ᚠᚾᛡᚳ⊹ᛋᛟᚹ•ᛈᚷᛝᛟ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes13()
    {
        // Arrange
        var originalRunes = "ᚷᚦᚠᛄᚷᚳ•ᛒᛁᛗᛚᛇᛠᚹ•ᚾᚫᚹᚷ⊹ᚩᚻᚪᛏᚾᛄ•ᚣᛝᛏᛡᛝ•ᚢ•ᚩᚠᚣ•ᛗᚢᛒ•ᛏᚠᛈ⊹ᚱᚩ•ᛉᚩᛝᛒ•ᛖᛏᚩᛉ•ᚣᛗᚠᛉ•ᛖᚩᚫᚷᚣᛚ•ᚩᛇ•ᚠᛋᚫᛇᛗᛡᛟᚹᚾᚩᚢᚹᛖᛁ•ᚾᚦᚫᛠᚪ•ᛠᛚ•ᚹ•ᛡᚩ•ᚢᚦᛗ•ᛝᛚᚪᚠᛝ•ᛚᚠᛚᚳᛒᚢᛝᛉ•ᚣᛡᚪᚷ•ᚹᛟᚪᚻᚹᚢ•ᛖᛠᚷ•ᛁᚪᛏᛄᛗ•ᛏᛖᛁ•ᚣᛡ•ᚦᚾᚠᚦ•ᚩᛈᚻᚪ•ᚻᛋᛠ•ᛡᛉᚪᚫ•ᚠᚣᛞᛠᛇᚠᚫ•ᛏᛗ•ᚳᛡᚷ•ᚱᚢᛞ•ᛄ•ᛋᛡᛇᚩ•ᛚᛟ•ᚦᚱᚫᛒᛚᚦ•ᛖᚪᚦᛗᛚ•ᚦᛉᚪᚱ•ᛟᛖᛒᛄᚱᛄᛖᛁᛈ•ᚪᛖᛠᚠᛄᚢ•ᛞᚹᚦᚣ•ᛉᚷᚩᚳᛡ•ᛇᛗᛞᚳᛏ•ᚻᛚᚦᛝᛖᛗᚱ•ᛒᚷᛞᛉᛗᛒᛉᚳᛝᚦᚣᛞᚫᛠ•ᛋᛏᛗᛏᚻᚹ•ᛇᚳᚪᛞ•ᛠᚢᛒᛉ•ᛡᛁᛡᛚ•ᚷᛋᚦᛞ•ᚠᚢᚩᛠ•ᛚᛋᚣᛏ•ᛋᚪᛞᚫᚹᛄᛞ•ᛋᛈᛋᛄ•ᚪᛖᛁᛇᛒᛟ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes14()
    {
        // Arrange
        var originalRunes = "•ᛏᛄ•ᚠᚩᛚᛞ•ᚾᚷᚳ•ᛚᚷᛗ•ᛠᚦᚢ•ᛟᚻᚾᛟᚣᛡ⊹ᛇᚻᚣᚪᛈ•ᚾᛋ⊹ᛞᚫᛠᚳᛉᛄ•ᚦᚹᛋᚱᚦᚫᚾ•ᛡᛚᚣᚫᛋᛖ•ᛟᚣᛝᛡ•ᚦᚣᚷᛇᚱ•ᛋᛠᛏ•ᛡᚳᛉ•ᛠᚷ•ᚳᛒᛋ•ᚹᚾᚻᛖᛝᛋ•ᚩᛡᛗᛉᛝ•ᛉᚦ•ᛠᛞᚳᛒᚷᛉᚹᛝᚢ•ᛉᛞᛈ⊹ᛉᛡᛈᛟ•ᚾᛡᚠᛡᚢᛋ•ᛉᚪᛖᚻᚱᚣᛠᛇ•ᛒᛟ•ᚪᛝᛡ•ᚳᚱᚳᛈᚩᛏ•ᚻᚣᚫᛁᛋᚩᚦᛚ•ᛟᛚ•ᛋᚪᚢᚪᛈᚻ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes15A()
    {
        // Arrange
        var originalRunes = "␍ᚠᚢᛚᛗ•ᚪᛠᚣᛟᚪ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes15B()
    {
        // Arrange
        var originalRunes = "ᛚᚢᛝᚾ•ᚳᚢ•ᛒᚾᛏᚠᛝ␍ᛁᚢᛁᚢ•ᛟᚫᛄᚠᚫ•ᚢᚷᛉᛇᛈᛉ•ᚣᛠᛚᚪᛉ•ᛟᛉᛡᚦᚻᛠ•ᚾᚪᚳ•ᚢᚷᚾ•ᛈᛖᚾᚦᚩᚢᛁᛡᚱ•ᛏᛁᛒᛇᚳᚠᚷ•ᚩᚦᚪ•ᛁᛈᚻᛡᛒ•ᚹᛈᚻᚱᛞᛉᛏᚢ•ᚣᛒ•ᚠᛋᛉᚢ•ᛗᛁ•ᛡᚱ•ᛝᚢᚠᚦᛝ•ᛈᛟᛒ•ᚻᚷᚻᛡᛚ•ᚩᛞᚪᚳ•ᚦᛈᛞᛋᛡᚻᛇᛚ•ᚢᛏᛋᛞ•ᚦᚢᛞᛝ•ᛚᛉᛝ•ᛏᚩᛚ•ᚪᛚ•ᚣ•ᛟᛡᛉᚣ•ᛒᚻᚫᛄᛡᛁ•ᚱᚦᛚᚠ•ᛠᚾᛝ•ᛉᛗᛒᚩᛠᛈ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes16()
    {
        // Arrange
        var originalRunes = "ᛖᛞᚪᚫᛏᚩᛠᛖᛠᛉᚳᛠᛏ•ᚩᛞᚳᛠᚾᚳᚦᛗ•ᛞ•ᚷᛁᚳᚹᛟ•ᚪᚢᛒᚳᚫ•ᚦᚱ•ᛋᚣᚪ•ᛏᚦᛒ•ᛝᚹᛋᚱᛁᛝ•ᛒᛁᚪᚫᛚ•ᛏᚱᛡᚫᚠᛞ•ᛝᛄᚩ•ᛡᛠᛉ•ᚪᛡᚻ•ᚱᛒᛁ•ᛞᛡᛄᚪᛈᚱᛋ•ᚢᛡ•ᚻᚷ•ᛚᛟᚠ•ᚻᚷᚫᛋ⊹ᛈᚹᚷᚷ•ᛗᛟᚪᚾᚱ•ᚩᛟᛞ•ᚷᛟᚠᛠ•ᛡᚷᚳ•ᛉᛠᚠᛚ•ᛒᚫᛈ•ᚩᛄᛈ•ᛄᛗᛠ•ᚾ•ᛉᚪ⊹ᛡᛖᛋᚷᚫᚦ•ᛄᚷᛉᚩᚦᛄᚳᚣ•ᚢᛄᚦᛄᚪᚾᛏᛒ•ᚳᛈᛡᛄᛋᚫ•ᛋᛗ•ᚻᛞᛠᛉᚢᛗ•ᛏᛠᛖᚣᚠ•ᛄᛏᛋᛗᛞᛟᛁᛝᚪᛉᛖᛈ•ᛚᛇᛞᚦ•ᚪᛋᛉ•ᚳᛒᚢᛟᚳᛒᛚᚾᛟᛝᛉᚩ•ᛖᚳ•ᛝᛟᚳᛁᛒᛈᚫ•ᚣᛖᛄᛝ•ᛞᚢᚱ•ᛉᛟᚩ•ᚠᚹᚩ•ᚣᛁᚠᚢᛇ•ᛚᛏᛈᛒᛗ•ᛇᛝ•ᚢᚳᚱᛡ•ᛖᚩᛁᚣᛄᛏᛡ•ᛖᚠᛇᚠᛚ•ᛁ⊹ᚣᚷᚠᛝᛡᛈᚷᛒ•ᛡᚩᚷᛡ•ᛟᚾᚹᛡᛈᛟ•ᚦᛈ•ᛟᚷ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes17()
    {
        // Arrange
        var originalRunes = "ᛚᚦ•ᛈᛞ•ᚦᛇᛒ•ᛡᚪᛒᚪ•ᚾᛗ•ᚳᚾᛖᛡᚹᛝᛏᚱ•ᛝᚫᛚᛟᛁᛇᚣ•ᛝᛡᚾᛏ•ᚱᛁ•ᛋᚪᛖ•ᛇᚢ•ᛝᛞᛄ•ᚠᚱᛠᛗᛠᚪ•ᚫᛈ•ᛏᚠ•ᛖᛏᚷᚾᚠᛁᚠ•ᚱᚻᚱᛇᛒ⊹ᚻᛈᛏ•ᛇᚱᛝᛡᛒᚹᛚᛏ•ᛗᛉᚦ•ᚾᛄᚳᚫ•ᚷᛈ•ᛋᛖᚩ•ᚢᛝᚩ•ᛏᛈᛁᚣᚾᚪ•ᛏᚹ•ᚠᛗᚾᛟᚾᚳᛒ⊹ᛄᛉᛡ•ᛟᚪᛁᚫᛝ•ᛒ•ᛉᛏᛄᛁᛋ⊹ᛠ•ᚳᛖᚱᚦᚣᚩᚣ•ᛈᚫᚷ•ᛡᛄᛁᚩ•ᚱᚦᛠ•ᛇᚦᚩᛉ•ᚾᚱᚾᚫᛁᛉ•ᛁ•ᛝᚣᚫᛡᚫᛗ•ᚹᛖ•ᛇᚷᚻᛖᛗ•ᚷᚢᛞᚹ•ᛄᚻ⊹ᛉᚱᚢᛄᚢᚾᛈ•ᛋᚣᛄᚫ•ᛈᚳᚣᚳᛒᛡ•ᚫᛟᚪᚠ•ᛏ•ᚷᚩᛇᛟ•ᛁᚱᛗ•ᛖᛉᛟ•ᛗᛇᚫᛟᚦ•ᚱ•ᛞᛁᚢᚦᚻᛗᛡᚾ•ᛁᚦᚻᛚ⊹ᛏᚳ•ᚪᚦ•ᚠᚪᚫᚣᚻᛠ•ᚦᚠᛋᚠᛝᚷᚱᛈ•ᛏᛄᛉᛟ•ᚷᛚᚻ•ᚩᚪᚦᛏᚳᛁ•ᚠᚣᚢᛁᚹ•ᛟᚪᚣᛁᛠᛄᚪ•ᛟᛝᚦ•ᛟᚠᚦᚾ•ᛇᚷ•ᛠᛚᛒᚠ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes18()
    {
        // Arrange
        var originalRunes = "•ᛠᚪᛄᛇᛠᛚ•ᚱᚷᛋ•ᚹᚩᛒᛁ•ᛠᚳ•ᛁᛞᛄ•ᛖᛗᚱ•ᚷᚪᚻᛠᛚᚷᚩ•ᛉᚻ•ᛡᛝ•ᛞᚱᚹᚩᛈᛡ•ᚣᚳᚦ•ᛁᛇᚢᛁ•ᛟᚦᚠᚳᚻ•ᚩᛁ•ᛝᚾᛁᛞ•ᛏ•ᚫᚱᛝᚫᛈ•ᛠᛞᛇᛉᚳᛠᚩᛟᛖ⊹ᛗᛈᛒᚦᛝᛋᚢᛡ•ᚻᛡᛏ•ᛉᛇᚷᚠᛡᛡᛟᚢ•ᛡᚦᚣᛞᚪᚫᛝᛒ•ᚳᚩᚷ•ᛏᛞᚦᛁ•ᚠᛒᛖ•ᚦᛟᚳ•ᚠᚻ•ᛞᚠᚣᛋᚾᛟ•ᛠᛇᛄ•ᛖᛉ•ᚩᛈᛠᛚᚪ•ᛟᚩᚾ•ᛄᛉᛋ•ᚣᚫᚷᛖᚩᛟᚢᚱᚹᚢ•ᛟᛡᛄᛇᚢᛞᛉ•ᛒᛇᚳ•ᛝᛚᛗᛠᛗ•ᚪᚱᛡᛗᛒᚩᚹ•ᛋᛖᚾᚻᚣ•ᛈ•ᛞᛚᛞ•ᛈᛏ•ᚪᛞᛚᛉ•ᛟᚱᚾᚹ⊹ᛠᚠᛁ•ᛟᚾᛒ•ᛇᛟᛖᛝᚳᚠᛏᛞᛏ•ᛇᚫ•ᛝᚢ•ᛠᛡᚫᛖᛟᛞᛝᛠ•ᚠᛗᛒᛚ•ᛏᚢ•ᛈᚱᚹᛟᛇᛉ•ᚳᛟᛈᛏ•ᚢᚠᚳᛞ•ᛄᛋᛞᛈᛚ•ᚠᛝᚱᛄᚣ•ᛞᛗᛖᚣ•ᚢᛖᛝᛠᚳᛞᛈᚩᛠ•ᛏᛒᚳ•ᚷ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes19A()
    {
        // Arrange
        var originalRunes = "ᚾᚩᛟᚾᚠ•ᚩᛁᚠᚢᛋᚾ•ᛞᚹᛠᛇᛈ•ᚱᚩᚩᛄ•ᚪᛟ•ᛇᛠᛄᛁ•ᛟᛄᛞᚢᚳᛝᚩ•ᚱᛝᛋ•ᛄᛁᛈᛉᛖ•ᛞᛁᚾᛗᛗᚳ⊹ᛉᚩᛁᛄᛞᚳ•ᚢᚪᛇ•ᚦᛡᛇᚻᛠᚣ•ᛠᚻ•ᚠᚩ•ᛡᛠᛋᛟᚪ•ᚹ•ᚫᚻᚩᛄᚢᚱᚩᚣ•ᛏᚫᚪᛡᚷ•ᛄᛚᛄ•ᛝᛏᛖᛒᛚᛉᚻ•ᚱᚩᚫᛇᛈᛄᛠ•ᚳᛈᛚᚣᛈ•ᚪᛠᚻᚻᛋᚫ•ᚩᛝᚹ•ᛋᛞᚠᚳᛠ•ᚩᛇᚫᚪᚩᚹᛗᚪ•ᚣᚫᚷᚫᛄᚱᚹᛞ•ᚱ•ᚦᚷᚳᚹ•ᚾᚷᛡ•ᛚᛒᚳ•ᛄᚷᚹᚹ⊹ᚱᛁᚠᛏ•ᚠᛚ•ᛋᛄᛚᚪᛄᚱᛏ•ᛞᚷᚫᛠᚠᛉᛞ•ᚫᚷᚻᛏ•ᛗᚣᛈ•ᛏᛒᛟᛝ•ᛄᛋᚾ•ᛝᛁᚹ•ᚦ•ᛠᛝᛞᚾᛟᚷᚫ•ᛁᛗ•ᛝᛉᚱᛞᛋᛗ•ᚠᚫᚹ•ᛟᛋ•ᚦᛞᛞᛈᛝ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes19B()
    {
        // Arrange
        var originalRunes = "ᛞᛡᚷᛒ•ᚪᛟ⊹ᚦᛡᛒ•ᚪᚹ•ᚾᛉᚫ•ᛚᛈᛁ•ᛒ•ᚠᚾᚠ•ᛡᚩᛏᛞᚾᛋᛖᚳᚻ•ᛖᚻ•ᚢᛟ•ᚪᛖᛗᛝ•ᛠᚫ•ᛈᚩᚪᛞ•ᚠᚫᚻ•ᚠᛏᚦᛄᛚᛄᛒ•ᛗᛇ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes20()
    {
        // Arrange
        var originalRunes = "ᛈ•ᛄᚢᛒ•ᚷᛁᛇ•ᛈᛉᚣ•ᛈᛟᚦᛞᚱᛠᚪᛡ•ᛝᛡᛒᛚᚻᚦᚫᛉ•ᛟᚫ•ᚪᛇ•ᛉᚳ•ᛠᚠᚫ•ᚢᚣᚦᛋ•ᚠᛝᚠᚱᚹ•ᛟᛒᛗᚷᛞᚾᛡ•ᛞᚪ•ᚻᚣᛇ•ᚱᛚ•ᛖᚣᛇᚻᛠᚩ•ᚢᚳᚱᚻ•ᛡᛟᛗᛠᛝᛄᚦ•ᛄᚢᛁᛇ•ᛄᛁ•ᛖᚷᛁ•ᚪᛇᛏ•ᛝᛡᚳᛚ•ᛇᚠᛗᚪ•ᚷᛚᛒᛋ•ᛉᛞᚫᛟᛋᛚ•ᚹᛏᛠᛗ•ᛚᚦᛗ•ᛝᚦ•ᚣᛈᚠ•ᚪᛞᛚᚪᛖᛚᚩ•ᚱᚷ•ᛚᚳᛇᛏᚷᚣᛟᛗ⊹ᚪᛁ•ᚷᛄᛒᛡᛗ•ᛞᛈᚪᚳᛠᚷᛋ•ᛏᛈ•ᚩᛋᛏᛗᚱᚣᛋᛉ•ᛁᛄᛚᛝᛚᛁ•ᛉᚢᛠᛗᛇᚢᛋᚻ•ᚳᛉᛄᚩ•ᚠᛄᚠ•ᛁᚣᛁᛟ•ᛏᚷᚱᚦ•ᛡᛒᛋᚳ•ᛇᚢᚷ•ᛚᚱ•ᛁᛗᚱ•ᛗᛝᚻᛈᚫ•ᛝᛋᚫ•ᛖᛈᛁ•ᛒᛇᚹᚫᚢᛄᚳᛒ•ᚦᛋᚹᚦᚫ•ᛡᛟᚷᛚ•ᛞᛚᚢᛟᛡ•ᚱᛞᚱᛒᛄᚳᚢᛠ•ᚩᛉᛉ•ᛝᛡᛄ•ᛁᚫᛟ•ᛖᛗᚹ•ᛖᛉᚦᛗᚪᛋᛉ•ᛞᚦ•ᛡᚢ•ᛉᛗᚫᛋᚳᛖ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes21()
    {
        // Arrange
        var originalRunes = "ᚳᚫᛠ•ᛞᚳᚷ•ᚩᛁᛇ•ᚾᛟᚷᚣᚳᚦᚳᚦ•ᛗᚣ•ᛈᚪᛒᛈ•ᚻᚢᚻᚾᛏᚫᛒᛇᚩᛁᛈ•ᚫᚩ'ᚣ•ᛡᚣᛗᚷ•ᚠᚱᛡᛚᛏ•ᛖᛟᚩᛈᛚᚩᚷᛁᛟᛠ•ᛞᛖᚳᛗᛁᚣ•ᛈᛚ•ᛁᚹᛋᛄᚹ•ᛟᛡᚪ•ᚦᛖᚩᛄᚷᛋᛝᚣᛗᛟᚻ•ᛗᚠᚦᛉᚦᚫᛋᛈᚣᚩᚠ•ᛈᛟᛋᛖᚫᛇᛗᛚᛈᚾ•ᛡᚠᚳᚾᚩᛄᛋᛡ•ᚫᛄᚦᚪᛠ•ᛈᚻᛋᛟ•ᛗᚹ•ᚱᚣᛁᚢ•ᛉᚹᛋᚱ•ᛞᛈᚦᛈᚩ•ᛞᛄᚩ•ᚢᛈᛖᚪᚫᛉᚫ•ᛏᚱᛟᛏᛒ•ᛠ•ᚫᚳᚾ•ᛖᛝᚦᛄᛄᚠᛚᚾᚩᛒ•ᛉᚷ•ᚪᚩᛚ•ᚪᚢ•ᛞᚻᚳᚹᛚᛡᛞᛇ•ᛟᚩᛡᛚᚳ•ᛡᚳᛉ•ᛝᛠᛝᚷᛝᛞᛄᛏ•ᛠᛈ•ᚹᛈᛗ•ᛈᚱ•ᚫᛏᛖᚢᛝᚫᛡ•ᚾᛁᛠᚻᚦᚣᛠ•ᚫ•ᚩᛉᛋᚩ•ᛄᚠᛏᚷ•ᚹᛁᚪᛁᚩᛁ•ᛝᛠ•ᚾ•ᚷᛗᚹᚦᛖ⊹ᚷᛟᚪᚹᛞᚻᚢ•ᛡᚹ•ᚣᚷᛉᛒᚪᚾᛝᛡᛄᛡ•ᚠᚷᛈᚦᚠᚦ•ᛁᛈᚪᛝᛋᛞᛟᚩ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes22()
    {
        // Arrange
        var originalRunes = "ᛝᛗ•ᛁᚷ•ᛄᚷ•ᚳᚩᚦᛖᚦᛄ•ᚣᚠ•ᚦᚳᛄᛡᛖᚢ•ᛉᛄᚳᚻᛄᚱᛄ•ᚪᚻᚾᚦ•ᛚᚷ•ᚱᚦ⊹ᛒᚪᚩᛖᚢᛡᛄᚹᛏᚱᚹᛟ•ᚦᚳᛗᚦᚠᚫᚻ•ᛡᚠᛠᚣᚪᚦᛚᛏᛒᚢᛝ•ᛖᛋᛗᚱ•ᚪᚹᛒ•ᚹᛒᛗᚱᚾᛗᚻᛗᛁᚾᚪᛞ•\"ᛡᛖᚩ•ᚾᚹᛡ•ᚢᛄᚦᛠ•ᛚᚳᚷᛚᛇ•ᛟᛠᛠᚪ\"⊹ᛇᛉᚣᚪ•ᚷᛏᚩ•ᛖᚹᛒᛈᚷᛝᛒ⊹ᛡᚦᚠᛋᚾ•ᛒᚦᚠ•ᛇᛝᛠ•ᚠᚾᛉ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes23()
    {
        // Arrange
        var originalRunes = "ᚢᚪ•ᚹᛝᚷᛉᛞᚷ•ᛁᛒᛁ•ᛇᛏᛒᛁᚣ␍ᛠᚷᛋᚫᛈᚹᛗᛠ•ᛇᛄᛇ•ᚹᚻᛁ•ᚷᛠᛒᚢᚣᚻᚣ•ᛝᚹᚢᚱᛋ•ᚩᛡᚠᛡᛠ•ᛞᛟᚦᛗᚳᚾᛉ•ᛞᚦᛖᚱᛇᚳ•ᚪᛄᛋᛟ•ᚢᚹᚱᛏ•ᛋᛖᛋᛏ•ᚣᚱᛠᚫᚾᛞ•ᛈᛒᛡᛋᚢᛞᛖᚣᚦ•ᛚᚹᛟᛋ•ᚷᛚᛄ•ᚫᛖᚩᚳᚦᚹᛗ•ᚢᚩᚷ•ᚠᚪᚩᛡᛝᛒᛠᚦᚳᚪ•ᚱᛡᛏ•ᛟᚹᚠᚣᛝᚢᚣᛁ•ᛚᛏᚫᚫ•ᚪ•ᚱᛈᚠᛗᚹᚩᛞ•ᛠᛒᛈ•ᛝᛟ•ᚾᚷᛗ•ᛡᛖᚩ•ᚾᛚᛉᛝ•ᛁᛡᚫᛗ•ᚻᛖᚹᛗ•ᛝᛈᛇᛗᛡᛄ•ᚫᚩᛡ•ᚠᚣᛉᛟᚫᚦ•ᚫᛒᚩ•ᚪᚦᛄᚱᛄᚾᚦ•ᛡᚠᚪᛏᚾᚻ•ᚷᚢ•ᛞ•ᚳᚦᚢᚱᚢᛟ•ᛞᚻᚱ•ᚷᚹᛏᛈᛖᚠ•ᚪᚻᛠᚦ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes24()
    {
        // Arrange
        var originalRunes = "ᛞᚱᚠ•ᛖᛄᚫ•ᚾᚳᚻᚹ•ᛇᛡᛈᛠᚹ•ᛗᛚ•ᚹᛟᚹᛠ•ᚪᚾᚪ•ᚳᚪ•ᚷᛚᚦᛒᚩᚹᚢ•ᚷᛚᚠᛋᚻ•ᚾᛉᛝᛗ•ᛖᚦᚢᛝᛡ•ᛈᚣᚢ•ᛉᚷᚷ•ᚹᛞᛁᛋ•ᚦᛡᛡᛈᚳᚪᚩ•ᚢᛗᚢᛉᚩᚣᚻᛏ•ᚩᚫᛗᚢ•ᚩᚾᛏᛠᛒᛟᛒᚠᛁᛈ•ᛚᛋᛝᚫᚳ•ᚫᛟᛏ•ᚢᚩᛉᚾᛡᛋᚠᛖ•ᛉᚱ•ᛗᚩᚩᚫ•ᚠᚢᚦᛖᛞᚾᚣ•ᛡᛋ•ᛋᚱᛚᛟ•ᚢᚻ⊹ᚢᚾᛈ•ᛁᚻ•ᛖᛉ•ᚦᛞᛗ•ᛈᛟᚠ•ᛈᚠᛝᚫᛝᛋ•ᛟᛄᚹ•ᛠᛒᚣ•ᛟᚹᛞ⊹ᚠᚣᛄᛁᛏᛉᛚ•ᚩᚦᛝ•ᚠᚪᛋᛡᛁᚻᛒᚱ•ᚪᚢᚣ•ᚫᚢ•ᛟᛠᚪᚣ•ᛖᛟᚫ•ᛖᛈᚠᛒ•ᛈᛄᛁ⊹ᛋᛝᛒ•ᚱᚦᚳᛇ•ᛚᛁᚢᛈᛏᚳᛒᛉ•ᛖᚪᚣᚠᛗᚳᚣᚱ•ᚻᚹᛏᚾᛡᛉᚫᚦᛟ•ᚳᚹ•ᛠᚠ•ᛏᛠ•ᛝᚩᚻ•ᛡᛠᛒᛋᚻᛟ•ᚫᛁ•ᛠᛏᛁᛋ•ᛏᚫᚻᚱ•ᚻᛄᛋᛡᚹᚾᚾᛡᚹᛚ•ᚢᛖ•ᛏ•ᚱᛝᚳᚣ•ᚪᛉᛇᛝᛋᛖᛇᛁ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes25()
    {
        // Arrange
        var originalRunes = "ᚻᚾ•ᚷ•ᚹᛉᚳᛉᚣ•ᛋᛈᚳᛟᚱ•ᛒᚣᛄᛝᛖᛁ•ᚾᚷᚪ•ᚣᚷ•ᛚᛒ•ᚢᛄᚩ•ᛝᛉᛉᚪᛖ•ᛒᚦᛉᛡᚱ⊹ᛏᚷᚹᛄᛋᛁᚠ•ᛠᛁᛡᚦᛝᚾᛖᚾᚠᚩᛗᛖᚣᚪ•ᚳᛖᚳᚹᚪᚫᚹ•ᛇᚢᚦᚻᛉᚢᚾ•ᛠᛚᚢᚾᚦᛈᛋᚢᛈᚱ•ᛞᚫᛟᚱᛡᚫᚪᚢ•ᚢᛗᛚᚦᛠ•ᛚᛝᛈᚣ•ᚩᛋᛟᚪᚱᛗᚦᛟᛈ•ᛚᛋ•ᛏᛁᚠᛋᛖᚹᛝ•ᛗᛞᚩ•ᛠᚫᛡᛒᛏᚩᛋ•ᛖᛏᚪᚠ•ᚫᛒ•ᛚᚾ•ᛋᚪᛉᛟ•ᚾᛚᚹᛖ•ᚩᛚᛁᛄᛏ•ᛒᚪᚠᛉᛏ•ᚩᛟᛄ•ᚾᚷᛋ•ᚷᛚᚷᛠ•ᛒᚷᛖᚩᚪᚩᛖᛞ•ᚷᛇᛗ•ᚳᚱᚷ•ᛈᛞᚩᚠᚹᛇ•ᛠᛞᚣᛝ•ᚾᛁᚠᛈᛚ⊹ᛖᛟ•ᚢᚳᛗ•ᛚᚫᛏᛉᛄᚱᛉ•ᛁᛠᚷᛚ•ᚷᚳᛋᚩᛝ•ᚫᚦ•ᛗᚻᛟᚠ•ᚱᛋᚳᚦ•ᚣᚩ•ᛒᛁᚫᚻᛖᚢᛏᛚᛚ•ᛇᚷᛟᚣ•ᛒᚾᚦᚻ•ᛠᛖᛄᛒᚾᛁᛚᛠᚱ•ᛄᚠᚳᛋᛝᚳᛈ•ᚷᚻᛋᛗ•ᛇᛞᛇ•ᚣ•ᛡᛖᛏᛠᚢ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes26()
    {
        // Arrange
        var originalRunes = "ᛡ•ᚩᚾᛠᚩ•ᛄᚣᛇᛉᛠᚪᛡ•ᚾᛞᛝᚻ•ᛈᛠᚻᛡᚢ•ᛝᚻᚦᛈ•ᛉᚢ•ᛠᚣᛈᛟᚦᛋᚣᛈ•ᚠᛏ•ᛒᛁᛟᚪᚷᛚ•ᛠᚻ•ᛝᛁᛡᛚᛝᚾᛞᚪᛈᚷ•ᚾᛏᚦᛋᛒ•ᛋᛋᛠ•ᚷᚳ•ᛠᛗᚢ•ᛖᛉᛒᚷᚫᚠᚩᛁᛉ⊹ᚠᚪ•ᛠᚱᛇ•ᚩᛁᛞᛋᛚᚦᛖᛒᛇ•ᛟᚷᚣᚷᚾᚷ•ᚦᚠᚳᛗ•ᚩᛖᛖ•ᚩᚠᛒᚻᛝ•ᚳᛁᛄᚪᚾᚩᚪ•ᛈᚻᚱᛗ•ᚱᛗᛟ•ᚦᚷᛄ•ᛒᚱᚦᚪᛠ•ᛉᛖᛡᛞᚦ•ᚱᛝᛄᛒ•ᚾᛏᚣ•ᛏᛋᛒᚾᚫ•ᚢᛖᛁᚩᛡ•ᛄᛇᚢᚦᛚᚳᛖ•ᛚᛁ•ᛒᚢᚠᚪᚱᛠ•ᛗᛒ•ᛞᛉᛗ•ᚢᛠᛏᚣ•ᚪᛄᛈᚢᛈᛠᚣᚷ•ᛗᛡᛗᚢᚪᛗᛝ•ᚣᛡ⊹ᚪᛖᛏ•ᛖᛋᚪᛟ•ᚳᚻᛁᛋᚠᛁᚾ•ᛈᛟᛝ•ᛇᚦᚣᛏᚫᛉ•ᛖᛟᛏ•ᛞᛡᛚᛖᛈᛏᚪ•ᛏᚠᚱᚾ•ᚪᛠᚱ•ᛠᚳ•ᚾᚻᚹᛒᛇᛋ•ᛁᚻᚣᛋᚹᚩᛉᚹ•ᚩᛝᚢ•ᚻᛝᛟ•ᛏᛚᚠ•ᛄᚷᛏᛝᛄᛝ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes27()
    {
        // Arrange
        var originalRunes = "ᛗᛈᚣ•ᛚᛋᚩᚪᚫᚻᛚᛖᛇᛁᛗᛚ•ᛚᛋᚳᛈ␍ᚾᚻᚷᚢᛡᚻᚢ•ᛒᚠ•ᛞᛄᚢ•ᛒᛖᛁ•ᚫᚠ•ᛈ•ᚫᛈᚦ•ᚱᛗᛚᚳ•ᛒᚷᚣᛗᛠᛒᚫ•ᚾᚦ•ᛗᚠᛡᛠᚳᛒᚷᚫᚠ•ᛖᛄᚱᚩ•ᛈᛒ•ᚠᛒᚩ•ᛇᚱᛠᚱ•ᛠᚷᛖᛚ•ᛇᚱᚾᛋᚩᚩᚳᚪᛖᚣᛖᛖ•ᛏᚱ•ᚢᚣ•ᛟᛄᛉ•ᛠᚷᛝ•ᚣᛏᛝᚾ•ᚪᛏᛋ•ᛝᚪᛄ•ᚠᛚᛋᚢ•ᚹᛠᛈᛁᛏ•ᛁᚾ•ᚱᚱᛝᛗ•ᚣᛗᚠᛁᚫᛁᚪ•ᚢᛟᛒᚹ•ᛗᛁᚻᚣᚹᛞᛚ⊹ᛟᛏᛞ•ᛟᚳᛒ•ᛡᛒ•ᚪᛏ•ᚹᛏᛈ•ᚹᛠᚩᚱᚩᛖ•ᚣᛚᛋ⊹ᚢᛡᚱᚠᛄᛇᚱᛡᚦᛖᚢᛏ•ᛝᚫ•ᚾᚪᛠᚩᚪᚾᚪᚦᚷᚩ•ᚫᛉᛒᛏᛖᛠᛗᚷᚱᛗ•ᚣᛝᚠᛒ•ᛞᛟᛞᚪ•ᛠᚱᚳᛁᛈᛞᚠᛗᛝᚻ•ᛋᚩ•ᛞᛈᛉᚾ•ᛟᚱᛡᚾᚳᚳᛏ•ᚾᛈᚠ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes28()
    {
        // Arrange
        var originalRunes = "ᛈᚳ•ᛄᚦᛒᛁᚹ•ᛞᚹᛝᛠᛡᚹᛚ•ᚹᛄᚾᚪᛟ•ᛏᛞᛉᚣᛖᚱᛞ•ᚱᛏᛇᛁᚳᛈ•ᛝ•ᚦᛟᚷᛄᚦ•ᚣᛋ•ᛠᚻ•ᚠᛒᛚ•ᛁᚫᛚᛞᛉᚪ•ᛁᚹᚷ•ᛒᚩᚹᚾᛠ•ᛋᛖᛗᛒᛋ•ᚳᚹᚦᛟᚠᚻᚫ•ᛞᚢᛁᛒᛞ•ᛇᛝᛈᚠᛁ•ᛟᚢᚣᛏ•ᚻᚱᛖᚾᚳᛈᛡᛈᛞᛄ•ᛁᛏᛗᛋᚫᛉᚩᚣ•ᚪᛄᛗᛡᛖ•ᛇᛄᚠᛗᚱ⊹ᛞᛟᚪᛒᛞᚻ•ᚾᛈᚪ•ᛇᚱᚻᚾᛝᛠᚠᚾᚠ•ᚩᛗᛋᚾ•ᛠᚪᛁᚢᛚ•ᚪᚫ•ᛄᛉᛡᚠ•ᛁᛖᛈᛠᚻ•ᚠᛇᚩᚹ•ᛠᛄᛇᛁᛠᚫ•ᛄᛒ•ᛋ•ᚠᛖᚷ•ᛋᛁ•ᛟᛗᛒᛁᛝᛏᚪᚢᛁᚦ•ᚩᛝᛗᚠ•ᚹᛟᛒᛟᛡ•ᚠᚣᛝᚩᛠ•ᚳᛚᛈᚱ•ᛞᛄᚩᛝᛄ•ᚪᛖᛗᛈᚾ•ᚠᛠᚷᛞᛒ•ᚩᛉᚷᚾᚣᚷ•ᛠᛈᛄᛞᚾᛟᚩᚢᚾᚹᛗ⊹ᛄ•ᚢᚷᛠ•ᛗ•ᛇᚪ⊹ᚻᚦᛡ•ᛝᛈᛞᛒ•ᚳᛉᚳ•ᛠᛉ•ᛟᚣ•ᛒᚦᛁᛄᛚᛡᛝᛡ•ᚹᛄᚫ•ᛋᛗᚪᛡᛠᛇᛝᛏ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes29()
    {
        // Arrange
        var originalRunes = "ᚦᛞᚷ•ᚢᛏᛚᛏᚣ•ᚢᛝ•ᚷᛟᚪᛏ•ᛄᚦᚣ•ᚫᚻᚪ•ᛒᛝ•ᚦᚢᚱᚪᚾᛞ•ᛁᛝᚫ•ᛚᚫᚷ•ᚹᛁᛒᚣ•ᚾᚫᚠ•ᛚᛋᛒ•ᛈᛟᚪᛟᛞᚷᛟᚣᛉᚷᛚ•ᛋᛠᛁ⊹ᚳᛟᛁᚦᛈᚹᛉ•ᛖᚢ•ᛟᛄᛝᛋᚢᛝ•ᚳᛡᛠ⊹ᛚᛇ•ᛚᚷᚢᛁᛏᛒᛋ•ᛞᛁ•ᚠᚠᚷᚠ•ᚦᛄᚳ•ᚫᛟ•ᛁᛗᛡᛁᛇᚦ•ᚩ•ᚢᛈᛒ•ᚻᛋ•ᛄᚣᛄᛖ⊹ᛒᛇᛇᚱ•ᚹᛄᛏᛡ•ᚳᚪᚫ⊹ᚩᛈᚱ•ᛡᚾᛗᛁᛝ•ᚻᚹᚦ•ᛡᚦᚻᚦ•ᛉᚫᚫᛋᚳᛡᚾᛇ•ᛟᛉᚢ•ᚱᛄᛖ•ᛚᚾᛞ•ᛗ•ᛏᚱᛟᚦ•ᛁᛝᛡᛒ•ᚳᚩᚹᛟ•ᛏᛗᛋᚱᚷ•ᚱᛚᛞᛚ•ᚩᚣ•ᛞᚳᚪᛖᛞᚠᚳ•ᛇᛖᛉᛚᚫ•ᛖᚩᛁᛋ•ᛡᛁᛟᛋᚪᛒᛗ•ᛗᚣᚹᛄ•ᛖᚫᛝᛚ•ᛄᚱᛇ•ᛈᛚᚩᚻ•ᚪᛞ•ᛡᛄ•ᛞᚠᚹᛞᛄᚳ•ᚾᚦᛉ•ᛄᚻ•ᚷᛚ•ᚠᛖᚦ•ᛇᚻ•ᛝᛖᛒᛚᛞᛁᛗᚠ•ᚹᛒᛗᛟᛁᛖᛁᛠ•ᛈᚻᛝᛖᛞᛟᚩᚻᛄ•ᚹᚩᚾᛄᛈᛗ•ᛖᚳ•ᛖᛇ•ᚷᚻᛗ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes30()
    {
        // Arrange
        var originalRunes = "ᛞᚪᛈᛖ•ᛗ•ᛉᚫᛒᛇᚱ•ᛖᚣᛟᚣ•ᚱᛠᛈᚢᛠ•ᚣᛖᚪᚻ•ᚩᛉᛠᚢᚻᛡᛟ•ᚷᚫᚩᛒᛉ•ᚫᚱᛞᛋᚩᚱ•ᚷᛠ•ᛉᚻᛁ•ᚷᚳᛞᛠᛡᚳ•ᛄᛠᛉᛇᚻᛋᚹ•ᛝᛡᚷᛖᛡᚣ•ᛠᚩᚷ•ᚱᚦᚠᛟᚩᚦ•ᚦᛁᛏᚱ•ᛇᛉᛇ•ᚢᚷᛠ•ᛟᛏ•ᚩᚠᛚ•ᛟᛝᛈ•ᚱᛡᚪᚩᛏ•ᚩᛠᚷᚫᛗ•ᛈᛋᚱ•ᛖᚦᚠ•ᛞᚹᚾᛚ•ᛝᚩᛇᛄ•ᚳᛚᚢᚹᛏ•ᚩᛖᛏᚠᚪᛚ•ᛟᛇᛟ•ᛠᚱᛇ•ᚢᚪᚦᛈᛟᛡᛉ⊹ᛡᛒᚱᛒᚠᚢᛚᚢᛟ•ᛒᛇᛒ•ᛉᚦᚹ•ᛝᚣᛖ•ᚳᚫᚣᛟ•ᚹᛁᛝᚫᛏ•ᚫᛇᛈᛡᛟᚠ•ᛚ•ᛝᚠᛡ⊹ᛞᚪᛚᛈ•ᛋᛁ•ᚢᚣᚪᛚᛠᛝᚹ•ᚪᛏᛈᚳᚣ•ᛝᚫᚻᛗᛞᚷᛚ⊹ᛠᛉᛒ•ᛇᛡᛋᛖ•ᚣᛁᛚ•ᚣᛠᚣ•ᚻ⊹ᚣᛉᚾᛏᚫᛉᛋᚦᚪᚹᛗ•ᚪᚱ•ᚪᚩᚻ⊹ᛗᛖᚫᛞᛠᛁᛗ•ᛒᛟᚾᚳᚩᚱᛉ•ᛋᚹᚫ•ᚻᛖ•ᛋᚠᚾ•ᚢᚦᛟᚷᛖᚪᛟᛇᛇ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes31()
    {
        // Arrange
        var originalRunes = "ᚦᚳᛒᛝᛏᛉᛡᛞ•ᛋᛡ•ᚩᚠ⊹ᛈᛖᛞᛋᛁ•ᛚᛁᚻᚾᛝᚱᚻᛈ•ᛇᚢᚫᛞ•ᛚᚻᛉᚳᛈ•ᛁᛗᛉᚳ⊹ᛄᚫᚾᛞᛋ•ᛏᛚᛡᚩᛋᛗ•ᛚᛞᚾ•ᛈᚫᛏᚷᛈ•ᚫᚦᛄᛗ•ᛒᚻᚩᚻᛁᚷᚻᚳ•ᛚᚹᛋᚱᛇᛗᛏ•ᛄᚳᛁ•ᛠᚦᛞ•ᛏᛚ•ᚱᛖᛠᛒᚪ•ᛒᚠᛒ•ᛁᛒᛡᛇᛏᚣ•ᛏᛖᚣᚳᚱᛋᚠ•ᛁᚦᚪᛉ•ᚪᚣᚫᛠ•ᛄ•ᛈᛗ•ᚠᛋ•ᚪᛒᚱ⊹ᛉᚣᚻ•ᚦᚩ•ᛇᛞᚢ•ᚠᛁ•ᚻᚩᚫᚠᚣᚷᚱᚪᛄ•ᛏᛉᛇ•ᛖᛠᛞ•ᛏᚠᚢᛝ•ᚫᛄᛖᛈᚳᛒᚦᚢᛝ•ᛡᛒᚹᚱ•ᛖᚾᛈᛇᚣᛇ•ᛉᚱᚹ•ᛒᛡᛞ•ᛖᚱᚩᚻᚣᛠᛈᚦ•ᛗᛁᚷᛚ•ᚹᛉᚫ⊹ᚠᛞᚾ•ᛄᛟ•ᚻᛚᛡ•ᛗᛖᚷ•ᛟᛁᛡ•ᚻᛟᚱᛇᚹᚣᚠ•ᛈ•ᛄᚷᚦ•ᚪᛒᛝ•ᛈᛒᚪᛖ•ᚢᚹᚻᚩᛒᛋᛉ•ᚹᛞ⊹ᚦᛇᚱᛖ•ᛄᚾᛞᛝᚹᚪ•ᚻᛖᚹ•ᛟᛡᛄᛡᛟᛝᛄᛉᛚᛄ•ᛞᛉᛟᛈ•ᚱᚪᛁᛏᚷᛉᛝᛇ•ᛠᛗᚩ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes32()
    {
        // Arrange
        var originalRunes = "ᛚ•ᚦᚫᚹ•ᚫᚢᛈᛡᚳ•ᚹᛝᚻᚹᛒᛗᛋᛟᛖᛁᛡ•ᛟᚹᚦᚻᛒ•ᛡᚱᛏᚦᚠ•ᚠᚩᚦ⊹ᚻᚩᛗᛖᛉᚹᛞᛋᛚᚠᛞ•ᛝᛒᛇᛡᛚᚪ•ᚹᛞᚾᚫᛉᛏᚣᛗᚷ•ᚦᚹᛉᛡᚦ•ᚹᛒᛋᚱᛉᛡᛉᚪ•ᚢᛒᚻᛠ•ᚹᛝᚢᚻᛇᛝᛡᛠᛄ•ᛋᛈᚦᛏ•ᛟᛝᚩᛗᛒᚢᛞᛋ•ᛒᛄ•ᛠᚱᛟ•ᛖᚾ•ᚾᚹᚷᚢᛚᚪᚩᚣ•ᚢᛏᚠᛄᛏ•ᚪᚷᛒᛇ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes33A()
    {
        // Arrange
        var originalRunes = "ᛞᛇ•ᛉᚳᚠᛁᚪᚹᚻᚷ␍ᛇᛟ•ᚠᛏᛖᛟᛠᚪᛡᛋᚷ•ᚣᛠᚾᚦᚫᚱ•ᚩᛡᛗ•ᚹᛉᛗ•ᚣᛞᛒᛏᚱ•ᚢᛄᚻ•ᚫᛟ•ᛡᛝᚹᚻᛋᚠᛡ•ᛚᚦᛏ•ᛁᚹᛏ•ᚩᚢᚾᚹᛗᛚ•ᛋᚦᛠᚹᛄ•ᚪᛄᚫᚷᚣᛗᚹᛞ•ᛈᛡ•ᛖᛄᚹ•ᛖᚢ•ᚻᚹ•ᛝᛁ•ᛋᚫᚷ•ᛄᛚ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes33B()
    {
        // Arrange
        var originalRunes = "ᛝᚦᛇ•ᛁᚠᚳᛟᛇ␍ᛞᛒᚣᛡᚣᚢ•ᚣᚾᚦᚱᛖᛗᛁ•ᛇᛞᚱᚹ•ᛉᛒᚻ•ᚳᛄᛡᚪ•ᚾᚹ•ᚾᛗ•ᚠᛇᛁ•ᛇᚪ•ᚩᛋᛒᛟ•ᛏᛄ•ᛈ•ᛖᛈᛄᚩᚹᚢᛠᛝᚹ•ᛗᚳᚩᛏᛏᚠᚢᛄ•ᛞᛠᛉᚩ•ᛉᚦᚷᛞ•ᛒᚩᛏᛚᛇᛁᛒᛡᚪ•ᛖᚠᛠᚢᛖ•ᛈᛋᚹᛞᛞ•ᛋᛡ•ᚹᚦᛞᛋ•ᛝᛄ•ᛚᚷᚢᛡ•ᚾᛉᚠ•ᚱᚪᚣᛗᚠᚦᚻ•ᚱᚪᚱ•ᚫᚪᚷᛟᛞ•ᛒ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes34()
    {
        // Arrange
        var originalRunes = "ᛗᛒ•ᚾᚻ•ᛇᛞ•ᚻᛗᛚᛁ⊹ᛠᚾᛁ•ᚫᛖᚢ⊹ᛏᚦᛇᛋᛈᚻ•ᚻᛇᚳᛠᚫ•ᛞᛚᛋᛝ•ᛁᚹ•ᚪᚳᚩᛏᛇᛝᚷ•ᚳᚦᛋᛠᚠᚢᛝᛚᚻ•ᚹᚩᛇᚪᛈᚷ•ᛇᛗᛚᛄᛋᛏ•ᛚᚳᛈ•ᚾᛋᛝ•ᚳᚪᚳ•ᚾᛉ•ᚾᚢᛉᚫᛗᛏᛞᛏᚫ•ᛟᛗᛋᛉ•ᛏᚣᛉ•ᛇᛠᚷ•ᚻᛒᚾᚷᛇᚢᛟ•ᛄᚦᛉᚩ•ᚾᚪ•ᛞ•ᚩᛈ•ᛠᛚᛋᛏ•ᛒᚷᛁᚢᛟᛖᛁ•ᛄᚦᛖᚻᚹ•ᛄᚫᛄᚾᚻᛉᚹ•ᛒᚪᛋ•ᚠᚱᚱᛁᛉᚢᚦᚻ•ᚢᛗᚪ•ᛞᛝᛠᚪ•ᚫᛉᛖᚾᚹ•ᛟ⊹ᛝᛞᚾ•ᛈᚫᚳᛡ•ᛈᚠᛉᚩ•ᛒᚷᛗᚫ•ᛚᚻᛞᚣᛖᛉᛒ•ᛄᚹᛇ•ᛈᚩᛁᚦᚠ•ᚷᚾᛈᛞᛝᛏᛖᚪ•ᛄᛋᛠ•ᛈᛝᚢ•ᛒᚷᚳᛉ•ᚪᚢᛈᛚ•ᛄᚱᚷᚣᚪ⊹ᚪᚠ•ᛗᛝᚣᚳᛟ•ᚹᚣᚷᛈ•ᛗᛖᚩᚹᚢ•ᛟᛞᛋᚱ•ᚣᛞᛋᚳᛡᛉ•ᚻᚦᚹᛚᛞᛠᚩᛞᛠᚢᛟᛖ⊹ᛠᚹ•ᛉᚻᛡᚹᛞ•ᚪᛗ•ᚠᚦᛈ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes35()
    {
        // Arrange
        var originalRunes = "ᛝᛏᚳᚪ•ᛠᚣᚷ•ᚳᚦᛖᚾᚢᛁᚫᛁᚢᛡ•ᚹᛚᚳ•ᚻᛈ•ᛞᛄᚳ•ᛗᛒ•ᛗᚪᛄ•ᚩᚪᛞᛁ•ᚩᚱᛟᚠᛖᚣᛟᛁ⊹ᛇᛟ•ᛁᛈᚣᛚᚪᛡ•ᚳᛏᛠᛋᛖᛒᛝ•ᚫᛟᚫᛞᛖᛞᚣᛡ•ᛠᚪᛖᚦᛚᚫ•ᚳᛋᚪᚩᚷᚹᛚ⊹ᛈᛖ'ᛏ•ᛄᛉᛝᛚ•ᛏᛉᚩᚣᛝᚠᚩᚣ•ᛁᚻ•ᛟᚫᚷᛄᛝᛡᚾᛗᚣᛟᛡ•ᛝᚷᛖᛉ•ᛟᛉᛈᛚᛋᛉᛠ•ᛚᛡ•ᚱᚪᛞ•ᛠᚷ•ᚱ•ᚳᛇᚻ•ᛗᚪᛟᚷ•ᛞᚪᛋᛡᚻ•ᛈᚷᛖᚳᛟᚱᛟᚢ•ᛁᚫᛟᚦ•ᛄᚱᛡ•ᚱᛖᚦᚣᛏᛝᛡᚩᛒ•ᛏᚦᚳ•ᛉᚳ•ᛋᚪᚫ•ᛗᚠᛄᚱᛖ•ᛡᛇᛁᛇᛟᛉᚳᚹᚪᛖ•ᛋᚢᛉ•ᛋᛟᛚ•ᛄᚾ•ᛈᛇᛒ•ᚦᚦ•ᛁᚫᛚᛋᛝᛄᛄᛡ•ᛟᚻᛇᚢᛚ•ᛁᚱ•ᛡᚻᛚᛏᚹᛉᛇ•ᚱᛏᛠ•ᛁᚫᛚᛗ•ᛁᚱᚷᛏᛠ•ᛇᛟᚻᛟᚳᛋᛏᚾᚩ•ᛁᚱᚷ•ᚹ•ᛞᚢᚣᛚᛁᛗᛒᚢ•ᛚᚱ•ᛏᛁᚢ•ᚷᚳᚠᛇ•ᛚᛇᚣᛏ•ᛏᚫᚢ•ᚫᛠᛇ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes36()
    {
        // Arrange
        var originalRunes = "ᚣᚾ•ᚢᚹᛝᚻ•ᚷᚣᚱ•ᚩᛁ•ᛚᚾᛉ•ᚾᚩᛈ•ᚠᛠᚫᚫᚩ•ᛉᚾᛋᛟᚫᛚ•ᚾᚫ•ᚦᚢᛠᚣᚫ•ᛈᛁᛇᚢᚱᛄ•ᛈᛟᛄᚪᛝᛈᚦᛈᚪᛝ•ᚣᛗᛟ•ᛉᛒᚢᛏᛇᛗᛈᚫᚣ•ᛉᚫᚣᚱᚫᚣᚠᚠᛗᛡ•ᛉᛖ•ᚱᚢᛏᚷᚢᚣᚱ•ᛡᚢᚩᛇᛁ•ᛄᚠᛈᛄᛞ•ᛁᚦᚩᚻᛡᚷᚻ␊1ᛚᚦᛇᛟ•ᚪᚫᛠ•ᛗᛉᚻᚳᛉᚪᛏᚦ•ᚫᛉ•ᚩᛋᚳᛞᛏ•ᚣᚹᚾ•ᛟᛏᛉ•ᚹᛁᛟᛄᚠᛁᚩ•ᛁᚱᛋ•ᛉᚾᛗᚪᛡ•ᚱᛈᛋᛞ•ᛁᛟ•ᚻᛖᛏᚢᚹ•ᛠᛟᛞᛟᛄᛁᛝᛡ•ᛄᚱᛞᛗᛒ•ᚩᚳᚩ•ᚦᛟᚱᚢᛚ•ᚢᚦᛋᚢᛞᛚ•ᚷᛁᚣᛝᚩᛟ•ᛁᛖᚣ•ᛖᚠ•ᛇᛝᛒᛚᛁᚢᚣᚠᛟᚾᛟ•ᛒᛟᚷᛄᚪᚾᛗᚫ•ᚣᚦᚠ•ᛁᛒᛝᛈᚾᛁᚱᚷ•ᛄᛇᚫ•ᚻᚪ•ᚱᛉᛉ•ᚩᛚᚾᚫ•ᛞᚣᛒᚾᚪ⊹"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes37A()
    {
        // Arrange
        var originalRunes = "2•ᚾᚣᛖᛉ•ᚾᚢᛉᛁ•ᛝᛏᛈᚹᛋᚣ•ᛏᛠᛈᛉ•ᚪᛁᛄᛋᚱᚪᛏᛋᛝᛏ•ᚳᚷᚳᚻ•ᛖᛟᚱᚪᛡᚻᚳ•ᛝᛒᛖᚱᛠᚪ•ᛚᛟᛖᛚᚪ•ᚦᛋ•ᚳᚹᚱᚹ•ᚩᚻᚣ•ᚢᛝᚩ•ᛈᛚᛁᛏᚪ•ᚠᛋᛝᛞ•ᚳᚪᚱᛒ•ᚹᛈ•ᚾᚩᚦᚳᚦᚾᛗᚩᛖ•ᚣᛇᚾ•ᚠᛒ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes37B()
    {
        // Arrange
        var originalRunes = "3•ᛞᚢᛈ•ᚹᚾᛖᚪ•ᚱᛚᛁᚹ•ᚫᛉ•ᛝᚠᛞᚪᚠ•ᛒᛄᛉ•ᛞᛄᛝᚣᛇᚪ•ᚫᛄ•ᛝᛈᚪ•ᚢᛠ•ᛇᛏᚱ•ᛖ•ᚫᛗ•ᚫᛠᚻ•ᛁᚫᛟ•ᛠᚹᚳᛄᚦᚻ•ᛡᚩᚢ•ᚩᚦᚷᛡ•ᚻᛋᚷᚪᛁᛟᛞᚪᛄ•ᛁᚹᛡᛒ•ᛗᛝᛡᛞᚠᛒᛋᛏ•ᛒᚷᚠ•ᚷᛟᚢᚳᚫᛏᛁᛖ•ᚱᚷᛗᚣ•ᚪᚷᚹ⊹"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes37C()
    {
        // Arrange
        var originalRunes = "4•ᛝᛄᛋᛄᛗᚱᛗ•ᚾᛒᛋᛗᛉᛞᚻᛉᛁ•ᚣᛡᚻᚣᛠᛉᚻ•ᛞᛖ•ᚹᛖᚦ•ᚢᚳ•ᛉᛗᚪᚣᛠ•ᚹᚫᚪᚳ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes38()
    {
        // Arrange
        var originalRunes = "ᚢᚫᚳᛇᚳᚣ•ᛡᚫᛏᛖᚳᚠ•ᛋᚻ•ᛋᚱᚢᚦ•ᛁᛋᛝᛗᛞᚫᚢᛠᚢᚪ⊹ᚾᛝᚳ•ᛖᛈᚹᛉ•ᚢᛉᚫ•ᚾᛈᚳᚻᚱᚣᚹᛚᛉᚱᛒ•ᛗᚫᛟᚣᚩ•ᚳᛇᛗ⊹5ᚻᚫᛉᚦᛒᛟ•ᛏᛟᚹᛄ•ᚫᛠᛗᚠᚫᚳᚷ•ᛇ•ᚻᚹᛗᚻᛝᚣ•ᛁᚩᛁ•ᛏᛁᛖᛡᛄ•ᛗᚣᛚ•ᚻᚱᚩᛞᛒᛡᛈᛠᛗ•ᚳᛠ•ᛖᛒᚢ•ᚷᛁᚦ•ᛟᚫ•ᛡᚻᛝᛖᚾ•ᚱᛠᛡᛋ•ᚻᛏᛝᚻᚪᚷᚩᛝᚫ•ᚹᛚᛏᚱ•ᚷᛁᚾ•ᛖᛠᛄᛡᛞᛋᚻ•ᛝᚾᚳᛋᚾᛞᛇᚾᛋᛁᚳᛡ•ᚱᛝᛚᚫᚣᛇᛚᚩ•ᚳᛞᚾ•ᛝᚷᛡ⊹ᛝᛄ•ᚻᛄᛚᛠᛟ•ᛄᛏᚷ•ᛚᛒᛝᚢᛏ•ᚻᚳ⊹␊ᚫᛞᛟᚫᛟᛗ•ᛟᚫᚪᚻᚱᛗᚢ•ᚣᚢᚣ•ᛈᛗ•ᚪᛄᚫᛟᛠᛚᚠᛖᛡᚢ•ᛉᚻ•ᚪᚩᛡᛒᛠᚢᚷ•ᚻᛏᛠᚪᛞ•"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes39A()
    {
        // Arrange
        var originalRunes = "ᛋᚹ•ᚦ•ᚾᛋᛁᚻᛒ•ᛉᛠᛝ•ᛒᚢᛚᛟᚢᚾ•ᚢᚦᚩᛗᚪ•ᚾᛞᚫᛇ•ᚫᚣᚪᛋ•ᚣᛝᛡᛗᚷᛇᚾᛈ•ᛠᚳᚻᛝᛚ•ᚠᚷᛡ•ᛁᛡᚪᚠᛒᛈ•ᚳᛋᚦᛠᚦᚫᚱ•ᚷᛞᛚᛟ•ᚷᚱᛁᛇ•ᚣᚩᛟᚢᛝᚱᚷ•ᛗᛏᚷᛒᛈᚷ•ᛗᛏ•ᛗᚣᚹᛒᛏᛒ•ᚷᚣᛈᚷ⊹ᚾᚦᛇᛒᚳ•ᚷᛖᛇᛟᛚᛈ•ᚹᚾ•ᚻᚷᚱᛇᛏ•ᛈᚷᛒ•ᚹᛗᛋᚹᛟᚻ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes39B()
    {
        // Arrange
        var originalRunes = "ᛡᚳᛋ␍ᛈᛞᛋᛡ•ᚪᚹᛏᚳᚹᛟ•ᛗᚹᛁᛒᛞ•ᚷᛇᚢᛚ•ᛉᛋᚫ•ᛟᚻᛚᚦᛒ•ᚣᚪᛚᛞᚦᚠ•ᚻ•ᛞᛝᚩᚢᛋᚪᚫ•ᛖᚦᛁ•ᛏᛄᛏ•ᛝᚦᚾᚳᛉᛏᛝ•ᚳᛈᛁ•ᚾᛏ•ᛒᚾᛡᚱᛒ•ᚢᛈᛋᚦᛁᚳᛈᛋᛁᚹ•ᚹᛚᚣᚾᚢ•ᛒᛁᚪᛠ•ᚹᛟᚳ•ᛠᚢᚪ•ᛚᚦᚹ•ᚠᚾᛏᚳᛡᛁ•ᛚᚩ•ᚾᛗᛄᛠ•ᚦᛟᛄ•ᚪᚦᚹ•ᛡᚾᛖᛠᛈ•ᛒᛋᛄ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes40()
    {
        // Arrange
        var originalRunes = "ᚠᚾᛗ•ᚣᚷᛞᚫᚻ•ᚪᛈᛉᚣᚻ•ᛇᛠᚩᛖ•ᛏᛝᛠ•ᛚᛁᛏᚦᚠ•ᛗᚪᚳᛖ⊹ᛞᚳ•ᛏᚱᛟᚷᛠᚾᚫᛒᚢᛖᛒᚢ•ᚦᚠᛟ•ᚷᛋᛟ•ᛁᛈ•ᛟᛉᛋᛒ•ᚹᛄᛒᚣᛗᚢᛠ•ᚱᛁᚢᛟᛄᛁ•ᛗᛖᚫ•ᚱᛋᛉᛝ⊹\"ᛠᛈᛚ•ᛞᚩᛚᛁᛉᛠᛝᛖᚱ\"•ᚾᛈᛖᚹᛡ•ᚾᛄᛏᚣ⊹ᛋᚩᛋᛏᛝ•ᚢᚾᛇᚪ•ᛖᛏᚪᛄᚳᚣ•ᛟᛒ•ᛚᛋ•ᛒᛞᛄ•ᛁᛝᚣᛖᚳ•ᛄᚻᛚᚣ•ᚷᚫᛚᛞ•ᛚᚫᛚᚦᛉ•ᛚ•ᛖᛉᚩᛉᛁᚳᚢᛗᚾᚢ•ᚩᚾᛇ•ᚻᛡᛚᛇᚩᚫᚪ•ᚩᛟᚩ•ᚣᚱ•ᛖᚠᚢ⊹ᛁᚻ•ᛟᛚᚾᛏ•\"ᚠᛞᚱᛠᚷ•ᛈᚩᛇᚩᛗᛠᛒ•ᛄᛡ•ᛋᛗᚠ•ᛏᚠᚫᚩ•ᛟᚳᛚᛞᛡᛚ•ᚩᚳᛝᚢ•ᛈᚹᛏ•ᚷᚳᛋ•ᚢᛟᚷᚦ•ᚠᛉᚠᛏ•ᚳᛋᛉᛟ•ᚷᚠᛉᚾᛞ•ᛒᛏᛠᛡ\"⊹ᛈᛡ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes41()
    {
        // Arrange
        var originalRunes = "ᛠᛁᚪ•ᛋᚣᛗᛞᚣᛋ•ᛒᛞᛄᛞ⊹ᚩᚾᛏᛚ•ᚳᚪᛝ•ᚱᚷᚻᚷ•ᛄᚹᚠ•ᚪᚢᛇ•ᛞᛏᛗᛄᛁ•ᛝᚫ•ᛉᛈᚳᛈᛠ•ᛟᚪᛒᛁᛁᛋ•ᛇᚷᚻᛋ•ᛇᛡᛒ•ᚠᚹᛝ•ᚫᚪᚠᚩᚣᛡᚪᚾᚻ•ᛒᚦᛟᛇᚣᛟᛁᛒ•ᛟ•ᚩᛋᚹ•ᛞᚳᚠᚪᛁ⊹ᛉᛏᛟᚢᚩᛟᚦᛈᛋᚩ•ᚻᛇᚦᛝ•ᛏᛠᚠᛝᛠ•ᚩᛗ•ᛏᚠᚣᛚᚣ•ᚹᛚᛞ•ᚪᛉᛠ•ᚪᛄ•ᚩᛋᛒᛚ•ᚳᛖᚾᚪᚩᚱᛏᚦ•ᚱᛒᚳᚣ•ᛠᛗᚹᛚ•ᚻᛈ•ᛇᛈᛖ•ᛚᛄᚩᛡᚪ•ᛖᛋᚫᚩ•ᛠᛉᛝᚣ•ᛖᚫᛒᛗ•ᛖᚻᚱ•ᛈᚾᛗ•ᚹᛏᛟᚣᚢ•ᚠᛉᛈᛗᚩᚷᚾ•ᛡᛇᚳᚠᛒᛈᛗ•ᛋᛇᛁ•ᛖᛈᚢᚱᛏᚳᚣ•ᛄᛚᚠ⊹ᚱᛚᚱᚫᛖᚻᛟ•ᛇᚣᛡ•ᚩᛉ•ᚪᛋᚣᛁᛝ•ᛉᛚᛄ'ᚳ•ᛖᚣᚢᛝᚦᛇᚱ•ᛠᛁᚫ•ᚦᚠᛟᚷᛠᛁ•ᛈᛋᛒ•ᛗᛒᛄᚠᚾᚳᛖ•ᚻᚫᚩᛄ•ᛉᛄᛚᛈᚪᛁ•ᛟᚹᚱᛁᚱᚦᛖᛉ•ᚪᚾ•ᛞᛄᚷ•ᛟᛟᚳᛏᛄ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes42()
    {
        // Arrange
        var originalRunes = "ᛞ•ᛉᚾᛗᚦ•ᛁᛄᚱ•ᛈᛉᚢᚫᚦᛒᚠᛄᚦ•ᚠᚪᛝᛖ•ᚹᚹᚣᛚᛇ•ᚢᚣ•ᚾᚱᚪ•ᛈᚾᚹ•ᛚᚾᛏᛚᚢᛒᚱᛝᚪᛋ•ᚫᛈ•ᛄᛚᚢᚳᚷ•ᛚᛏᛄᚹᛈ•ᚫᛗᛚ•ᛉᛚᛗᛏᛞᚠᛈᛁ⊹\"ᚠᚳᚦᛗᛄᚹᚱᚪᛚ•ᚩᛝᚱᚢᛈᚱᛟᛡ•ᚳᛉᚱ•ᛇᛏᚦᚾ•ᚱᛇᚫᛞᛟᚻ•ᛒᚾᚣ•ᚠᛡᚪᛡᛖᚫᛞᛄᚢᛖ•ᚦᚱ•ᚩᛇᚱᛡ•ᚣᛁᛉᛇᚻᚩᛠ•ᚫᚻᛡᛝᛠᚦ•ᚾᚣ•ᚾᚠᛁᛝ\"⊹\"ᛏᚻᚹᚫ•ᛒᛇ•ᛡᚻᛉᛒ•ᛞᛝᚱᛄᚦᚻ•ᚪᚷᚣᛁᚠᚷ•ᛁᛏᛞᛠᛒᚠᚩᛈ•ᛇᛡᛟᚹᚱᚾᚩᛏ•ᛋᚹᚢ⊹ᛖᛡᛖᛡᚦ•ᛉᚪᚷᛈᚾ•ᛋᚱᚠᛞᛝᚻᛖᛄᛞ•ᛄᛡ•ᚱᚹ•ᚷᛝᚪᛒ•ᛄᛈᛄ•ᛏᚠᛉ•ᚪᛄ•ᛁᚠᛉᚢᚩᚣᚻᚦ•ᚻᚾᛁᛒ•ᛡᛟᛡᛋᛈᚣᛉ•ᛠᚢᛠᛚ•ᚠᛝᛗᚻ•ᚦᛒᚩ•ᛗᛚ•ᚩᛠᛋᚦᛠ•ᛇᛋᛉ•ᚠᛗᛒ•ᚫᛋᛇᚾᛡᚾ•ᚢᚫᚹ•ᛞᛠᚢᚾᛝᚠᚾᛖᚫ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes43()
    {
        // Arrange
        var originalRunes = "ᚻᛄ•ᛁᛖᛏᛡ•ᚷᛁᚩᚾ•ᚳᚢᚫᛗᛈᛋᚪᛡ•ᚷᛚᚣᚹᛟ\"•ᚠᚢ•ᛉᚠᚫᛞᚠᛡᛄᚾ⊹ᚻᛋᚦᚠ•ᛏᚠᛄᚱᚹᚠᛋᚾᚹᛄᛖᛒᚢᚦ•ᚩᛇᚫᛈ•ᛡᛟ⊹ᚢᛁᚩᛄᚩᛇᛟᛄᛞᚩ•ᛈᚹᛞᚷᚱ•ᚠᛟ•ᛇᚷ•ᛄᛟᛇᚫᛋᚫᚣ•ᛒᛏᛞᛟ•ᛠᚻᛡᚱᛠᛠᛉᛋ•ᚠᚾᚣᚱᚠ⊹ᚪᚾᛡᚪᛖᚫ•ᚳᛇᛁᛝ•ᛒᛡᛞᛠᚫᛒᛠᚳᛉᚠ•ᚫᛏᛁᚱᚪᛗᚩ•ᛚᛉᛋᚪ•ᛒᚩᛈᚫᚩᛝᚻᛇᛖᛇᚫ•ᚻᛖᛇᛠ•ᚱᛗᛞ•ᚫᛇᛗ⊹ᚾᚾᚣᛡ•ᚱᚾᛗᛠ•ᛄᛉᛋᛄ•ᛟᛖᛒ•ᛏᚻᚾ•ᚠᚪᚠ•ᛒᚾ•ᚩᚾ•ᛖᛋᛏᛒᚹᛡ⊹ᚻᛏ•ᚩᛟᚩ•ᛒᚾᛖᚳᛁᚹᚣᛟ•ᛟᚩᛒ•ᛋᛖᚩ•ᚫᚻᛟᚠᚫᚷᚩᛄ•ᛟᛒᚻ•ᚳᛖᛁᛚᚫᚣᛚ•ᚢᛚᛁ•ᚾᛟᛏ•ᚫᛈᛟᛈᛝᛗ•ᚳᚢᛁ•ᚣᛋᚳᚢᛡᛇᚩ•ᚠᛖ•ᚷᛟ•ᚻᚫ•ᛝᚠ•ᛗᚠᛝᛉᛞᛁ•ᛗᛝᚣᚪᛝᚠᛉᛁᛟᚷᛚ•ᛇᚩ•ᚫᛡᛏ•ᛄᛏ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes44()
    {
        // Arrange
        var originalRunes = "ᛠᚢ•ᚷᚦᚣ•ᚦᚾᛟᚣᚩᛖᚻ•ᛁᛋᛖᚣᚦᚪᛡᛝᛟᛇᛚ•ᛡᛏᛝ•ᛁᛚ•ᚠᛉᛡᛠᛏ•ᚠᚾᛄᚠᚻᚳ•ᚻᛞᛠᚣᛟᛝ•ᛉᛇᚻᚩᛋᚻ⊹ᛇᛏᚠ•ᛚᚱᛇᚦᚪᛁᛁ•ᛒᚠᛁᛚ•ᛄᛡᛒᚣᛗᚫᚫ•ᛞᚻᛟ•ᚪᚹᛉᛚᛏᛁᚪ•ᛟᛞᛖᚾᛈᚻᚣ•ᚦᛚᛖᛋᛖᛟᚫᛖ•ᛏᚱᚪ•ᛁᚫᚹᚫ•ᛋᛈᚱ•ᛄᛡᚪᛏ•ᚫᚦ•ᚠᛠᚢᛈᚣᚫᛝ•ᚣᚾᚻᛡ•ᚳᛗᚠᚾ•ᛞᛄ•ᛖᚩ•ᛒᚷᚻᚪ•ᛖᛞᛟᚠᛇᛞᛟ•ᛈᚳᛁᚪᛒᚷᛒᛈᛟ•ᛟᛄᚠᚪᛖ•ᛄᚣᚩᛄ•ᚣ•ᚫᛋ•ᚦᛁᚫᛄᚫᛏ•ᛖᛇᚻᛟ•ᚣᚠᚹᛞᚷ⊹ᛡᚱᛒᚢ•ᛒᛚᚢ•ᚷᛈᛄᚪ•ᛏᛡ•ᚳᛄᚠᛡᛝᛚᚣᛒ•ᛗᚻ•ᚱᛚᛟᛠᛋᚦᛝ•ᛏᚳᛟᛉᛁ•ᛄᚱᚳᛖᛏᛄᚷ•ᛡᛈᛏᛉᚩᛁᛄᛟ•ᚷᚩᚪᚢ•ᚣᛖᚪᛋᛟᛇᚢᚪᛡ•ᛗᚱᛚᚳᚠ•ᛒᛗᛝ•ᚻᛉ•ᛠᛄᚫ•ᛉᚪᚷᚻᚣᛏᛖᛝ•ᛉᛉᛗᚾᚫᛋ•ᚱᛗᛞᛋ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes45()
    {
        // Arrange
        var originalRunes = "ᚳ•ᚦᛚᛟ•ᛝᛇᚢ•ᚻᚩ•ᛏ⊹ᚢᛁᚦᛄᚾᚠᚱᚦ•ᛋᛟᚷᛠᛗᚪ•ᛝᛚᚪᛁᛒᛠᚢᛋ•ᚩ•ᛖᛋᛝ•ᚠᛡᚢᛟᛞᛇᚪ•ᛞᛡᛒᚹᚩ•ᛄᛋ•ᛟᛝᛏᚳ•ᚻᚾᛇᛋ•ᛗᛚᚻᛞᛖᛈ•ᚫᛄᚱᚪᚢᚻᚱᚦᚱ•ᛟᛄ•ᛟᛗᚩᛟᛏ•ᚫᛇ•ᛉᛒᚳ•ᛄᛁ•ᚪᚩᛉ•ᚹᚪᚾᛈᛏᚢᚣ•ᛁᛒᚢ⊹ᚦᚩᛡ•ᛗᚳᚠᛉᚱᛁ•ᚪᛗᛏᛒ•ᛗᛚᛁᚦᛏᛠᛋᚾᚷᛚ•ᛏ•ᛇᛈ•ᚩᛚᛞ•ᛚᚹᚳᛄᚹᛉ•ᚪᛡᚹᛇ•ᛖᛖᚹ•ᛏᚪ•ᚣᚠᛉᚳ•ᛗᚩᚷᛞᚷ•ᛚᚳ•ᛒᚣᛋᚣᚠᛞᚣᛝ•ᛠᛇᛏᚩᚢᚫ•ᛟᛁᛒ•ᛏᚾᚫᚠ⊹ᛄᛟᛗᚾᛈ•ᛠᛡᚩᛏᛡᚪᚱᛞ•ᚪᛝᛈᚹᛗᛄᛟᛠᚩ•ᛚᚹᛉ•ᚱᛗ•ᚩᛏᚹᛄᚹᚾ•ᚷᚳᛠ•ᛄᚳᚢᚱ•ᛟᛇᛟᚾᚻᚫᛉ•ᚣᛚᚩ•ᚩᛡᚳᚻᛄ•ᛋᚣᚹᛁ•ᚣᚠᛋᚾᚪ•ᚷᛖᚾᛄᚪᚹᛠ•ᛞᚠᛟ•ᚢᛁ•ᛖᛇᚦ•ᚫᛞ•ᚳᛄ•ᚷᚢᚻᚣᚻᛁᛒᛉᚾ•ᚹᛝ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes46()
    {
        // Arrange
        var originalRunes = "ᚻᛏᛉᚫᛁᛄᚢ•ᛞᚠᛡᚫ•ᛋᛁᚹᛝᛈ•ᛗᛉᛄᛈ•ᛞᛗᛝ•ᛇᛚᛞᚣ•ᚠᚩᛞ•ᛝᚷᚾᛇ•ᚷᛖ•ᛚᛉᚣ•ᚫᛚᛖᛉ⊹ᛡᛝᛋ•ᚳᛁᚦ•ᚷᛏᚣ•ᚹᚩ•ᛝᛖ•ᛒᚪᛗᛏᚪᚷᛒ⊹ᛈᛡᛟ•ᚪᛉᛝᛒᛞᛉᛄᚦᚢ•ᛏᛇᛖ•ᚣᚪᚳ•ᛠᚦᚹ•ᛏᛉᚩᚳᛞᛒ•ᛟᚩᛠᚾᚠᚪ⊹ᛚᛗᛖᛁᚦᚫᚪᛡᛄᛁᚪᚱ•ᚦᚱᛖᛖᚣᛋᚾ•ᛖᛏᚢᚻᛈᚳᚦᛋ•ᚳᛇᛉᛖᛇᚠ•ᛞᛠᛏᛈ•ᚣᛇᛠᚢᛏ•ᛉᚦᚷᚻ•ᚫᚾᛠᚱ•ᛡᛒᛏᛁᛉ•ᚩᚢᛝ•ᛚᛒᛇᚩ•ᛟᛉ•ᚦᛞᚷᚠ•ᚩᚱᛈᚪᛏ•ᚫᛋᚪᚦ•ᛖᛟᚪᛝᚫ•ᚣᛒᛚ•ᛡᚦᚾᚠᛈᛟᛡᚾ•ᛖᚹ•ᛖᛗᚩ•ᛉᚹᚦᛠ•ᛁᚦᛒᛖᚱ•ᛟᚳᛉ•ᛈᛖ•ᛁᚢᚦ•ᛈᚠᛞᛈᛄ•ᛁᛟᚻ•ᛒᚦᛏᚩᚳᚢᛚ•ᛞᛄᛝ•ᚦᛄᛁᚪ•ᚹᚣ•ᚢᛝᚾ•ᛋᚾᛈᚠᚫᛒᛄᚫ•ᛡᛗᚹ•ᛇᚪᚩᚾᛄᚳᛚᛒᛉ•ᚣᛠᚦᚹ•ᛝᛚᛗᚳᛡᛇᚠᚫ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes47()
    {
        // Arrange
        var originalRunes = "ᛠᛁᚦ•ᛒᛠᛚᚦᚳᛞᛁᛇ•ᚠᚢᛉᛋᛉᛁᚦᚫᛋᛗ•ᚦᚹ⊹ᛈ•ᛒᛋᛏᚫᚾᚱᛁ•ᚦᛇᛡᚱᛚᛡᚹ•ᚢᚩᛋᚱ•ᚹᚫ•ᛒᚹᛡᛖᛟᛄ•ᛡᚣᛖᚩᛖᛡᚷᚫᚠᚾᚹ•ᛟᛏᚫᚠᛄᚹᛠ⊹ᚦᛞ•ᛁᚫᚩᚾ•ᛋᚷᛈᚪᛖᚩ•ᚣᚦᚹ•ᚾᚷ⊹ᛠᛋᚩᛇᛏ•ᛝᛚᚷᛞ•ᛒᛈᛈ•ᛗᛁᚪᛖ•ᛚᛏᛁ•ᚫᛄᛖ•ᛒᚾᚠᚪᛋᚷᛒᚠ•ᚫᚹᚣᚷᚢᛡᚠᛠ•ᛖᛋᛞ•ᛚᚳᛒᛞᛏᛈ•ᛖᚾᛈᚣ•ᚱᚠᚻ•ᚫᛝ•ᛟᚪᛗ•ᛒ•ᛡᛚ•ᛝᛋᚱᚢᚹᚱᚣᚻᚹ•ᚹᛡᛈ•ᛁᚻᚾᚻᚱ•ᚳᛖᛏᚫᚩᛋ•ᚣᛋ⊹ᛝᚫᛡᛝᚫ•ᚻᚦ•ᛇᚪᛞᛋ•ᛒᛁᚳᛈ•ᛇᛒᛟᚫ•ᛠᛝᛖ•ᛝᛠᚣ•ᛒᚣᛉᚻᚢᚠᚦᛞᚹ•ᛗᚢᛁᛡᛄᚩ•ᛋᛇᚫᛇᛝᚱ•ᛚᛇᛠ•ᛏᚩᛄ•ᚩᛝᛈ•ᚱᚻᛠᚢᛉᚦ•ᚣᚢᛋ•ᛡᛚᛖᚷᛗᛝᚹᚻᚱᛋ•ᚢᛟᚣᛠᚷᚩᚷ•ᛇᛁᛖ•ᛠᛄᛇᛁᚾᛄᚩᛗᚱᛡᛉ•ᚠᚻᚳ•ᚪᚩᚪᚫ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes48()
    {
        // Arrange
        var originalRunes = "ᚻᚳᛁᚦ•ᛄᚷ⊹ᛝᛖᚢ•ᛡᛏᛁ•ᛚᚩᚱᛈ⊹ᚠᚪ•ᛈᛞᚱᛒ•ᛝᛁᛋ•ᚷ•ᚠᚾᛈᚠᛒ•ᛟᚦᛁᛠᚪ•ᛡᛏᚾᚳ⊹ᚦᛟᚻᛈᛖᛚᚫ•ᛟᚠᛗ•ᛡᛝ⊹ᛒᛝᚦᛝᛠᚠ•ᛇᛗᛟ•ᚩᛠᛈ•ᛁᛡᚱ•ᚹᚹᛟᚩᛒᚩ•ᚾᚩᛄᛟᚾ•ᚦᛡᚠ•ᚩᛄᛞᚦᛏᛁ•ᛈᚾᚪᚱᛄ•ᛉᚱᚣ•ᛝᛡ•ᛏᛗ•ᛈᛞᚣᚻ•ᛗᛝᚫᚳᛇ⊹ᛡᚣᛄᛟ•ᛝᚩᚢᛇᛁᚱ•ᛏᚪ•ᚩᚻᚪᛚᚫᛚᚪ•ᛋᛈ•ᛏᚪᛄᚳᚦᚢᛏᚹᚦ•ᛗᚷᛖᛗᚣᛡᛁᛞ•ᚢᛋᚠᛒ•ᛟᛚᛟ•ᚪᛒ•ᚦᛚᚣ•ᚳᛠᚣ•ᛞᛇᛁ⊹ᚹᛉ•ᛟᛝᛒᚢᛋᛞᚻᛞ•ᚢ•ᛠᚱ•ᚫᚩᚻᛝᛒᚪᚹ•ᛈᛡᚾᛚᛇ•ᛖᛟᛝ•ᛡᚠᛇᛡ•ᚳᚦᚹ⊹ᛚᚦᚪᛁᛈ•ᛞᛟᛄ•ᚢᛉᚢᚾᛠᚠ•ᚩᚾᚪ•ᚱᛠᚷ•ᛗᚢ•ᛗᛁᛄᛒᛗᚱᚾᛗ•ᚩᚾᚠᚣ•ᛗᚠᛇᚠᛄ•ᛒᛡᛈᛄᛖᛡᛏ•ᛈᛟᚫᛏᛟ•ᚻᛖᚾ•ᚳᛇᚩ•ᛋᚻᚫᛇ•ᛝᛁᛟ•ᛇᚠᚢᛞᚣᚪᛚᚠ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes49()
    {
        // Arrange
        var originalRunes = "ᛡ•ᛖᛄ•ᚠᛚᛟ•ᛁᚳ•ᛁᛝᚷᚦ•ᛗᛋᚫᚷᚪᛠ•ᛗᛁ•ᛒᛡᛏᚾ•ᛝᛗᚦ•ᛏᚣᚫᛄ•ᛖᚻᚠᚪᛡᚷ•ᚪᛗᛁ•ᛞᛉᛏ•ᚢᛖᚦᚾ•ᛖᚪᛈᚹᛠᛚ•ᛒᚢᚱᛡᛟ•ᚪᚣ•ᛟᛇᚹᛄᛈᛞ␊"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes51()
    {
        // Arrange
        var originalRunes = "ᚹᚹᛈ•ᚠᛡᛚᛉᛒᚾ•ᚳᛗᚾᚱᛗ•ᚻᚦᚫᛞᛄ•ᛒᛡᚫ•ᛇᚹᛗᚢ•ᚪᛈᛡ•ᛈᛁᛄ•ᚪᚢᚾᛠᛖᛞᛗᚪ•ᛏᛟᛗ•ᛋᛞᛝᚷᛚᛋᛞᛝ•ᛟ•ᛋᛄᛞ•ᛚᛟᚠᛄᚫᚠᚪ•ᛝᛟᚣᛈ•ᚣᚩᛒᚷᚳᛖᛏᚹ•ᚪᛋᛒ•ᛗᛠᚣᛇᛗᚫᛚᚱ•ᚹᛇᛄᛒ•ᛈᛚᚠ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes52()
    {
        // Arrange
        var originalRunes = "ᛈ•ᚠᛗ•ᛝᚪᛇᚾᛟᚹᛇᛉ•ᚣᚫᛉᛞᛟᚱᛒ•ᛡᚱᛟ•ᚹᛏᚷᚱᛄᛖ•ᛠ•ᛈᛚᛞ•ᚻᚦᚱ•ᚦᚣᛚᛉ•ᛠᛈᚫᚠᚪ•ᚫᚪᛒ•ᛈᛋ•ᛗ•ᛏᚫᚳᛈᛝᚹᚦ•ᚻᛠ•ᛞᚩᛄᚷ•ᛋᚩᛠᚳᛖᛋ•ᚣᛖᚫ•ᛈᚦ•ᛁᛇᛈᚳᛝ⊹ᛈᚳᛇᚢᛏᚳᛡᛇᛝᚾᚢᚻᚦ•ᚣᚠᛗᚾ•ᛝᚠᛄᛉᛟᚱᛗ•ᛝᛠᛄᛏᚳ•ᚢᚷᚦ•ᚠᚦᛋ•ᚪᛈᚩᚪᚫᛞᛋᛝ•ᛒᛗᚩᚷ•ᚹᚠᛗᛖ•ᛠᛇᚻᚠᚻᚳᚱᚫ•ᛝᛗᛉᚳ•ᛋᚪᚹᛋᛠ•ᚩᚣᛚᛉᛝ•ᛠᛟᛉᛟᛠᛡᛝᛒ•ᛝᚳᚫᛁᚱ⊹ᛒᚠ•ᛏᚣᚣ•ᛠᛒ•ᚣᛚᚩ•ᛇᛉ•ᚩᚷᛗᚩ•ᚠᛚᛟᛝᚦᛠ•ᚦᚣᛖᚣ•ᚾᚷᚾ⊹ᛡᛏ•ᛄᛟᚾᛁ•ᛋᛟ•ᛠᚦᚣ•ᛋᛒ•ᚫᛚᚪᛄᛡᛖᚷᛉᛡᚾᛉᛏ•ᛡᛒᚻᛚᚷ•ᚢᚦᛠ•ᚢᚾᛁᚩᛗᛠᛁᚷ•ᛟᚦᚱᚣ•ᛒᛖᛠᚩᛈ•ᛗᛏᚱᚫᚢᚻᛁᛝ•ᛇᚳᚠ•ᛄᚾᚱᚷ•ᛟᚷᚻᚣᚻ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes53()
    {
        // Arrange
        var originalRunes = "ᛇᚫᛠᚫᚣ•ᚢᛗᛈ•ᛉᛁᚢᚾᚩᛟᚾ•ᚷᛞᚦ•ᛡᚫᚹ•ᛞᛟᛖᚱ•ᛗᚾᛖᚻᚷᛒᚢᛄ•ᚢᚦᛗᛖᛞᛝ•ᛒᚷᚣᚱ•ᛖᛁᚢᛄ•ᚣᛡᛚᚢ•ᛄᛟ⊹\"ᛠᛉᚣᛇᚱ•ᚩᛈᛋᚳᚫᛗᛇ•ᚾᛄ•ᛖᚠᛋ•ᛖᚠᚪᛝ•ᚢᛝᛄᛇᚷᚠᛝᚱᛁᚦ•ᛄᚢᚫ•ᚣᛋᚠᛖᚢᛋᚫᚣᛠ•ᛁᛏᛟᚱᛏᛟᚩ•ᚷᚾᚻ•ᛞᛗᚩᚳᛞᛖᛏ•ᚹᛉᛞᛚ•ᚩᚫᛄ•ᛇᚢᛒ\"•ᛗᛏ•ᛞᛗᛖ⊹ᛏᛈᚹᛇᛋ•ᚹᛒᛇᚦ•ᚾᚻᚷᛄ•ᚱᛡᛞᛡᚦᚪᛁᛇᚫᛉᛚ•ᛇᛠ•ᛡᚪᛄ•ᚻᚱ•ᚦᛈᛞᛄᛝᚩ•ᚷᚠᛇᛗᚳ•ᚻᛞᚩᛏᚳ•ᚢᚱ•ᛈᚾ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes54()
    {
        // Arrange
        var originalRunes = "ᚪ•ᛗᛝᛞᛡᚦᛉᛁᛗ␍ᛡᛞᛈᛝᚢᚹᚪᛗ•ᛏᚪᛝ•ᛝᚦᛡᚹᛋᚻ•ᛁᚳ•ᚫᛈᚫᚷᚩ•ᛗᛁᚪ•ᛖᚩ•ᛏᚹᚩ•ᚠᚣᚢᛏᛄ•ᚦᛄᛠᛖᚳᚾᛠ•ᚳᛠᛖ•ᚱᚩᚢᛉ•ᛞᚹᚻᛒᛝᚠᚪᚳᛄᚢ•ᚩᛄᛡᛠᛁᛚᚷᚻ•ᛒᚢᛄ•ᛉᚪᚳᚹᛡ•ᛗᚩᛈᚣᛞᛡᛚᛈ•ᛇᛁᚦᚱ•ᚣᚷᛗ•ᛉᛟᚷᛋ•ᛗᛈᛄᛟᛞ•ᛟᛏᛡᛟ•ᛏᛝᛁ•ᛗᛝᚣᚪᚫ•ᛝ•ᚱᚣᛄ•ᚾᛚᚢᛉᛒ•ᚻᛈᛄᚩᛠ•ᚷᚫᚹ•ᛉᛋᛞᚳ•ᚢᛏ•ᛟᚻᛇᚾᛈᛏ•ᛠᚣᛒᚢᚷ•ᚷᚪᛇ•ᚾᚷᚩᛖᛚᛗᛒᚦ•ᚣᛡᛟᛇᚣ•ᛗᚳᛟᚦ•ᛖᛚᚱᛇᛈᚱᛞᚣ•ᛉᛞ•ᛝᚣᛈ•ᛋᛖᛉᚹ•ᚳᚷᚠᛞᚱᛖ•ᛞᛖᚹᚩᛇᛟ•ᚻᚩᛟ•ᛒᛋ•ᚻᛠᚪᚳᛁᛗᛉᛄᛗᛖ•ᛗᛚ•ᚷᚩᛏᚦᛉᛖᛠᚱᚷᚣ"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes55()
    {
        // Arrange
        var originalRunes = "ᛝ•ᚫᛗᛁᚹ•ᛋᛒ•ᛉᛗ•ᛋᛇᚷᛞᚦᚫ•ᚠᛡᚪᛒᚳᚢ•ᚹᚱ•ᛒᛠᚠᛉᛁᛗᚢᚳᛈᚻᛝᛚᛇ•ᛗᛋᛞᛡᛈᚠ•ᛒᚻᛇᚳ•ᛇᛖ•ᛠᛖᛁᚷᛉᚷᛋ•ᛖᛋᛇᚦᚦᛖᛋ•ᚦᛟ•ᚳᛠᛁᛗᚳᛉ•ᛞᛄᚢ•ᛒᛖᛁ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes56A()
    {
        // Arrange
        var originalRunes = "ᚫᛄ•ᛟᛋᚱ␍ᛗᚣᛚᚩᚻ•ᚩᚫ•ᚳᚦᚷᚹ•ᚹᛚᚫ•ᛉᚩᚪᛈ•ᛗᛞᛞᚢᚷᚹ•ᛚ•ᛞᚾᚣᛄ•ᚳᚠᛡ•ᚫᛏᛈᛇᚪᚦ•ᚳᚫ␊"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes56B()
    {
        // Arrange
        var originalRunes = "ᚳᛞ•ᚠᚾ•ᛡᛖ•ᚠᚾᚳᛝ•ᚱᚠ•ᚫᛁᚱᛞᛖ•ᛋᚣᛄᛠᚢᛝᚹ•ᛉᚩ•ᛗᛠᚹᚠ•ᚱᚷᛡ•ᛝᚱᛒ•ᚫᚾᚢᛋ␍"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
    
    [Fact]
    public async Task TransposeRuneToLatin_AndBack_ShouldReturnOriginalRunes57()
    {
        // Arrange
        var originalRunes = "ᛈᚪᚱᚪᛒᛚᛖ␍ᛚᛁᚳᛖ•ᚦᛖ•ᛁᚾᛋᛏᚪᚱ•ᛏᚢᚾᚾᛖᛚᛝ•ᛏᚩ•ᚦᛖ•ᛋᚢᚱᚠᚪᚳᛖ⊹ᚹᛖ•ᛗᚢᛋᛏ•ᛋᚻᛖᛞ•ᚩᚢᚱ•ᚩᚹᚾ•ᚳᛁᚱᚳᚢᛗᚠᛖᚱᛖᚾᚳᛖᛋ⊹ᚠᛁᚾᛞ•ᚦᛖ•ᛞᛁᚢᛁᚾᛁᛏᚣ•ᚹᛁᚦᛁᚾ•ᚪᚾᛞ•ᛖᛗᛖᚱᚷᛖ␗"; // Example runes

        ICharacterRepo characterRepo = new CharacterRepo(); // Use the real implementation

        var transposeRuneToLatinHandler = new TransposeRuneToLatin.Handler(characterRepo);
        var transposeLatinToRuneHandler = new TransposeLatinToRune.Handler(characterRepo);

        // Act
        var latinResult = await transposeRuneToLatinHandler.Handle(new TransposeRuneToLatin.Command(originalRunes), CancellationToken.None);
        var runeResult = await transposeLatinToRuneHandler.Handle(new TransposeLatinToRune.Command(latinResult), CancellationToken.None);

        // Assert
        Assert.Equal(originalRunes, runeResult);
    }
}