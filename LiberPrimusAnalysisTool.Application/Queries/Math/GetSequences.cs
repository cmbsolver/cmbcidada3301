using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == "LiberPrimusAnalysisTool.Application.Queries.Math" && type.GetInterfaces().Any(x => x.Name.Contains("ISequence")))
                {
                    var name = type.GetProperties().Where(p => p.Name == "Name").FirstOrDefault().GetValue(null);
                    mathTypes.Add($"{name}");
                }
            }

            return Task.FromResult(mathTypes.OrderBy(x => x).ToList());
        }
    }
}