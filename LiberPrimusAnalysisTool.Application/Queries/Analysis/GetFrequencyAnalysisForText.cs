using LiberPrimusAnalysisTool.Entity.Analysis;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Analysis;

public class GetFrequencyAnalysisForText
{
    public class Query: IRequest<LetterFrequency>
    {
        public Query(string text)
        {
            Text = text;
        }
        
        public string Text { get; private set; }
    }
    
    public class Handler: IRequestHandler<Query, LetterFrequency>
    {
        public Task<LetterFrequency> Handle(Query request, CancellationToken cancellationToken)
        {
            var letterFrequency = new LetterFrequency();
            
            foreach (var letter in request.Text)
            {
                letterFrequency.AddLetter(letter.ToString());
            }

            letterFrequency.UpdateLetterFrequencyDetails();
            
            return Task.FromResult(letterFrequency);
        }
    }
}