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
    Task<IEnumerable<Entry>> GetEntries();
  }
}
