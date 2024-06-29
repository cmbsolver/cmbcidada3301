using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Image;

public class BinaryInvert
{
    public class Command: IRequest<string>
    {
        public Command(string inputFile)
        {
            InputFile = inputFile;
        }

        public string InputFile { get; set; }
    }
    
    public class Handler: IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!Directory.Exists("./output/bininvert"))
            {
                Directory.CreateDirectory("./output/bininvert");
            }
            
            var fileInfo = new FileInfo(request.InputFile);
            var outputFileName = $"./output/bininvert/{fileInfo.Name.Replace(fileInfo.Extension, "")}_inverted.bin";
            var bytes = File.ReadAllBytes(request.InputFile);
            List<byte> newBytes = new List<byte>();

            foreach (var bytedata in bytes)
            {
                StringBuilder tempByteString = new StringBuilder();
                var bBits = Convert.ToString(bytedata, 2);
                
                foreach (var bit in bBits)
                {
                    if (bit == '0')
                    {
                        tempByteString.Append('1');
                    }
                    else
                    {
                        tempByteString.Append('0');
                    }
                }
                
                newBytes.Add(Convert.ToByte(tempByteString.ToString(), 2));
                tempByteString.Clear();
            }
            
            File.WriteAllBytes(outputFileName, newBytes.ToArray());
            File.WriteAllBytes($"{outputFileName}.txt", newBytes.ToArray());
            
            var newFileInfo = new FileInfo(outputFileName);
            
            return Task.FromResult($"Completed File Inversion. File is in {newFileInfo.FullName}");
        }
    }
}