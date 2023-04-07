using LibVLCSharp.Shared;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: MyApp arg1 arg2");
            return;
        }

        string url = args[0];
        string filename = args[1];

        Core.Initialize();

        using var libVlc = new LibVLC();

        const string prefix = "Daily.Wire.Exodus.with.Jordan.Peterson.";
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





//var libVLC = new LibVLC();

////var libVLC = new LibVLC("--no-xlib");
//var mediaPlayer = new MediaPlayer(libVLC);

//const string m3u8 = "https://manifest-gcp-us-east4-vop1.cfcdn.media.dailywire.com/00e0284DgpUVUxeeKKOyEECy8M5VXCc1TKTg58rcLLD01ffYy1eDvv6GThGJrORG00GPBZXLa8BkY4ZA3022tDe6C2g/rendition.m3u8?cdn=cloudflare&expires=1680978240&skid=default&signature=NjQzMWIxNDBfZmRlZTNkM2M1YTI3NWE0NGExODc5YzYzNzBlYjQwNGIzZTdlYWJlYTE1NTc1YTk0OTY1NTg1MDRmOTA2YWZiYQ==&vsid=F8mYo00I39PdO9vh6z24qxmwh7KXNWWgmqzp32KUSxSmti016UnDQYnuzoSWtRV611ALlZ9sKb501M";

//var media = new Media(libVLC, new Uri(m3u8));
//mediaPlayer.Play(media);

//// Subscribe to the TimeChanged event to show download progress
//media.DurationChanged += (sender, e) =>
//{
//    Console.WriteLine($"Downloaded {e.Duration} seconds of the video");
//};

//await Task.Delay(5000); // wait for 5 seconds
//mediaPlayer.Stop();
//var outputMedia = new Media(libVLC, new Uri(m3u8));
//outputMedia.AddOption(
//    ":sout=#transcode{vcodec=h264,vb=800,acodec=mp3,ab=128}" +
//    ":standard{access=file,mux=mp4,dst=" + "output_dw.mp4" + "}");

//mediaPlayer.Play(outputMedia);

//await Task.Delay(5000); // wait for 5 seconds
//mediaPlayer.Stop();

//Console.WriteLine("done!");
//Console.ReadLine();