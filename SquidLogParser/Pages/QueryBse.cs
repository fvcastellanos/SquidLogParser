using Microsoft.AspNetCore.Components;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public abstract class QueryBase: PageBase
    {
        [Inject]
        protected FilterUrlService FilterUrlService { get; set; }

        protected abstract void RefreshResults();

        protected void AddFilter(string url)
        {
            var result = FilterUrlService.AddUrl(url);

            result.Match(Right => {

                RefreshResults();

            }, ShowErrorMessage);
        }
    }
}