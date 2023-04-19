using LibVLCSharp.Shared;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: M3U8Scraper.exe url filename");
            return;
        }

        string url = args[0];
        string filename = args[1];

        Core.Initialize();

        using var libVlc = new LibVLC();

        const string prefix = "";
        Console.WriteLine($"Scrape {prefix}{filename}");

        using var player = new MediaPlayer(libVlc);

        var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var destination = Path.Combine(currentDirectory, $"{prefix}{filename}.ts");
        //libVlc.Log += (sender, e) => Console.WriteLine($"[{e.Level}] {e.Module}:{e.Message}");
        player.PositionChanged += (sender, e) =>
        {
            Console.WriteLine($"Position: {e.Position}.");
        };

        player.EndReached += (sender, e) =>
        {
            Console.WriteLine("Recorded file is located at " + destination);
            Environment.Exit(0);
        };

        using var media = new Media(
            libVlc,
            new Uri(url),
            // Define stream output options.
            // In this case stream to a file with the given path and play locally the stream while streaming it.
            ":sout=#file{dst=" + destination + "}",
            ":sout-keep");
        player.Play(media);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}