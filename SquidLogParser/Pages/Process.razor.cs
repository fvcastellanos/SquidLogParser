using Microsoft.AspNetCore.Components;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class ProcessBase: PageBase
    {
        [Inject]
        protected LogService LogService { get; set; }

        protected bool DisplaySuccessMessage;
        protected bool DisplayProcessMessage;
        protected long ProcessedLogs;

        protected override void OnInitialized()
        {
            InitializeVariables();
        }

        protected void ProcessLogs()
        {
            InitializeVariables();
            DisplayProcessMessage = true;

            var result = LogService.IngestLogs();

            result.Match(right => 
            {
                 DisplaySuccessMessage = true;
                 ProcessedLogs = right;
            }, ShowErrorMessage);
        }

        // --------------------------------------------------------------------------------------------------

        private void InitializeVariables()
        {
            DisplaySuccessMessage = false;
            DisplayProcessMessage = false;
            HideErrorMessage();
        }
    }
}