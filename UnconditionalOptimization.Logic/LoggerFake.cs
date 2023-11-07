using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class LoggerFake : ILogger
{
    public void AddARowToTheTable(string[] row) { }
}