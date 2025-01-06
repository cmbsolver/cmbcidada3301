using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Text;

public class GetWordsFromLengths
{
    public class Command : IRequest<IEnumerable<DictionaryWord>>
    {
        public string Ints { get; set; }

        public string Catalog { get; set; }

        public Command(string ints, string catalog)
        {
            Ints = ints;
            Catalog = catalog;
        }
    }

    public class Handler : IRequestHandler<Command, IEnumerable<DictionaryWord>>
    {
        private readonly MediatR.IMediator _mediator;

        public Handler(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<DictionaryWord>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<int> ints = new();
            List<DictionaryWord> words = new();

            if (request.Ints.Contains(","))
            {
                ints.AddRange(request.Ints.Split(",").Select(x => Convert.ToInt32(x.Trim())));
            }
            else
            {
                ints.Add(Convert.ToInt32(request.Ints));
            }

            ints = ints.OrderBy(x => x).ToList();

            foreach (var value in ints)
            {
                using (var context = new LiberContext())
                {
                    switch (request.Catalog)
                    {
                        case "Regular":
                            words = context.DictionaryWords.Where(x => x.DictionaryWordLength == value).ToList();
                            break;
                        case "Runeglish":
                            words = context.DictionaryWords.Where(x => x.RuneglishWordLength == value).ToList();
                            break;
                        case "Runes":
                            words = context.DictionaryWords.Where(x => x.RuneWordLength == value).ToList();
                            break;
                    }
                }
            }

            return words.OrderBy(x => x.DictionaryWordText).ToList();
        }
    }
}