using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class IndexCharactersFromDirectory
{
    public class Command : INotification
    {
        public Command(string directoryPath, string excludedCharacters)
        {
            DirectoryPath = directoryPath;
            ExcludedCharacters = excludedCharacters;
        }

        public string DirectoryPath { get; set; }

        public string ExcludedCharacters { get; set; }
    }

    public class Handler : INotificationHandler<Command>
    {
        private readonly IMediator _mediator;

        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(Command notification, CancellationToken cancellationToken)
        {
            List<string> excludedCharacters = new();
            excludedCharacters.AddRange(notification.ExcludedCharacters.Split(','));
            if (excludedCharacters.Count != 0)
            {
                excludedCharacters.Add(",");
            }

            FileInfo fileInfo = new(Environment.ProcessPath);
            File.Delete($"{fileInfo.Directory}/liberdatabase.db");

            await using (var context = new LiberContext())
            {
                // Make sure the database is deleted
                await context.Database.EnsureDeletedAsync();

                // Make sure the database is created
                await context.Database.EnsureCreatedAsync();
            }

            await ReadDirectoryContents(notification.DirectoryPath, excludedCharacters.ToArray());
        }

        private async Task ReadDirectoryContents(string directory, string[] excludedCharacters)
        {
            var directoryInfo = new DirectoryInfo(directory);
            if (directoryInfo.GetDirectories().Length > 0)
            {
                foreach (var subDirectory in directoryInfo.GetDirectories())
                {
                    await ReadDirectoryContents(subDirectory.FullName, excludedCharacters);
                }
            }

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount / 2
            };

            await Parallel.ForEachAsync(directoryInfo.GetFiles(), options, async (file, cancellationToken) =>
            {
                if (file.FullName.ToLower().EndsWith(".txt"))
                {
                    await ReadAndIndexFileContents(file.FullName, excludedCharacters);
                }
            });
        }

        private async Task ReadAndIndexFileContents(string file, string[] excludedCharacters)
        {
            FileInfo fileInfo = new(file);

            await using (var context = new LiberContext())
            {
                try
                {
                    // Reading in the file contents
                    var lines = await File.ReadAllLinesAsync(file);
                    List<string> liberLines = new();

                    // Prepping text for liber conversion - will need this to perform liber analysis.
                    foreach (var line in lines)
                    {
                        var liberLine = await _mediator.Send(new PrepLatinToRune.Command(line));
                        liberLines.Add(liberLine);
                    }

                    TextDocument? textDocument = context.TextDocuments.FirstOrDefault(x => x.FileName == fileInfo.Name);
                    if (textDocument == null)
                    {
                        textDocument = new TextDocument
                        {
                            FileName = fileInfo.Name,
                        };
                        await context.TextDocuments.AddAsync(textDocument);
                        await context.SaveChangesAsync();
                    }

                    // In case we don't get the ID.
                    textDocument = context.TextDocuments.FirstOrDefault(x => x.FileName == fileInfo.Name);

                    if (textDocument == null || textDocument.Id == 0)
                    {
                        throw new Exception("Could not get the ID of the text document.");
                    }

                    List<TextDocumentCharacter> textDocumentCharacters = new List<TextDocumentCharacter>();
                    foreach (var line in lines)
                    {
                        foreach (var xcharacter in line)
                        {
                            var character = xcharacter.ToString().ToUpper();
                            if (excludedCharacters.Contains(character))
                            {
                                continue;
                            }

                            if (textDocumentCharacters.Any(x => x.Character == character))
                            {
                                var textDocumentCharacter = textDocumentCharacters.First(x => x.Character == character);
                                textDocumentCharacter.Count++;
                            }
                            else
                            {
                                var textDocumentCharacter = new TextDocumentCharacter
                                {
                                    Character = character,
                                    Count = 1,
                                    TextDocumentId = textDocument.Id
                                };
                                textDocumentCharacters.Add(textDocumentCharacter);
                            }
                        }
                    }

                    foreach (var textDocumentCharacter in textDocumentCharacters)
                    {
                        try
                        {
                            var item = await context.TextDocumentCharacters.FirstOrDefaultAsync(x =>
                                x.Character == textDocumentCharacter.Character &&
                                x.TextDocumentId == textDocumentCharacter.TextDocumentId);

                            if (item == null)
                            {
                                await context.AddAsync(textDocumentCharacter);
                                await context.SaveChangesAsync();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Character: {textDocumentCharacter.Character}");
                            Console.WriteLine(e.ToString());
                        }

                    }

                    List<LiberTextDocumentCharacter> liberTextDocumentCharacters =
                        new List<LiberTextDocumentCharacter>();
                    foreach (var line in liberLines)
                    {
                        foreach (var xcharacter in line)
                        {
                            var character = xcharacter.ToString().ToUpper();
                            if (excludedCharacters.Contains(character))
                            {
                                continue;
                            }

                            if (liberTextDocumentCharacters.Any(x => x.Character == character))
                            {
                                var liberTextDocumentCharacter =
                                    liberTextDocumentCharacters.First(x => x.Character == character);
                                liberTextDocumentCharacter.Count++;
                            }
                            else
                            {
                                var liberTextDocumentCharacter = new LiberTextDocumentCharacter
                                {
                                    Character = character,
                                    Count = 1,
                                    TextDocumentId = textDocument.Id
                                };
                                liberTextDocumentCharacters.Add(liberTextDocumentCharacter);
                            }
                        }
                    }

                    foreach (var liberTextDocumentCharacter in liberTextDocumentCharacters)
                    {
                        try
                        {
                            var item = await context.LiberTextDocumentCharacters.FirstOrDefaultAsync(x =>
                                x.Character == liberTextDocumentCharacter.Character &&
                                x.TextDocumentId == liberTextDocumentCharacter.TextDocumentId);

                            if (item == null)
                            {
                                await context.AddAsync(liberTextDocumentCharacter);
                                await context.SaveChangesAsync();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Character: {liberTextDocumentCharacter.Character}");
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    await context.Database.CloseConnectionAsync();    
                }
            }
        }
    }
}