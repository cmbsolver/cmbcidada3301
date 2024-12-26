using System.Text;
using LiberPrimusAnalysisTool.Entity.Numeric;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Math;

public class CircularShift
{
    public class Command : IRequest<string>
    {
        public Command(string text, int shift, string direction, IntBits intType, string outputType)
        {
            Text = text;
            Shift = shift;
            Direction = direction;
            IntType = intType;
            OutputType = outputType;
        }

        public string Text { get; set; }
        
        public int Shift { get; set; }
        
        public string Direction { get; set; }
        
        public IntBits IntType { get; set; }
        
        public string OutputType { get; set; }
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
            StringBuilder result = new();
            List<string> binaryStrings = new();
            request.Text = request.Text.Replace(" ", string.Empty);
            string[] words = request.Text.Split(',');
            
            if (request.Shift > IntegerUtils.ConvertToIntBitValue(request.IntType))
            {
                throw new Exception("Shift value is greater than the bit length");
            }

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].Trim();
                StringBuilder newBinaryString = new();
                string binaryString = string.Empty;
                var intType = IntegerUtils.DetectIntegerType(words[i]);
                switch (intType)
                {
                    case IntBits.Byte:
                        binaryString = Convert.ToString(Convert.ToByte(words[i]), 2);
                        break;
                    case IntBits.Short:
                        binaryString = Convert.ToString(Convert.ToInt16(words[i]), 2);
                        break;
                    case IntBits.Int:
                        binaryString = Convert.ToString(Convert.ToInt32(words[i]), 2);
                        break;
                    case IntBits.Long:
                        binaryString = Convert.ToString(Convert.ToInt64(words[i]), 2);
                        break;
                }

                if (binaryString.Length < IntegerUtils.ConvertToIntBitValue(request.IntType))
                {
                    binaryString = binaryString.PadLeft(IntegerUtils.ConvertToIntBitValue(request.IntType), '0');
                }
                else if (binaryString.Length > IntegerUtils.ConvertToIntBitValue(request.IntType))
                {
                    throw new Exception("String violates the bit length");
                }
                
                if (request.Direction.ToLower() == "left")
                {
                    var nibble = binaryString.Substring(binaryString.Length - request.Shift, request.Shift);
                    var remainder = binaryString.Substring(0, binaryString.Length - request.Shift);
                    newBinaryString.Append(nibble);
                    newBinaryString.Append(remainder);
                }
                else if (request.Direction.ToLower() == "right")
                {
                    var nibble = binaryString.Substring(0, request.Shift);
                    var remainder = binaryString.Substring(request.Shift, binaryString.Length - request.Shift);
                    newBinaryString.Append(remainder);
                    newBinaryString.Append(nibble);
                }
                
                if (newBinaryString.Length != IntegerUtils.ConvertToIntBitValue(request.IntType))
                {
                    throw new Exception("String violates the bit length");
                }
                else
                {
                    binaryStrings.Add(newBinaryString.ToString());
                }
            }

            switch (request.OutputType)
            {
                case "ANSI":
                    result.AppendLine(ConvertBinaryToANSI(binaryStrings));
                    break;
                case "ASCII":
                    result.AppendLine(ConvertBinaryToAscii(binaryStrings));
                    break;
                case "NUMBERS":
                    List<long> numbers = new();
                    foreach (var binaryString in binaryStrings)
                    {
                        var value = Convert.ToInt64(binaryString, 2);
                        numbers.Add(value);
                    }
                    result.AppendLine(string.Join(", ", numbers));
                    break;
                default:
                    List<byte> bytes = new();
                    foreach (var binaryString in binaryStrings)
                    {
                        StringBuilder binaryStringBuilder = new();
                        for (int i = 0; i < binaryString.Length; i++)
                        {
                            binaryStringBuilder.Append(binaryString[i]);
                            if (binaryStringBuilder.Length == 8)
                            {
                                var value = Convert.ToInt64(binaryString, 2);
                                if (value is > byte.MaxValue or < byte.MinValue)
                                {
                                    throw new Exception("Value is greater or less than byte max value");
                                }
                                bytes.Add(Convert.ToByte(value));
                                binaryStringBuilder.Clear();
                            }
                        }

                        await File.WriteAllBytesAsync(request.OutputType, bytes.ToArray(), cancellationToken);
                    }

                    result.AppendLine($"Please check the output file: {request.OutputType}");
                    break;
            }

            return result.ToString();
        }

        private string ConvertBinaryToAscii(List<string> binaryStrings)
        {
            StringBuilder result = new();
            foreach (var binaryString in binaryStrings)
            {
                if (binaryString.Length % 8 != 0)
                {
                    result.Append(_characterRepo.GetASCIICharFromBin(binaryString.Substring(1, 7), true));
                }
                else
                {
                    StringBuilder binaryStringBuilder = new();
                    for (int i = 0; i < binaryString.Length; i++)
                    {
                        binaryStringBuilder.Append(binaryString[i]);
                        if (binaryStringBuilder.Length % 8 == 0)
                        {
                            result.Append(_characterRepo.GetASCIICharFromBin(binaryStringBuilder.ToString(), true));
                            binaryStringBuilder.Clear();
                        }
                    }
                }
            }
            
            return result.ToString();
        }

        private string ConvertBinaryToANSI(List<string> binaryStrings)
        {
            StringBuilder result = new();
            foreach (var binaryString in binaryStrings)
            {
                if (binaryString.Length % 8 != 0)
                {
                    result.Append(_characterRepo.GetANSICharFromBin(binaryString, true));
                }
                else
                {
                    StringBuilder binaryStringBuilder = new();
                    for (int i = 0; i < binaryString.Length; i++)
                    {
                        binaryStringBuilder.Append(binaryString[i]);
                        if (binaryStringBuilder.Length % 8 == 0)
                        {
                            result.Append(_characterRepo.GetANSICharFromBin(binaryStringBuilder.ToString(), true));
                            binaryStringBuilder.Clear();
                        }
                    }
                }
            }
            
            return result.ToString();
        }
    }
}