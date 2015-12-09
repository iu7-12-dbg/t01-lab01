using System.Collections.Generic;

namespace Core.Utils
{
    public interface WeightedGraph<L>
    {
        int Cost(Location a, Location b);
        IEnumerable<Location> Neighbors(Location id);
    }

    public struct Location
    {
        public readonly int x, y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}