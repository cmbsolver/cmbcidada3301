using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Text;

public class GetWordsFromLengths
{
    public class Command : IRequest<IEnumerable<string>>
    {
        public string Ints { get; set; }

        public string Catalog { get; set; }

        public Command(string ints, string catalog)
        {
            Ints = ints;
            Catalog = catalog;
        }
    }

    public class Handler : IRequestHandler<Command, IEnumerable<string>>
    {
        private readonly MediatR.IMediator _mediator;

        public Handler(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<int> ints = new();
            List<string> words = new();

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
                    foreach (var word in context.DictionaryWords)
                    {
                        switch (request.Catalog)
                        {
                            case "Regular":
                                if (word.DictionaryWordText.Length == value)
                                {
                                    words.Add(word.DictionaryWordText);
                                }
                                break;
                            case "Runeglish":
                                if (word.RuneglishWordText.Length == value)
                                {
                                    words.Add(word.RuneglishWordText);
                                }
                                break;
                            case "Runes":
                                if (word.RuneWordText.Length == value)
                                {
                                    words.Add(word.RuneWordText);
                                }
                                break;
                        }
                    }
                }
            }

            return words;
        }
    }
}