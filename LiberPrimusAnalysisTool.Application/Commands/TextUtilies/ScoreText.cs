using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class ScoreText
{
    public class Command: IRequest<Tuple<ulong, double>>  
    {
        public Command(string text, List<string> wordList)
        {
            Text = text;
            WordList = wordList;
        }
        
        public string Text { get; set; }
        
        public List<string> WordList { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, Tuple<ulong, double>>
    {
        private readonly ICharacterRepo _characterRepo;
        
        public Handler(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }
        
        public async Task<Tuple<ulong, double>> Handle(Command request, CancellationToken cancellationToken)
        {
            ulong count = 0;

            await Parallel.ForEachAsync(request.WordList, async (word, cancellationToken) =>
            {
                if (request.Text.Contains(word))
                {
                    count += (ulong)word.Length ^ 2;
                }
            });
            
            double ioc = CalculateIncidenceOfCoincidence(request.Text);
            
            return new Tuple<ulong, double>(count, ioc);
        }
        
        public double CalculateIncidenceOfCoincidence(string text)
        {
            var frequencies = new Dictionary<char, int>();
            int totalLetters = 0;

            // Count the frequency of each letter in the text
            foreach (char c in text)
            {
                if (char.IsLetter(c) || _characterRepo.IsRune(c.ToString(), false))
                {
                    char upperChar = char.ToUpper(c);
                    if (frequencies.ContainsKey(upperChar))
                    {
                        frequencies[upperChar]++;
                    }
                    else
                    {
                        frequencies[upperChar] = 1;
                    }
                    totalLetters++;
                }
            }

            // Calculate the IoC
            double ioc = 0.0;
            foreach (var frequency in frequencies.Values)
            {
                ioc += frequency * (frequency - 1);
            }

            if (totalLetters > 1)
            {
                ioc /= (double)(totalLetters * (totalLetters - 1));
            }

            return ioc;
        }
        
    }
}