using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Utility.Message;

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
            public Command(string fileToProcess)
            {
                FileToProcess = fileToProcess;
            }

            public string FileToProcess { get; set; }
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
            /// The message bus
            /// </summary>
            private readonly IMessageBus _messageBus;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="characterRepo">The character repo.</param>
            /// <param name="mediator">The mediator.</param>
            public Handler(ICharacterRepo characterRepo, IMediator mediator, IMessageBus messageBus)
            {
                _characterRepo = characterRepo;
                _mediator = mediator;
                _messageBus = messageBus;
            }

            /// <summary>
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                HashSet<string> englishDictionary = new HashSet<string>();

                var isGpStrict = true;

                string[] runes = _characterRepo.GetGematriaRunes();
                string[] permuteRunes = _characterRepo.GetGematriaRunes().Reverse().ToArray();

                HashSet<Tuple<string, string[]>> filesContents = new HashSet<Tuple<string, string[]>>();

                Dictionary<string, double> fileLeader = new Dictionary<string, double>();
                Dictionary<string, string[]> fileContents = new Dictionary<string, string[]>();

                FileInfo fileInfo = new FileInfo(request.FileToProcess);
                var flines = File.ReadAllLines(request.FileToProcess);
                filesContents.Add(new Tuple<string, string[]>($"{fileInfo.Name}", flines));
                
                if (!Directory.Exists($"output/text/{fileInfo.Name.Replace(".txt", string.Empty)}"))
                {
                    Directory.CreateDirectory($"output/text/{fileInfo.Name.Replace(".txt", string.Empty)}");
                }

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
                        _messageBus.SendMessage($"{runNumber}:{counterWrite}", "SubstituteUltima");
                    }
                    else if (counterWrite % 3301 == 0)
                    {
                        File.WriteAllText("lastrun.txt", $"{runNumber}:{counterWrite}");
                        _messageBus.SendMessage($"{runNumber}:{counterWrite}", "SubstituteUltima");
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
                                string filename =
                                    $"output/text/{fileInfo.Name.Replace(".txt", string.Empty)}/POSSIBLE-MATCH{percentage}-{fileInfo.Name.Replace(".txt", string.Empty)}";
                                File.AppendAllText(filename, $"PERCENTAGE: {percentage}\r\n");
                                File.AppendAllText(filename, $"ORIGINAL: {string.Join(",", runes)}\r\n");
                                File.AppendAllText(filename, $"REPLACE : {string.Join(",", permutation)}\r\n");
                                File.AppendAllLines(filename, tlines);
                                
                                _messageBus.SendMessage($"PERCENTAGE: {percentage}", "SubstituteUltima");
                            }
                            else
                            {
                                if (fileLeader.ContainsKey(file.Item1))
                                {
                                    if (percentage > fileLeader[file.Item1])
                                    {
                                        fileLeader[file.Item1] = percentage;
                                        fileContents[file.Item1] = tlines.ToArray();

                                        string filename =
                                            $"output/text/{fileInfo.Name.Replace(".txt", string.Empty)}/POSSIBLE-MATCH{percentage}-{fileInfo.Name.Replace(".txt", string.Empty)}";
                                        File.AppendAllText(filename, $"PERCENTAGE: {percentage}\r\n");
                                        File.AppendAllText(filename,
                                            $"ORIGINAL: {string.Join(",", runes)}\r\n");
                                        File.AppendAllText(filename,
                                            $"REPLACE : {string.Join(",", permutation)}\r\n");
                                        File.AppendAllLines(filename, tlines);
                                        
                                        _messageBus.SendMessage($"PERCENTAGE: {percentage}", "SubstituteUltima");
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
                
                var text = _mediator.Send(new FixUpControlChars.Command(tmpLine.ToString())).Result;
                
                var wordList = text.Trim().Split(" ");
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