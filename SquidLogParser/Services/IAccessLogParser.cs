namespace SquidLogParser.Services
{
    public interface IAccessLogParser
    {
        void ParseFile(string fileName);
    }
}