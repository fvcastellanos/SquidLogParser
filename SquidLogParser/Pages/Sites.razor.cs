using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using SquidLogParser.Domain;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class SitesBase: ComponentBase
    {
        private const int DefaultTopRows = 10;

        [Inject]
        protected QueryLogService QueryLogService { get; set; }

        protected IEnumerable<AccessLogView> VisitedSites;

        protected string ErrorMessage;

        protected int TopRows;

        protected override void OnInitialized()
        {
            TopRows = DefaultTopRows;
            GetTopVisitedSites();
        }

        public void TopRowChange(ChangeEventArgs e)
        {
            int rows = int.Parse(e.Value.ToString());
            GetTopVisitedSites(rows);
        }

        // ------------------------------------------------------------------------------------

        private void GetTopVisitedSites(int top = DefaultTopRows)
        {
            var result = QueryLogService.GetTopVisitedSitesLastNDays(top);

            result.Match(right => {
                VisitedSites = right;
            }, left => {
                ErrorMessage = left;
            });
        }
    }
}