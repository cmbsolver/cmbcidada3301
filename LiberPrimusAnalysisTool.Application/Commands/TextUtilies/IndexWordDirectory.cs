using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class IndexWordDirectory
{
    public class Command : INotification
    {
        public Command()
        {
            
        }
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
            await using (var context = new LiberContext())
            {
                try
                {   
                    // Make sure the database is created
                    await context.Database.EnsureCreatedAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                try
                {
                    foreach (var word in context.DictionaryWords)
                    {
                        context.Remove(word);
                    }
                    await context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
            
            var processExe = Environment.ProcessPath;
            var dictionaryPath = Path.Combine(Path.GetDirectoryName(processExe) ?? string.Empty, "words.txt");
            var lines = await File.ReadAllLinesAsync(dictionaryPath, cancellationToken);
            
            await Parallel.ForEachAsync(lines, async (line, cancellationToken) =>
            {
                var word = new DictionaryWord();
                word.DictionaryWordText = line.ToUpper();

                var runeglishText =
                    await _mediator.Send(new PrepLatinToRune.Command(line.ToUpper()), cancellationToken);
                word.RuneglishWordText = runeglishText;

                var runeText = await _mediator.Send(new TransposeLatinToRune.Command(runeglishText),
                    cancellationToken);
                word.RuneWordText = runeText;

                await using (var context = new LiberContext())
                {
                    await context.DictionaryWords.AddAsync(word, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }
            });
        }
    }
}