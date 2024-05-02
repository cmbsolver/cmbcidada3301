using LiberPrimusAnalysisTool.Application.Queries.Selection;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using Spectre.Console;
using System.Text;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Get Word From Ints
    /// </summary>
    public class SubstituteUltima
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : INotification
        {
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : INotificationHandler<Command>
        {
            /// <summary>
            /// The character repo
            /// </summary>
            private readonly ICharacterRepo _characterRepo;

            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="characterRepo">The character repo.</param>
            /// <param name="mediator">The mediator.</param>
            public Handler(ICharacterRepo characterRepo, IMediator mediator)
            {
                _characterRepo = characterRepo;
                _mediator = mediator;
            }

            /// <summary>
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                HashSet<string> englishDictionary = new HashSet<string>();

                var isGpStrict = AnsiConsole.Confirm("Use GP spellings?", true);

                var allFiles = await _mediator.Send(new GetTextSelection.Query(false));

                string[] runes = _characterRepo.GetGematriaRunes();
                string[] permuteRunes = _characterRepo.GetGematriaRunes().Reverse().ToArray();

                HashSet<Tuple<string, string[]>> filesContents = new HashSet<Tuple<string, string[]>>();

                Dictionary<string, double> fileLeader = new Dictionary<string, double>();
                Dictionary<string, string[]> fileContents = new Dictionary<string, string[]>();

                foreach (var file in allFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    var flines = File.ReadAllLines(file);

                    filesContents.Add(new Tuple<string, string[]>($"{fileInfo.Name}", flines));
                }

                AnsiConsole.Status()
                    .AutoRefresh(true)
                    .Spinner(Spinner.Known.Circle)
                    .SpinnerStyle(Style.Parse("green bold"))
                    .Start("Processing files...", ctx =>
                    {
                        ctx.Status("Reading dictionary...");
                        ctx.Refresh();
                        using (var file = File.OpenText("words.txt"))
                        {
                            string line;
                            while ((line = file.ReadLine()) != null)
                            {
                                if (isGpStrict)
                                {
                                    englishDictionary.Add(line.ToUpper().Trim().Replace("QU", "CW").Replace("Q", "C")
                                        .Replace("K", "C").Replace("V", "U").Replace("Z", "S"));
                                }
                                else
                                {
                                    englishDictionary.Add(line.ToUpper().Trim());
                                }
                            }

                            file.Close();
                            file.Dispose();
                        }

                        ctx.Status("Processing");
                        ctx.Refresh();

                        long counterWrite = 0;
                        long runNumber = 0;
                        long lastRunNumber = 0;
                        long lastWrite = 0;

                        if (File.Exists("lastrun.txt"))
                        {
                            string lastRun = File.ReadAllText("lastrun.txt");
                            string[] lastRunSplit = lastRun.Split(":");
                            lastRunNumber = long.Parse(lastRunSplit[0]);
                            lastWrite = long.Parse(lastRunSplit[1]);
                        }

                        foreach (var permutation in _characterRepo.GetPermutations(permuteRunes))
                        {
                            counterWrite++;
                            if (counterWrite >= (long.MaxValue - 1))
                            {
                                counterWrite = 0;
                                runNumber++;
                                File.WriteAllText("lastrun.txt", $"{runNumber}:{counterWrite}");
                            }
                            else if (counterWrite % 3301 == 0)
                            {
                                AnsiConsole.Write($"Processed: {runNumber}:{counterWrite} run permutations - {DateTime.Now}");
                                File.WriteAllText("lastrun.txt", $"{runNumber}:{counterWrite}");
                            }

                            if (runNumber >= lastRunNumber && counterWrite >= lastWrite)
                            {
                                HashSet<Tuple<string, string>> transcriptions = new HashSet<Tuple<string, string>>();

                                for (int i = 0; i < permutation.Length; i++)
                                {
                                    transcriptions.Add(new Tuple<string, string>(runes[i], permutation[i]));
                                }

                                foreach (var file in filesContents)
                                {
                                    var lineScore = file.Item2.AsParallel().Select(x =>
                                    {
                                        int lineNumber = Array.IndexOf(file.Item2, x);
                                        var scoreAndWordCount = CalculateScoreAndWordCount(x, lineNumber, transcriptions,
                                            englishDictionary);
                                        return scoreAndWordCount;
                                    });
                                    
                                    var wordCount = lineScore.Sum(x => x.Item1);
                                    var score = lineScore.Sum(x => x.Item2);
                                    var tlines = lineScore.OrderBy(x => x.Item4).Select(x => x.Item3);

                                    // This is if the dictionary matches the word count.
                                    var percentage = ((double)score / (double)wordCount) * 100;
                                    if (score == wordCount || percentage > 80)
                                    {
                                        AnsiConsole.WriteLine($"File: {file.Item1} - {DateTime.Now}");
                                        string filename =
                                            $"output/POSSIBLE-MATCH{DateTime.Now.ToBinary()}-{file.Item1}";
                                        File.AppendAllText(filename, $"PERCENTAGE: {percentage}\r\n");
                                        File.AppendAllText(filename, $"ORIGINAL: {string.Join(",", runes)}\r\n");
                                        File.AppendAllText(filename, $"REPLACE : {string.Join(",", permutation)}\r\n");
                                        File.AppendAllLines(filename, tlines);
                                    }
                                    else
                                    {
                                        if (fileLeader.ContainsKey(file.Item1))
                                        {
                                            if (percentage > fileLeader[file.Item1])
                                            {
                                                AnsiConsole.WriteLine(
                                                    $"File: {file.Item1} - Highest Percentage: {percentage}");
                                                fileLeader[file.Item1] = percentage;
                                                fileContents[file.Item1] = tlines.ToArray();

                                                string filename =
                                                    $"output/POSSIBLE-MATCH{DateTime.Now.ToBinary()}-{file.Item1}";
                                                File.AppendAllText(filename, $"PERCENTAGE: {percentage}\r\n");
                                                File.AppendAllText(filename,
                                                    $"ORIGINAL: {string.Join(",", runes)}\r\n");
                                                File.AppendAllText(filename,
                                                    $"REPLACE : {string.Join(",", permutation)}\r\n");
                                                File.AppendAllLines(filename, tlines);
                                            }
                                        }
                                        else
                                        {
                                            fileLeader.Add(file.Item1, percentage);
                                            fileContents.Add(file.Item1, tlines.ToArray());
                                        }
                                    }
                                }
                            }
                        }
                    });
            }

            private Tuple<int, int, string, int> CalculateScoreAndWordCount(string line, int lineNumber,
                HashSet<Tuple<string, string>> transcriptions, HashSet<string> englishDictionary)
            {
                if (line.Trim().Length <= 0)
                {
                    return new Tuple<int, int, string, int>(0, 0, string.Empty, lineNumber);
                }
                
                Tuple<int, int, string, int> retval = null;
                int score = 0;
                int wordCount = 0;

                StringBuilder tmpLine = new StringBuilder();

                for (int j = 0; j < line.Length; j++)
                {
                    var transcription = transcriptions.FirstOrDefault(x => x.Item1 == line[j].ToString());

                    if (transcription != null)
                    {
                        tmpLine.Append(_characterRepo.GetCharFromRune(transcription.Item2));
                    }
                    else
                    {
                        tmpLine.Append(line[j]);
                    }
                }

                var wordList = tmpLine.ToString().Trim().Split(" ");
                wordCount += wordList.Length;
                foreach (var word in wordList)
                {
                    if (englishDictionary.Contains(word))
                    {
                        score++;
                    }
                }

                retval = new Tuple<int, int, string, int>(wordCount, score, tmpLine.ToString(), lineNumber);
                return retval;
            }
        }
    }
}