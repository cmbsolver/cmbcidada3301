using System;
using System.Collections.Generic;
using System.Linq;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LiberPrimusAnalysisTool.Application.Commands.Math
{
    /// <summary>
    /// Calculate Totient
    /// </summary>
    public class CalculateSequence
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<NumericSequence>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query" /> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="name">The name.</param>
            public Query(ulong value, string name)
            {
                Value = value;
                NameToRun = name;
            }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string NameToRun { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public ulong Value { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, NumericSequence>
        {
            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task<NumericSequence> Handle(Query request, CancellationToken cancellationToken)
            {
                NumericSequence numericSequence = null;

                object query = null;

                Assembly asm = Assembly.GetAssembly(typeof(CalculateSequence));
                List<Tuple<Type, string>> mathTypes = new List<Tuple<Type, string>>();

                foreach (Type type in asm.GetTypes())
                {
                    if (type.Namespace == "LiberPrimusAnalysisTool.Application.Queries.Math" &&
                        type.GetInterfaces().Any(x => x.Name.Contains("ISequence")))
                    {
                        var name = type.GetProperties().Where(p => p.Name == "Name").FirstOrDefault().GetValue(null);
                        mathTypes.Add(new Tuple<Type, string>(type, name.ToString()));
                    }
                }

                var stype = mathTypes.FirstOrDefault(x => x.Item2.ToUpper() == request.NameToRun.ToUpper()).Item1;
                query = stype.GetMethod("BuildCommand").Invoke(null, new object[1] { request.Value });

                numericSequence = (NumericSequence)await _mediator.Send(query);

                return numericSequence;
            }
        }
    }
}