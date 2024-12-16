using LiberPrimusAnalysisTool.Entity.Image;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Page;

/// <summary>
/// The container for getting the pages.
/// </summary>
public class GetPages
{
    /// <summary>
    /// The command for getting the pages
    /// </summary>
    public class Command : IRequest<List<LiberPage>>
    {

    }

    /// <summary>
    /// The handler for the get pages command
    /// </summary>
    public class Handler : IRequestHandler<Command, List<LiberPage>>
    {
        /// <summary>
        /// The handler for the get pages command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<LiberPage>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<LiberPage> retval = new List<LiberPage>();
            var processFileInfo = new FileInfo(Environment.ProcessPath);
            await GetPages($"{processFileInfo.Directory}/input", retval);
            return retval.OrderBy(x => x.FileName).ToList();
        }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pages"></param>
        private async Task GetPages(string path, List<LiberPage> pages)
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
                if (file.EndsWith(".png") ||
                    file.EndsWith(".jpg") ||
                    file.EndsWith(".jpeg") ||
                    file.EndsWith(".bmp") ||
                    file.EndsWith(".gif") ||
                    file.EndsWith(".tiff") ||
                    file.EndsWith(".webp") ||
                    file.EndsWith(".svg"))
                {
                    var page = new LiberPage();
                    page.PageName = Path.GetFileName(file);
                    page.FileName = Path.GetFullPath(file);
                    pages.Add(page);
                }
            }
        }
    }
}