using System.Globalization;
using System.Net;

namespace NetworkChecker;

internal static class Program
{
    private static bool _previousConnectionState;
    
    [Obsolete("Obsolete")]
    public static async Task Main()
    {
        while (true)
        {
            bool currentConnectionState = IsConnectionAvailable();
            DumpConnectionInfo(currentConnectionState);
            _previousConnectionState = currentConnectionState;
            await Task.Delay(3000);
        }
    }

    [Obsolete("Obsolete")]
    private static bool IsConnectionAvailable()
    {
        try
        {
            var url = CultureInfo.InstalledUICulture switch
            {
                _ => "https://www.gstatic.com/generate_204"
            };

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.KeepAlive = false;
            request.Timeout = 10000;

            using var response = (HttpWebResponse)request.GetResponse();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static async void DumpConnectionInfo(bool isConnectionAvailable)
    {
        if (isConnectionAvailable == _previousConnectionState) return;

        string message = $"[{DateTime.Now}] З'єднання {
            (isConnectionAvailable ? "є" : "немає")
        }!";
        
        await using StreamWriter stream = new("connection_status.txt", true);
        await stream.WriteLineAsync(message);
        
        if (!isConnectionAvailable)
            Console.WriteLine(message);
    }
}
