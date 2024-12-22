using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class TransposeLatinToRune
{
    public class Command : IRequest<string>
    {
        public Command(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, string>
    {
        private readonly ICharacterRepo _characterRepo;
        public Handler(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }
        
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();
            
            request.Text = request.Text.ToUpper();
            
            for (int i = 0; i < request.Text.Length; i++)
            {
                var xchar = request.Text[i];
                if (!_characterRepo.IsRune(xchar.ToString()))
                {
                    switch (xchar)
                    {
                        case 'A':
                            if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'E')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("AE"));
                                i++;
                            }
                            else
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("A"));
                            }

                            break;
                        case 'E':
                            if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'A')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("EA"));
                                i++;
                            }
                            else if(((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'O')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("EO"));
                                i++;
                            }
                            else
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("E"));
                            }

                            break;
                        
                        case 'O':
                            if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'E')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("OE"));
                                i++;
                            }
                            else
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("O"));
                            }

                            break;
                        
                        case 'T':
                            if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'H')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("TH"));
                                i++;
                            }
                            else
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("T"));
                            }

                            break;
                        
                        case 'I':
                            if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'O')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("IO"));
                                i++;
                            }
                            else if(((i + 2) < (request.Text.Length)) && request.Text[i + 1] == 'N' && request.Text[i + 2] == 'G')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("ING"));
                                i += 2;
                            }
                            else if(((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'A')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("IA"));
                                i++;
                            }
                            else
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("I"));
                            }
                            
                            break;
                        
                        case 'N':
                            if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'G')
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("NG"));
                                i++;
                            }
                            else
                            {
                                sb.Append(_characterRepo.GetRuneFromChar("N"));
                            }

                            break;
                        
                        default:
                            sb.Append(_characterRepo.GetRuneFromChar(xchar.ToString()));
                            break;
                    }
                }
                else
                {
                    sb.Append(_characterRepo.GetRuneFromChar(xchar.ToString()));
                }
            }
            
            return Task.FromResult(sb.ToString());
        }
    }
}