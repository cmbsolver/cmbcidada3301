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
                List<string> englishDictionary = new List<string>();

                var isGpStrict = AnsiConsole.Confirm("Use GP spellings?", true);

                var allFiles = await _mediator.Send(new GetTextSelection.Query());

                string[] runes = _characterRepo.GetGematriaRunes();
                string[] permuteRunes = _characterRepo.GetGematriaRunes().Reverse().ToArray();

                List<Tuple<string, string[]>> filesContents = new List<Tuple<string, string[]>>();

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
                                    englishDictionary.Add(line.ToUpper().Trim().Replace("QU", "CW").Replace("Q", "C").Replace("K", "C").Replace("V", "U").Replace("Z", "S"));
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

                        long counter = 0;
                        foreach (var permutation in _characterRepo.GetPermutations(permuteRunes))
                        {
                            counter++;
                            if (counter == long.MaxValue - 1)
                            {
                                AnsiConsole.WriteLine($"Processed {counter} - {DateTime.Now}");
                                counter = 0;
                            }

                            List<Tuple<string, string>> transcriptions = new List<Tuple<string, string>>();

                            for (int i = 0; i < permutation.Length; i++)
                            {
                                transcriptions.Add(new Tuple<string, string>(runes[i], permutation[i]));
                            }

                            Parallel.ForEach(filesContents, file =>
                            {
                                List<string> tlines = new List<string>();

                                for (int i = 0; i < file.Item2.Length; i++)
                                {
                                    StringBuilder tmpLine = new StringBuilder();

                                    for (int j = 0; j < file.Item2[i].Length; j++)
                                    {
                                        var transcription = transcriptions.FirstOrDefault(x => x.Item1 == file.Item2[i][j].ToString());

                                        if (transcription != null)
                                        {
                                            tmpLine.Append(_characterRepo.GetCharFromRune(transcription.Item2));
                                        }
                                        else
                                        {
                                            tmpLine.Append(file.Item2[i][j]);
                                        }
                                    }

                                    tlines.Add(tmpLine.ToString());
                                }

                                int score = 0;
                                int wordCount = 0;
                                for (int i = 0; i < tlines.Count; i++)
                                {
                                    if (tlines[i].Trim().Length == 0)
                                    {
                                        continue;
                                    }

                                    var wordList = tlines[i].Trim().Split(" ");
                                    wordCount += wordList.Length;
                                    foreach (var word in wordList)
                                    {
                                        if (englishDictionary.Contains(word))
                                        {
                                            score++;
                                        }
                                    }
                                }

                                // This is if the dictionary matches the word count.
                                var percentage = ((double)score / (double)wordCount) * 100;
                                if (score == wordCount || percentage > 90)
                                {
                                    AnsiConsole.WriteLine($"File: {file.Item1} - {DateTime.Now}");
                                    string filename = $"output/POSSIBLE-MATCH{DateTime.Now.ToBinary()}-{file.Item1}";
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
                                            AnsiConsole.WriteLine($"File: {file.Item1} - Highest Percentage: {percentage}");
                                            fileLeader[file.Item1] = percentage;
                                            fileContents[file.Item1] = tlines.ToArray();
                                        }
                                    }
                                    else
                                    {
                                        fileLeader.Add(file.Item1, percentage);
                                        fileContents.Add(file.Item1, tlines.ToArray());
                                    }
                                }
                            });
                        }

                        foreach (var file in fileContents)
                        {
                            string filename = $"output/{file.Key}";
                            File.AppendAllLines(filename, file.Value);
                        }
                    });
            }
        }
    }
}