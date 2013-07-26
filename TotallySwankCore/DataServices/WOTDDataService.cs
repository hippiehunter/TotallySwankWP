using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TotallySwankWP.External;
using TotallySwankWP.Models;

namespace TotallySwankWP.DataServices
{
  public class WOTDDataService : IWOTDDataService
  {
    private const string HOMEPAGE_URL = "http://www.urbandictionary.com/";

    public async Task<IEnumerable<Entry>> GetEntries()
    {
      List<Entry> entries = new List<Entry>();

      HtmlDocument doc = new HtmlDocument();
      WebClient wc = new WebClient();
      string s = null;

      try {
        s = await wc.DownloadStringTaskAsync(HOMEPAGE_URL);
      }
      catch (WebException e) {
          //because async debugging on windows phone sucks, having this here at least gives us a hint at what happened
          throw;
      }

      doc.LoadHtml(s);

      foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='daily']")) {
        string name = HttpUtility.HtmlDecode(node.SelectSingleNode(".//a[@href]").InnerHtml).ToLowerInvariant();
        string definition = HtmlRemoval.StripTags(HttpUtility.HtmlDecode(node.SelectSingleNode("div[@class='definition']").InnerHtml.Trim().Replace("<br>", " ")));
        string example = HtmlRemoval.StripTags(HttpUtility.HtmlDecode(node.SelectSingleNode("div[@class='example']").InnerHtml.Trim().Replace("<br><br>", "\n").Replace("<br>","")));
        entries.Add(new Entry(name, definition, example));
      }

      return entries;
    }

  }
}
