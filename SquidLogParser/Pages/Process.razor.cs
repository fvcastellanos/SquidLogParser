using Microsoft.AspNetCore.Components;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class ProcessBase: ComponentBase
    {
        [Inject]
        protected LogService LogService { get; set; }

        protected bool DisplaySuccessMessage;
        protected bool DisplayErrorMessage;
        protected bool DisplayProcessMessage;
        protected long ProcessedLogs;
        protected string ErrorMessage;

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
            }, left => {
                DisplayErrorMessage = true;
                ErrorMessage = left;
            });
        }

        // --------------------------------------------------------------------------------------------------

        private void InitializeVariables()
        {
            DisplaySuccessMessage = false;
            DisplayErrorMessage = false;
            DisplayProcessMessage = false;
            ErrorMessage = "";
        }
    }
}