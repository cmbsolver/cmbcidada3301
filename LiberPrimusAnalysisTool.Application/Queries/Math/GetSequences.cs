using System.Reflection;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Math;

public class GetSequences
{
    public class Query : IRequest<List<string>>
    {
    }

    public class Handler : IRequestHandler<Query, List<string>>
    {
        public Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            Assembly asm = Assembly.GetAssembly(typeof(CalculateSequence));
            List<string> mathTypes = new List<string>();
            var counter = 1;

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == "LiberPrimusAnalysisTool.Application.Queries.Math" && type.GetInterfaces().Any(x => x.Name.Contains("ISequence")))
                {
                    var name = type.GetProperties().Where(p => p.Name == "Name").FirstOrDefault().GetValue(null);
                    mathTypes.Add($"{name}");
                    counter++;
                }
            }

            return Task.FromResult(mathTypes);
        }
    }
}