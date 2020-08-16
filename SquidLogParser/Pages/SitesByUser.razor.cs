using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using SquidLogParser.Domain;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class SitesByUserBase: ComponentBase
    {
        [Inject]
        protected QueryLogService QueryLogService { get; set; }

        protected IEnumerable<string> UserList;

        protected IEnumerable<AccessLogView> VisitedSites;

        protected string ErrorMessage;

        protected string SelectedUser;

        protected int TopRows;

        protected override void OnInitialized()
        {
            VisitedSites = new List<AccessLogView>();
            TopRows = 10;
            GetUsers();
        }

        protected void PerformQuery()
        {
            GetVisitedSites();
        }

        // ---------------------------------------------------------------------------------------------

        private void GetUsers()
        {
            var result = QueryLogService.GetUsers();

            result.Match(right => {
                UserList = right;
            }, left => {
                ErrorMessage = left;
            });
        }

        private void GetVisitedSites()
        {
            var result = QueryLogService.GetTopSitesByUser(SelectedUser, TopRows);
            result.Match(right => {
                VisitedSites = right;
            }, left => {
                ErrorMessage = left;
            });            
        }
    }
}