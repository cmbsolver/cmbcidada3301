using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// This is to brute force the fiestel cipher.
    /// </summary>
    public class BruteForceFiestel
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : INotification
        {
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : INotificationHandler<Command>
        {
            /// <summary>
            /// The character repo
            /// </summary>
            private readonly ICharacterRepo _characterRepo;

            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;
            
            /// <summary>
            /// number of rounds to transform data block, each round a new "round" key is generated. 
            /// </summary>
            private const int _rounds = 32;

            /// <summary>
            /// int to be used in decryption
            /// </summary>
            private static readonly uint[] _decryptionInts = new uint[]
                { 3299, 3298, 2838, 3288, 3294, 3296, 2472, 4516, 1206, 708, 1820, 3258, 3222, 3152, 3038, 3278 };

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="characterRepo">The character repo.</param>
            /// <param name="mediator">The mediator.</param>
            public Handler(ICharacterRepo characterRepo, IMediator mediator)
            {
                _characterRepo = characterRepo;
                _mediator = mediator;
            }

            /// <summary>
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var allFiles = new string[0]; //await _mediator.Send(new GetTextSelection.Query(false));
                HashSet<Tuple<string, string[]>> filesContents = new HashSet<Tuple<string, string[]>>();

                foreach (var file in allFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    var flines = File.ReadAllLines(file);

                    filesContents.Add(new Tuple<string, string[]>($"{fileInfo.Name}", flines));
                }

                Parallel.ForEach(filesContents, file =>
                {
                    List<string> tlines = new List<string>();

                    for (int i = 0; i < file.Item2.Length; i++)
                    {
                        StringBuilder tmpLine = new StringBuilder();

                        for (int j = 0; j < file.Item2[i].Length; j++)
                        {
                            tmpLine.Append(_characterRepo.GetCharFromRune(file.Item2[i][j].ToString()));
                        }

                        tlines.Add(tmpLine.ToString());
                    }

                    for (int i = 0; i < tlines.Count; i++)
                    {
                        string tline = tlines[i];
                        foreach (var dint in _decryptionInts)
                        {
                            while (tline.Length % 16 != 0)
                            {
                                tline += " ";
                            }

                            var decodedString = Decode(tline, dint);
                        }
                    }
                });
            }

            /// <summary>
            /// Decodes text that was encoded using specified key.
            /// </summary>
            /// <param name="text">Text to be decoded.</param>
            /// <param name="key">Key that was used to encode the text.</param>
            /// <exception cref="ArgumentException">Error: key should be more than 0x00001111 for better encoding, key=0 will throw DivideByZero exception.</exception>
            /// <exception cref="ArgumentException">Error: The length of text should be divisible by 16 as it the block lenght is 16 bytes.</exception>
            /// <returns>Decoded text.</returns>
            public string Decode(string text, uint key)
            {
                // The plain text will be padded to fill the size of block (16 bytes)
                if (text.Length % 16 != 0)
                {
                    throw new ArgumentException($"The length of {nameof(key)} should be divisible by 16");
                }

                List<ulong> blocksListEncoded = GetBlocksFromEncodedText(text);
                StringBuilder decodedTextHex = new();

                foreach (ulong block in blocksListEncoded)
                {
                    uint temp = 0;

                    // decompose a block to two subblocks 0x0123456789ABCDEF => 0x01234567 & 0x89ABCDEF
                    uint rightSubblock = (uint)(block & 0x00000000FFFFFFFF);
                    uint leftSubblock = (uint)(block >> 32);

                    // Feistel "network" - decoding, the order of rounds and operations on the blocks is reverted
                    uint roundKey;
                    for (int round = _rounds - 1; round >= 0; round--)
                    {
                        roundKey = GetRoundKey(key, round);
                        temp = leftSubblock ^ BlockModification(rightSubblock, roundKey);
                        leftSubblock = rightSubblock;
                        rightSubblock = temp;
                    }

                    // compose decoded block
                    ulong decodedBlock = leftSubblock;
                    decodedBlock = (decodedBlock << 32) | rightSubblock;

                    for (int i = 0; i < 8; i++)
                    {
                        ulong a = (decodedBlock & 0xFF00000000000000) >> 56;

                        // it's a trick, the code works with non zero characters, if your text has ASCII code 0x00 it will be skipped.
                        if (a != 0)
                        {
                            decodedTextHex.Append((char)a);
                        }

                        decodedBlock = decodedBlock << 8;
                    }
                }

                return decodedTextHex.ToString();
            }

            // convert the encoded text to the set of ulong values (blocks for decoding)
            private static List<ulong> GetBlocksFromEncodedText(string text)
            {
                List<ulong> blocksListPlain = new();
                for (int i = 0; i < text.Length; i += 16)
                {
                    ulong block = Convert.ToUInt64(text.Substring(i, 16), 16);
                    blocksListPlain.Add(block);
                }

                return blocksListPlain;
            }

            // here might be any deterministic math formula
            private static uint BlockModification(uint block, uint key)
            {
                for (int i = 0; i < 32; i++)
                {
                    // 0x55555555 for the better distribution 0 an 1 in the block
                    block = ((block ^ 0x55555555) * block) % key;
                    block = block ^ key;
                }

                return block;
            }

            // There are many ways to generate a round key, any deterministic math formula does work
            private static uint GetRoundKey(uint key, int round)
            {
                // "round + 2" - to avoid a situation when pow(key,1) ^ key  = key ^ key = 0
                uint a = (uint)System.Math.Pow((double)key, round + 2);
                return a ^ key;
            }
        }
    }
}