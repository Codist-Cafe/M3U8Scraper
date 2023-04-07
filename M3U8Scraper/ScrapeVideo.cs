internal class ScrapeVideo
{
    public string OutputFilename { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public ScrapeVideo()
    {

    }
    public ScrapeVideo(string outputFilename, string url)
    {
        OutputFilename = outputFilename;
        Url = url;
    }
}