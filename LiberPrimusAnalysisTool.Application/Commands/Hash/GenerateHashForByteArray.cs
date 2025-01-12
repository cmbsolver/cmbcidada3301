using System.Security.Cryptography;
using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Hash;

public class GenerateHashForByteArray
{
    public class Command : IRequest<string>
    {
        public byte[] ByteArray { get; set; }
        public string HashType { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Hash(request.ByteArray, request.HashType));
        }
        
        private string Hash(byte[] array, string hashType)
        {
            return hashType switch
            {
                "SHA-512" => HashSha512(array),
                "SHA3-512" => HashSha3_512(array),
                "Blake2s" => HashBlake2s(array),
                "Blake2b" => HashBlake2b(array),
                _ => throw new ArgumentOutOfRangeException(nameof(hashType), hashType, null)
            };
        }
        
        private string HashSha512(byte[] array)
        {
            using var sha512 = SHA512.Create();
            var hash = sha512.ComputeHash(array);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        
        private string HashSha3_512(byte[] array)
        {
            using var sha3 = SHA3_512.Create();
            var hash = sha3.ComputeHash(array);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        
        private string HashBlake2s(byte[] array)
        {
            var hash = Blake2Fast.Blake2s.ComputeHash(array);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        
        private string HashBlake2b(byte[] array)
        {
            var hash = Blake2Fast.Blake2b.ComputeHash(array);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}