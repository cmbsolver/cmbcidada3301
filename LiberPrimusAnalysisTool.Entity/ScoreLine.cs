using System;
using System.Collections.Generic;
using System.Linq;

namespace LiberPrimusAnalysisTool.Entity
{
    /// <summary>
    /// ScoreLine
    /// </summary>
    public class ScoreLine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreLine" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="lines">The lines.</param>
        /// <param name="englishDictionary">The english dictionary.</param>
        public ScoreLine(string fileName, string[] lines, IEnumerable<string> englishDictionary)
        {
            FileName = fileName;
            Lines = lines;
            Score = 0;
            ReadableLines = new List<string>();

            foreach (var line in lines)
            {
                List<Tuple<string, int>> wordList = new List<Tuple<string, int>>();

                foreach (var word in englishDictionary)
                {
                    if (line.ToUpper().Contains(line))
                    {
                        int index = 0;
                        while (index < line.Length)
                        {
                            var startIndex = line.ToUpper().IndexOf(word, index);

                            if (startIndex >= 0)
                            {
                                var futureIndex = startIndex + line.Length;
                                if (wordList.Any(x => x.Item2 >= startIndex && x.Item2 <= futureIndex))
                                {
                                    var itemsToDelete = wordList.Where(x => x.Item2 >= startIndex && x.Item2 <= futureIndex).ToList();
                                    foreach (var item in itemsToDelete)
                                    {
                                        wordList.Remove(item);
                                        Score = Score - item.Item1.Length;
                                    }
                                }

                                wordList.Add(new Tuple<string, int>($"{word}:{startIndex}", startIndex));
                                index = startIndex + line.Length;
                                Score = Score + word.Length;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                if (wordList.Count > 0)
                {
                    var tline = string.Join(" ", wordList.OrderBy(x => x.Item2).Select(x => x.Item1));
                    if (tline.Trim().Length > 0)
                    {
                        ReadableLines.Add(tline);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the dictionary words.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="lines">The lines.</param>
        /// <param name="englishDictionary">The english dictionary.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetDictionaryWords(string filename, string[] lines, IEnumerable<string> englishDictionary)
        {
            List<string> words = new List<string>();
            var lineCount = lines.Length;

            foreach (var line in lines)
            {
                words.AddRange(englishDictionary.AsParallel().Where(x => line.Contains(x)));
                lineCount--;
            }
            return words;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; }

        /// <summary>
        /// Gets the lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public string[] Lines { get; }

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; }

        /// <summary>
        /// Gets the readable lines.
        /// </summary>
        /// <value>
        /// The readable lines.
        /// </value>
        public List<string> ReadableLines { get; private set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{FileName} - {Score}";
        }
    }
}