using System.Collections.Generic;

namespace LiberPrimusAnalysisTool.Utility.Character
{
    /// <summary>
    /// CharacterRepo
    /// </summary>
    /// <seealso cref="LiberPrimusAnalysisTool.Utility.Character.ICharacterRepo" />
    public class Permutator : IPermutator
    {
        /// <summary>
        /// Gets the permutations.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public IEnumerable<string[]> GetPermutations(string[] array)
        {
            foreach (var permutation in Permute(array, 0, array.Length - 1))
            {
                yield return permutation;
            }
        }

        /// <summary>
        /// Permutes the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private IEnumerable<string[]> Permute(string[] array, int l, int r)
        {
            if (l == r)
            {
                yield return (string[])array.Clone();
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    Swap(ref array[l], ref array[i]);
                    foreach (var permutation in Permute(array, l + 1, r))
                    {
                        yield return permutation;
                    }
                    Swap(ref array[l], ref array[i]); // backtrack
                }
            }
        }

        /// <summary>
        /// Swaps the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        private void Swap(ref string a, ref string b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}