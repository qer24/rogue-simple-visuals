using System.Linq;

namespace RogueProject.Utils
{
    /// <summary>
    /// Disjoint set data structure
    /// </summary>
    public class DisjointSet
    {
        private readonly int[] _parent;
        private readonly int[] _rank;

        /// <summary>
        /// Disjoint set data structure
        /// </summary>
        public DisjointSet(int size)
        {
            _parent = Enumerable.Range(0, size).ToArray();
            _rank = new int[size];
        }

        public int Find(int x)
        {
            if (_parent[x] != x)
                _parent[x] = Find(_parent[x]);
            return _parent[x];
        }

        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if (rootX == rootY) return;

            if (_rank[rootX] < _rank[rootY])
            {
                _parent[rootX] = rootY;
            }
            else if (_rank[rootX] > _rank[rootY])
            {
                _parent[rootY] = rootX;
            }
            else
            {
                _parent[rootY] = rootX;
                _rank[rootX]++;
            }
        }
    }
}
