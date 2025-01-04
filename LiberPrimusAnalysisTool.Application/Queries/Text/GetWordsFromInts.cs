using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Text;

public class GetWordsFromInts
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
    
    public class Handler: IRequestHandler<Command, IEnumerable<string>>
    {
        private readonly MediatR.IMediator _mediator;
        
        public Handler(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<ulong> ints = new();
            List<string> words = new();
            
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
                    foreach (var word in context.DictionaryWords)
                    {
                        var gemSum = await _mediator.Send(new CalculateGematriaSum.Command(word.RuneWordText));

                        if (Convert.ToUInt64(gemSum) == value)
                        {
                            switch (request.Catalog)
                            {
                                case "Regular":
                                    words.Add(word.DictionaryWordText);
                                    break;
                                case "Runeglish":
                                    words.Add(word.RuneglishWordText);
                                    break;
                                case "Runes":
                                    words.Add(word.RuneWordText);
                                    break;
                            }
                        }
                    }
                }   
            }
            
            return words;
        }
    }
}