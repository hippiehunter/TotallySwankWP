using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotallySwankWP.DataServices;
using TotallySwankWP.Models;

namespace TotallySwankWP.Design
{
  public class DesignWOTDDataService : IWOTDDataService
  {
    public void GetEntries(Action<IEnumerable<Entry>, Exception> callback)
    {
      List<Entry> entries = new List<Entry>();

      entries.Add(
        new Entry ("Totally Swank", 
                   "Fail CST (STAR) Question from 2009 to 2011. How much lower can California state standards go?", 
                   "Though his tomb has not yet been excavated, it is said to be a totally swank underground palace.")
      );
      entries.Add(
        new Entry ("twerk", 
                   "to work one's body, as in dancing, especially the rear end", 
                   "She was twerking it on the dance floor.")
      );

      entries.Add(
        new Entry ("crutch", 
                   "A thick piece of paper or tagboard used in the mouth piece of a joint to support the paper and faciliate easy smoking of said joint.", 
                   "Damn nigga! You better put a crutch in that little prison J so we can smoke that bitch 'til the wheels fall off it!")
      );

      callback(entries, null);
    }
  }
}
