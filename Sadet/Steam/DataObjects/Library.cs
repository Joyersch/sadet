using System.Linq;

namespace Sadet.Steam.DataObjects;

public class Library
{
    public Library()
    {
        Games = new List<Game>();
    }

    public List<Game> Games { get; set; }
}