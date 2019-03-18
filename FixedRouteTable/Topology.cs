using System.Collections.Generic;

namespace FixedRouteTable
{
    public class Topology
    {
        /// <summary>
        /// Chế độ tìm đường
        /// </summary>
        public enum Mode
        {
            LeastCost,
            MinimumHop
        }

        /// <summary>
        /// Kích thước mô hình
        /// </summary>
        private int _topoSize;

        public int TopologySize
        {
            get => _topoSize;
            private set => _topoSize = value;
        }

        /// <summary>
        /// Tất cả Router trong mô hình
        /// </summary>
        private Dictionary<int, Router> allNode;

        public Dictionary<int, Router> AllNode
        {
            get => allNode;
            set => allNode = value;
        }
        public Topology(int topo_size)
        {
            TopologySize = topo_size;
            CreateAllNode(topo_size);
        }

        /// <summary>
        /// Thêm chi phí liên kết giữa các Router
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void AddRelative(int from, int to, int cost)
        {
            AllNode[from].RelativeNode(AllNode[to], cost);
        }

        private void CreateAllNode(int numNode)
        {
            AllNode = new Dictionary<int, Router>(numNode);
            for (int i = 1; i <= numNode; i++)
            {
                Router node = Router.CreateNode(i);
                AllNode.Add(i, node);
            }
        }

        public ListOfRoutePath RoutePath(int from, int to)
        {
            ListOfRoutePath allPath = ListOfRoutePath.CreateCacheListPath();
            AllNode[from].PathToFromTopology(allPath: ref allPath
                , destinationNode: AllNode[to]);
            return allPath;
        }

        /// <summary>
        /// Tính toán bản định tuyến toàn Node
        /// </summary>
        /// <param name="mode">Chế độ tìm đường</param>
        /// <returns></returns>
        public int[,] MetrixCaculate(Mode mode = Mode.LeastCost)
        {
            int[,] metrix = new int[_topoSize, _topoSize];
            metrix.Initialize();
            for (int x = 0; x < _topoSize; x++)
            {
                Dictionary<Router, Router> _routeTable = new Dictionary<Router, Router>();
                for (int y = 0; y < _topoSize; y++)
                {
                    int x_transform = x + 1;
                    int y_transform = y + 1;
                    if (x_transform == y_transform)
                    {
                        metrix[x, y] = 0;
                        continue;
                    }
                    ListOfRoutePath listPath = RoutePath(x_transform, y_transform);
                    if (listPath.CantRoute)
                    {
                        metrix[x, y] = -1;
                        continue;
                    }
                    metrix[x, y] = mode == Mode.LeastCost ?
                        listPath.GetLeastCostPath().NextHop().HostID
                        :
                        listPath.GetMinimunHopPath().NextHop().HostID;
                    _routeTable.Add(AllNode[y_transform], AllNode[metrix[x, y]]);
                }
                AllNode[x + 1].ImportRouteTable(_routeTable);
            }
            return metrix;
        }

        public Router this[int RID]
        {
            get
            {
                if (this.AllNode.ContainsKey(RID))
                {
                    return this.AllNode[RID];
                }
                else
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
        }
    }
}
