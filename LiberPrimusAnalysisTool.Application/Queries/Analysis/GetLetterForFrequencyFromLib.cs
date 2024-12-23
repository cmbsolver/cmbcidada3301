using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Analysis;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Queries.Analysis;

public class GetLetterForFrequencyFromLib
{
    public class Query: IRequest<LetterFrequency>
    {
        public Query(string mode)
        {
            Mode = mode;
        }

        public string Mode { get; set; }
    }
    
    public class Handler: IRequestHandler<Query, LetterFrequency>
    {
        public async Task<LetterFrequency> Handle(Query request, CancellationToken cancellationToken)
        {
            var lettersToIncludeArray = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };
            
            var runesToIncludeArray = new List<string>
            {
                "ᛝ", "ᛟ", "ᛇ", "ᛡ", "ᛠ", "ᚫ", "ᚦ", "ᚠ", "ᚢ", "ᚩ", "ᚱ", "ᚳ", "ᚷ", "ᚹ",
                "ᚻ", "ᚾ", "ᛁ", "ᛄ", "ᛈ", "ᛉ", "ᛋ", "ᛏ", "ᛒ", "ᛖ", "ᛗ", "ᛚ", "ᛞ", "ᚪ",
                "ᚣ"
            };
            
            var letterFrequency = new LetterFrequency();
            
                switch (request.Mode)
                {
                    case "runes":
                        await using (var context = new LiberContext())
                        {
                            var rletters = context.RuneTextDocumentCharacters
                                .Where(x => runesToIncludeArray.Contains(x.Character))
                                .ToList();

                            foreach (var letter in rletters)
                            {
                                letterFrequency.AddLetter(letter.Character, letter.Count);
                            }

                            letterFrequency.UpdateLetterFrequencyDetails();
                        }

                        break;
                    case "runes-med":
                        List<Tuple<long, string, long, double>> docRunes = new List<Tuple<long, string, long, double>>();
                        List<TextDocument> documents;
                        
                        await using (var context = new LiberContext())
                        {
                            documents = context.TextDocuments.ToList();
                        }

                        var parallelOptions = new ParallelOptions
                        {
                            MaxDegreeOfParallelism = Environment.ProcessorCount / 2
                        };

                        await Parallel.ForEachAsync(documents, parallelOptions, async (document, cancellationToken) =>
                        {
                            await using (var context = new LiberContext())
                            {
                                LetterFrequency docLetterFrequency = new LetterFrequency();

                                var raletters = await context.RuneTextDocumentCharacters
                                    .Where(x => runesToIncludeArray.Contains(x.Character) &&
                                                x.TextDocumentId == document.Id)
                                    .ToListAsync(cancellationToken: cancellationToken);

                                foreach (var letter in raletters)
                                {
                                    docLetterFrequency.AddLetter(letter.Character, letter.Count);
                                }

                                docLetterFrequency.UpdateLetterFrequencyDetails();

                                foreach (var letter in docLetterFrequency.LetterFrequencyDetails)
                                {
                                    docRunes.Add(new Tuple<long, string, long, double>(
                                        document.Id,
                                        letter.Letter,
                                        letter.Occurrences,
                                        letter.Frequency));
                                }
                            }
                        });
                        
                        // Now we need to get the average letter frequency for each letter
                        foreach (var rune in runesToIncludeArray)
                        {
                            var runeCounts = docRunes.Where(x => x != null && x.Item2 != null && x.Item2 == rune).Select(x => x.Item3).OrderBy(x => x);
                            var runeFrequencies = docRunes.Where(x => x != null && x.Item2 != null && x.Item2 == rune).Select(x => x.Item4).OrderBy(x => x);
                            
                            var runeCountAverage = FindMedian(runeCounts.ToArray());
                            var runeFrequencyAverage = FindMedian(runeFrequencies.ToArray());
                            letterFrequency.AddLetter(rune, runeCountAverage, runeFrequencyAverage);
                        }
                        
                        break;
                    case "letters":
                        await using (var context = new LiberContext())
                        {
                            var letters = context.TextDocumentCharacters
                                .Where(x => lettersToIncludeArray.Contains(x.Character))
                                .ToList();

                            foreach (var letter in letters)
                            {
                                letterFrequency.AddLetter(letter.Character, letter.Count);
                            }

                            letterFrequency.UpdateLetterFrequencyDetails();
                        }

                        break;
                    case "intermediary":
                        await using (var context = new LiberContext())
                        {
                            var iletters = context.LiberTextDocumentCharacters
                                .Where(x => lettersToIncludeArray.Contains(x.Character))
                                .ToList();

                            foreach (var letter in iletters)
                            {
                                letterFrequency.AddLetter(letter.Character, letter.Count);
                            }

                            letterFrequency.UpdateLetterFrequencyDetails();
                        }

                        break;
                    default:
                        await using (var context = new LiberContext())
                        {
                            var lletters = context.TextDocumentCharacters
                                .Where(x => lettersToIncludeArray.Contains(x.Character))
                                .ToList();

                            foreach (var letter in lletters)
                            {
                                letterFrequency.AddLetter(letter.Character, letter.Count);
                            }

                            letterFrequency.UpdateLetterFrequencyDetails();
                        }
                        break;
                }
            
            return letterFrequency;
        }
        
        private double FindMedian(double[] array)
        {
            long mid = Convert.ToInt64(array.Length/2);
            double median = 0;
            if (mid % 2 != 0){
                median = array[mid];
            }
            else {
                median =  (array[mid - 1] + array[mid])/2;
            }
        
            return median;
        }
        
        private long FindMedian(long[] array)
        {
            long mid = Convert.ToInt64(array.Length/2);
            long median = 0;
            if (mid % 2 != 0){
                median = array[mid];
            }
            else {
                median =  (array[mid - 1] + array[mid])/2;
            }
        
            return median;
        }
    }
}