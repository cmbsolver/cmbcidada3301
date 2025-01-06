using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Text;

public class GetWordsFromInts
{
    public class Command : IRequest<IEnumerable<DictionaryWord>>
    {
        public string Ints { get; set; }

        public Command(string ints)
        {
            Ints = ints;
        }
    }

    public class Handler : IRequestHandler<Command, IEnumerable<DictionaryWord>>
    {
        public async Task<IEnumerable<DictionaryWord>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<ulong> ints = new();
            List<DictionaryWord> words = new();

            if (request.Ints.Contains(","))
            {
                ints.AddRange(request.Ints.Split(",").Select(x => Convert.ToUInt64(x.Trim())));
            }
            else
            {
                ints.Add(Convert.ToUInt64(request.Ints));
            }

            ints = ints.OrderBy(x => x).ToList();

            foreach (var value in ints)
            {
                using (var context = new LiberContext())
                {
                    words = context.DictionaryWords.Where(x => x.GemSum == (long)value).ToList();
                }
            }

            return words;
        }
    }
}