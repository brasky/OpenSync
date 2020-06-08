using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenSync.Server.Controllers
{
    [ApiController]
    public class YoutubeController : ControllerBase
    {

        private static readonly HttpClient client = new HttpClient();
        private string apikey = Startup.googleapikey;
        private const string YT_API_BASE = "https://www.googleapis.com/youtube/v3/videos?id=";
        private const string YT_API_TITLE_FIELD = "&fields=items(snippet(title))&part=snippet";
        private const string YT_API_THUMBNAILS_FIELD = "&fields=items(snippet(thumbnails))&part=snippet";

        [HttpGet("api/youtube/gettitle/{videoId}")]
        public async Task<string> GetTitleAsync(string videoId)
        {
            HttpResponseMessage response = await client.GetAsync(YT_API_BASE + videoId + "&key=" + apikey + YT_API_TITLE_FIELD);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<YouTubeJson>(responseBody);
            return result.items[0].snippet.title;
        }

        [HttpGet("api/youtube/getthumbnails/{videoId}")]
        public async Task<string> GetThumbnailAsync(string videoId)
        {
            HttpResponseMessage response = await client.GetAsync(YT_API_BASE + videoId + "&key=" + apikey + YT_API_THUMBNAILS_FIELD);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<YouTubeJson>(responseBody);
            return result.items[0].snippet.thumbnails.@default.url;
        }
    }
    public class Snippet
    {
        public string title { get; set; }
        public Thumbnails thumbnails { get; set; }
    }

    public class Item
    {
        public Snippet snippet { get; set; }
    }

    public class YouTubeJson
    {
        public IList<Item> items { get; set; }
    }
    public class Default
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Medium
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class High
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Standard
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Maxres
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Thumbnails
    {
        public Default @default { get; set; }
        public Medium medium { get; set; }
        public High high { get; set; }
        public Standard standard { get; set; }
        public Maxres maxres { get; set; }
    }

}
