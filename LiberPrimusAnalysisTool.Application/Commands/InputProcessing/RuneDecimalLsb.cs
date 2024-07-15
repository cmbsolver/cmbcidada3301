using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing;

public class RuneDecimalLsb
{
    public class Command : IRequest<string>
    {
        /// <summary>
        /// The command
        /// </summary>
        /// <param name="outputType"></param>
        /// <param name="pageSelection"></param>
        /// <param name="includeControlCharacters"></param>
        /// <param name="reverseBytes"></param>
        /// <param name="shiftSequence"></param>
        /// <param name="bitsOfInsignificance"></param>
        /// <param name="discardRemainder"></param>
        public Command(string outputType, string pageSelection, bool includeControlCharacters, bool reverseBytes, bool shiftSequence, int bitsOfInsignificance, bool discardRemainder)
        {
            OutputType = outputType;
            PageSelection = pageSelection;
            IncludeControlCharacters = includeControlCharacters;
            ReverseBytes = reverseBytes;
            ShiftSequence = shiftSequence;
            BitOfInsignificance = bitsOfInsignificance;
            DiscardRemainder = discardRemainder;
        }

        public string OutputType { get; set; }
        
        public string PageSelection { get; set; }

        public bool IncludeControlCharacters { get; set; }

        public bool ReverseBytes { get; set; }

        public bool ShiftSequence { get; set; }

        public int BitOfInsignificance { get; set; }

        public bool DiscardRemainder { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, string>
    {
        private readonly ICharacterRepo _characterRepo;
        
        public Handler(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }
        
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            StringBuilder retval = new StringBuilder();
            string fileContent = File.ReadAllText(request.PageSelection);
            StringBuilder binString = new StringBuilder();
            
            try
            {
                if (request.OutputType == "string")
                {
                    foreach (var character in fileContent)
                    {
                        var decValue = _characterRepo.GetValueFromRune(character.ToString());
                        if (decValue > 0)
                        {
                            var binaryValue = Convert.ToString(decValue, 2).PadLeft(8, '0');
                            var lsbValue = binaryValue.Substring(binaryValue.Length - request.BitOfInsignificance, request.BitOfInsignificance);
                            binString.Append(lsbValue);
                        }
                    }
                    
                    if (request.ReverseBytes)
                    {
                        var revered = binString.ToString().Reverse();
                        binString = new StringBuilder();
                        binString.Append(string.Join("", revered));
                    }
                    
                    StringBuilder binChars = new StringBuilder();
                    foreach (var character in binString.ToString())
                    {
                        binChars.Append(character);
                        
                        if (binChars.Length == 8)
                        {
                            retval.Append(_characterRepo.GetANSICharFromBin(binChars.ToString(), request.IncludeControlCharacters));
                            binChars.Clear();
                        }
                    }
                }
                else if (request.OutputType == "file")
                {
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return retval.ToString();
        }
    }
}