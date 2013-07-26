using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TotallySwankWP.External;
using TotallySwankWP.Models;

namespace TotallySwankWP.DataServices
{
  public class SearchDataService : ISearchDataService
  {
    private const string SEARCH_URL = "http://www.urbandictionary.com/define.php?term=";

    public async void GetEntries(string query, Action<IEnumerable<Entry>, Exception> callback)
    {
      List<Entry> entries = new List<Entry>();

      HtmlDocument doc = new HtmlDocument();
      WebClient wc = new WebClient();
      string s = null;

      try {
        s = await wc.DownloadStringTaskAsync(SEARCH_URL + Uri.EscapeDataString(query));
      }
      catch (WebException e) {
        callback(null, e);
        return;
      }

      doc.LoadHtml(s);

      HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//td[@class='text']");

      if (nodes == null || nodes.Count <= 0) {
        callback(null, new ArgumentException());
        return;
      }
      
      foreach (HtmlNode node in nodes) {
        if (node == null) continue;

        string name = query.ToLowerInvariant();

        string definition, example;

        try {
          var defNode = node.SelectSingleNode("div[@class='definition']");
          var exNode = node.SelectSingleNode("div[@class='example']");

          if (defNode == null || exNode == null)
            continue;

          definition = defNode.InnerHtml;
          definition = definition.Trim().Replace("<br><br>", "\n");
          definition = HttpUtility.HtmlDecode(definition);
          definition = HtmlRemoval.StripTags(definition);

          example = exNode.InnerHtml;
          example = example.Trim().Replace("<br><br>", "\n");
          example = HttpUtility.HtmlDecode(example);
          example = HtmlRemoval.StripTags(example);
        }
        catch (Exception ) {
          continue;
        }

        entries.Add(new Entry(name, definition, example));
      }
      
      callback(entries, null);
    }
  }
}
