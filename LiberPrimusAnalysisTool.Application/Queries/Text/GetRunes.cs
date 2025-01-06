using LiberPrimusAnalysisTool.Entity.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Text;

public class GetRunes
{
    public class Query: IRequest<RuneDetail[]>
    {
        
    }
    
    public class Handler: IRequestHandler<Query, RuneDetail[]>
    {
        private readonly ICharacterRepo _characterRepo;
        
        public Handler(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }
        
        public Task<RuneDetail[]> Handle(Query request, CancellationToken cancellationToken)
        {
            List<RuneDetail> runeDetails = new();
            var runes = _characterRepo.GetGematriaRunes();

            foreach (var rune in runes)
            {
                runeDetails.Add(new RuneDetail
                (
                    rune,
                    _characterRepo.GetCharFromRune(rune),
                    _characterRepo.GetValueFromRune(rune)
                ));
            }

            runeDetails = runeDetails.OrderBy(x => x.Value).ToList();
            
            return Task.FromResult(runeDetails.ToArray());
        }
    }
}