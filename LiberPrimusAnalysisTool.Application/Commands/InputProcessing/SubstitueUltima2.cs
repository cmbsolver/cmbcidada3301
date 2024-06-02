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
    public class SubstituteUltima2
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
            /// Transient permutator service.
            /// </summary>
            private readonly IPermutator _permutator;

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
            /// <param name="permutator">The permutator.</param>
            public Handler(ICharacterRepo characterRepo, IMediator mediator, IPermutator permutator)
            {
                _characterRepo = characterRepo;
                _mediator = mediator;
                _permutator = permutator;
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

                Tuple<string, string[]> filesContents = null;

                FileInfo fileInfo = new FileInfo(request.FileToProcess);
                var flines = File.ReadAllLines(request.FileToProcess);
                filesContents = new Tuple<string, string[]>($"{fileInfo.Name}", flines);
                
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

                var lineScore = filesContents.Item2.AsParallel().Select(x =>
                {
                    int lineNumber = Array.IndexOf(filesContents.Item2, x);
                    var scoreAndWordCount = CalculateScoreAndWordCount(
                        x,
                        lineNumber,
                        englishDictionary,
                        runes,
                        permuteRunes,
                        filesContents.Item1,
                        fileInfo);
                    return scoreAndWordCount;
                });

                var tlines = lineScore.OrderBy(x => x.Item4).Select(x => x.Item3);

                string filename =
                    $"output/POSSIBLE-MATCH{DateTime.Now.ToBinary()}-{filesContents.Item1}";
                File.AppendAllLines(filename, tlines);
            }

            /// <summary>
            /// This is to calculate the score and word count independently.
            /// </summary>
            /// <param name="line"></param>
            /// <param name="lineNumber"></param>
            /// <param name="englishDictionary"></param>
            /// <param name="runes"></param>
            /// <param name="permuteRunes"></param>
            /// <param name="fileName"></param>
            /// <param name="fileInfo"></param>
            /// <returns>The final score.</returns>
            private Tuple<string, string, string, int> CalculateScoreAndWordCount(
                string line, 
                int lineNumber,
                HashSet<string> englishDictionary, 
                string[] runes, 
                string[] permuteRunes,
                string fileName,
                FileInfo fileInfo)
            {
                if (line.Trim().Length <= 0)
                {
                    return new Tuple<string, string, string, int>(string.Empty, string.Empty, string.Empty, lineNumber);
                }
                
                Tuple<string, string, string, int> retval = null;
                double highestPercentage = 0;
                long counter = 0;
                long runCounter = 0;

                foreach (var permutation in _permutator.GetPermutations(permuteRunes))
                {
                    int score = 0;
                    int wordCount = 0;

                    counter++;
                    if (counter >= long.MaxValue - 1)
                    {
                        counter = 0;
                        runCounter++;
                    }
                    
                    if (runCounter >= long.MaxValue - 1)
                    {
                        runCounter = 0;
                    }
                    
                    HashSet<Tuple<string, string>> transcriptions = new HashSet<Tuple<string, string>>();

                    for (int i = 0; i < permutation.Length; i++)
                    {
                        transcriptions.Add(new Tuple<string, string>(runes[i], permutation[i]));
                    }

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

                    var wordList = text.Split(" ");
                    wordCount += wordList.Length;
                    foreach (var word in wordList)
                    {
                        if (englishDictionary.Contains(word))
                        {
                            score++;
                        }
                    }
                    
                    var percentage = ((double)score / (double)wordCount) * 100;
                    if (percentage > highestPercentage)
                    {
                        highestPercentage = percentage;
                        string filename =
                            $"output/text/{fileInfo.Name.Replace(".txt", string.Empty)}/BEST-POSSIBLE-MATCH-LINE-{lineNumber}-{percentage}-{fileName}";
                        File.AppendAllText(filename, $"PERCENTAGE: {percentage}\r\n");
                        File.AppendAllText(filename, $"ORIGINAL: {string.Join(",", runes)}\r\n");
                        File.AppendAllText(filename, $"REPLACE : {string.Join(",", permutation)}\r\n");
                        File.AppendAllText(filename, tmpLine.ToString());
                        
                        _messageBus.SendMessage($"PERCENTAGE: {percentage}", "SubstituteUltima");
                        
                        retval = new Tuple<string, string, string, int>(
                            string.Join(",", runes), 
                            string.Join(",", permutation), 
                            tmpLine.ToString(), 
                            lineNumber);

                        if (percentage >= 100)
                        {
                            break;
                        }
                    }
                    
                    if (percentage > 80)
                    {
                        string filename =
                            $"output/text/{fileInfo.Name.Replace(".txt", string.Empty)}/POSSIBLE-MATCH-LINE-{lineNumber}-{percentage}-{fileName}";
                        File.AppendAllText(filename, $"PERCENTAGE: {percentage}\r\n");
                        File.AppendAllText(filename, $"ORIGINAL: {string.Join(",", runes)}\r\n");
                        File.AppendAllText(filename, $"REPLACE : {string.Join(",", permutation)}\r\n");
                        File.AppendAllText(filename, tmpLine.ToString());
                        
                        _messageBus.SendMessage($"PERCENTAGE: {percentage}", "SubstituteUltima");
                    }
                }

                return retval;
            }
        }
    }
}