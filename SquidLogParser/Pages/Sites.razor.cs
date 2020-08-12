using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using SquidLogParser.Domain;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class SitesBase: ComponentBase
    {
        [Inject]
        protected QueryLogService QueryLogService { get; set; }

        protected IEnumerable<AccessLogView> VisitedSites;

        protected string ErrorMessage;

        protected override void OnInitialized()
        {
            GetTopVisitedSites();
        }

        // ------------------------------------------------------------------------------------

        private void GetTopVisitedSites()
        {
            var result = QueryLogService.GetTopVisitedSites();

            result.Match(right => {
                VisitedSites = right;
            }, left => {
                ErrorMessage = left;
            });
        }
    }
}