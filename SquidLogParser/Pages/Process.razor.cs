using Microsoft.AspNetCore.Components;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class ProcessBase: ComponentBase
    {
        [Inject]
        protected LogService LogService { get; set; }

        protected bool HasProcessedLogs;
        protected long ProcessedLogs;

        protected override void OnInitialized()
        {
            HasProcessedLogs = false;
        }

        protected void ProcessLogs()
        {
            ProcessedLogs = LogService.IngestLogs();
            HasProcessedLogs = true;
        }
    }
}