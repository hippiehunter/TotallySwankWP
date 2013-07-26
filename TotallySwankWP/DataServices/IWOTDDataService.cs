using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotallySwankWP.Models;

namespace TotallySwankWP.DataServices
{
  public interface IWOTDDataService
  {
    void GetEntries(Action<IEnumerable<Entry>, Exception> callback);
  }
}
