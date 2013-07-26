using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TotallySwankWP.Models
{
  public class Entry
  {
    public Entry(string name, string definition, string example)
    {
      Name = name;
      Definition = definition;
      Example = example;
    }

    public string Name { get; private set; }
    public string Definition { get; private set; }
    public string Example { get; private set; }
  }
}
