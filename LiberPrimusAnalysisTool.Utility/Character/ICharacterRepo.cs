using System.Collections.Generic;

namespace LiberPrimusAnalysisTool.Utility.Character
{
    /// <summary>
    /// ICharacterRepo
    /// </summary>
    public interface ICharacterRepo
    {
        /// <summary>
        /// Gets the ANSI character from bin.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <returns></returns>
        string GetANSICharFromBin(string bin, bool includeControlCharacters);

        /// <summary>
        /// Gets the ANSI character from decimal.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns></returns>
        string GetANSICharFromDec(int dec, bool includeControlCharacters);

        /// <summary>
        /// Gets the ASCII character from bin.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <returns></returns>
        string GetASCIICharFromBin(string bin, bool includeControlCharacters);

        /// <summary>
        /// Gets the ASCII character from decimal.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns></returns>
        string GetASCIICharFromDec(int dec, bool includeControlCharacters);

        /// <summary>
        /// Gets the gematria strings Cicada Solver Style.
        /// </summary>
        /// <returns></returns>
        string[] GetGematriaRunes();

        /// <summary>
        /// Gets the character from rune.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        string GetCharFromRune(string value);

        /// <summary>
        /// Gets the rune from value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        string GetRuneFromValue(int value);

        /// <summary>
        /// Gets the value from rune.
        /// </summary>
        /// <param name="rune"></param>
        /// <returns></returns>
        int GetValueFromRune(string rune);

        /// <summary>
        /// Gets the permutations.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        IEnumerable<string[]> GetPermutations(string[] array);
    }
}