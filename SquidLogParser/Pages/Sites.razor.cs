using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using SquidLogParser.Domain;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class SitesBase: QueryBase
    {
        [Inject]
        protected QueryLogService QueryLogService { get; set; }

        protected IEnumerable<AccessLogView> VisitedSites;

        protected int TopRows;

        protected int LastDays;

        protected override void OnInitialized()
        {
            TopRows = 10;
            LastDays = 30;
            HideErrorMessage();
            GetTopVisitedSites();
        }

        public void TopRowChange(ChangeEventArgs eventArgs)
        {
            TopRows = int.Parse(eventArgs.Value.ToString());
            GetTopVisitedSites();
        }

        public void LastDaysChange(ChangeEventArgs eventArgs)
        {
            LastDays = int.Parse(eventArgs.Value.ToString());
            GetTopVisitedSites();
        }

        protected override void RefreshResults()
        {
            HideErrorMessage();
            GetTopVisitedSites();
        }

        // ------------------------------------------------------------------------------------

        private void GetTopVisitedSites()
        {
            HideErrorMessage();
            var result = QueryLogService.GetTopVisitedSitesLastNDays(TopRows, LastDays);

            result.Match(right => {
                VisitedSites = right;
            }, ShowErrorMessage);
        }
    }
}