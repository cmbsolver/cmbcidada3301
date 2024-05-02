namespace LiberPrimusAnalysisTool.Utility.Character
{
    /// <summary>
    /// ICharacterRepo
    /// </summary>
    public interface IPermutator
    {
        /// <summary>
        /// Gets the permutations.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        IEnumerable<string[]> GetPermutations(string[] array);
    }
}