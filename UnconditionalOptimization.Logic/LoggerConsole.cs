using System.Collections.Concurrent;
using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class LoggerConsole : ILogger, IDisposable
{
    private ConcurrentQueue<string[]> _rows;
    private bool _isOn;
    private Thread _listeningThread;

    public LoggerConsole()
    {
        _rows = new ConcurrentQueue<string[]>();
        _isOn = true;
        _listeningThread = new Thread(new ThreadStart(Listener));
        _listeningThread.Start();
    }

    public void AddARowToTheTable(string[] row)
    {
        _rows.Enqueue(row);
    }

    private void Listener()
    {
        while (_isOn || _rows.Count > 0)
        {
            string[] row;
            if (_rows.TryDequeue(out row))
            {
                string res = "";
                for (int i = 0; i < row.Length; i++)
                    res += String.Format("{0,-20}", row[i]);
                Console.WriteLine(res);
            }
            else
                Thread.Sleep(0);
        }
    }

    public void Dispose()
    {
        _isOn = false;
        _listeningThread.Join();
        GC.SuppressFinalize(this);
    }

    ~LoggerConsole()
    {
        _isOn = false;
        _listeningThread.Join();
    }
}