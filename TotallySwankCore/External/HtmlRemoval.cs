using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TotallySwankWP.External
{
  // stolen from http://www.dotnetperls.com/remove-html-tags
  public static class HtmlRemoval
  {
    public static string StripTags(string source)
    {
      char[] array = new char[source.Length];
      int arrayIndex = 0;
      bool inside = false;

      for (int i = 0; i < source.Length; i++) {
        char let = source[i];
        if (let == '<') {
          inside = true;
          continue;
        }
        if (let == '>') {
          inside = false;
          continue;
        }
        if (!inside) {
          array[arrayIndex] = let;
          arrayIndex++;
        }
      }
      return new string(array, 0, arrayIndex);
    }
  }
}
