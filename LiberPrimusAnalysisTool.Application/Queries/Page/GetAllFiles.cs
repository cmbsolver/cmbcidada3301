using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Page;

/// <summary>
/// The container for getting the pages.
/// </summary>
public class GetAllFiles
{
    /// <summary>
    /// The command for getting the pages
    /// </summary>
    public class Command : IRequest<List<string>>
    {
        public Command(string initialDirectory)
        {
            InitialDirectory = initialDirectory;
        }

        public string InitialDirectory { get; set; }
    }

    /// <summary>
    /// The handler for the get pages command
    /// </summary>
    public class Handler : IRequestHandler<Command, List<string>>
    {
        /// <summary>
        /// The handler for the get pages command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<string> retval = new List<string>();
            await GetPages(request.InitialDirectory, retval);
            return retval.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pages"></param>
        private async Task GetPages(string path, List<string> pages)
        {
            if (Directory.GetDirectories(path).Length > 0)
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    await GetPages(dir, pages);
                }
            }

            foreach (var file in Directory.GetFiles(path))
            {
                var fileInfo = new FileInfo(file);
                pages.Add(fileInfo.FullName);
            }
        }
    }
}