using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using SquidLogParser.Domain;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class SitesByUserBase: PageBase
    {
        [Inject]
        protected QueryLogService QueryLogService { get; set; }

        [Parameter]
        public string ParameterUser { get; set; }

        protected IEnumerable<string> UserList;

        protected IEnumerable<AccessLogView> VisitedSites;

        protected string SelectedUser;

        protected int TopRows;

        protected int LastDays;

        protected override void OnInitialized()
        {
            VisitedSites = new List<AccessLogView>();
            TopRows = 10;
            LastDays = 30;
            VerifyParameter();
            GetUsers();
        }

        protected void PerformQuery()
        {
            GetVisitedSites();
        }

        // ---------------------------------------------------------------------------------------------

        private void GetUsers()
        {
            HideErrorMessage();
            var result = QueryLogService.GetUsers();

            result.Match(right => {
                UserList = right;
            }, ShowErrorMessage);
        }

        private void GetVisitedSites()
        {
            HideErrorMessage();
            var result = QueryLogService.GetTopSitesByUser(SelectedUser, TopRows, LastDays);
            result.Match(right => {
                VisitedSites = right;
            }, ShowErrorMessage);            
        }

        private void VerifyParameter()
        {
            if (!string.IsNullOrEmpty(ParameterUser))
            {
                SelectedUser = ParameterUser.Replace("-", ".");
                GetVisitedSites();
            }
        }
    }
}