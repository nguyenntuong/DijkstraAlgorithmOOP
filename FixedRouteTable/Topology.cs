/*
 * Author & Creator : Nguyễn Nhựt Tường
 * Date: 16-3
 * Modifer-Date: 18-3
 * */
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedRouteTable
{
    public class Topology
    {
        /// <summary>
        /// Get instance
        /// </summary>
        /// <param name="topo_size"></param>
        /// <returns></returns>
        public static Topology CreateTopology(int topo_size)
        {
            return new Topology(topo_size);
        }

        /// <summary>
        /// Chế độ tìm đường
        /// </summary>
        public enum Mode
        {
            LeastCost,
            MinimumHop
        }

        /// <summary>
        /// Chứa Cache định tuyến cho việc tái sử dụng
        /// </summary>
        private ListOfRoutePath[,] _listOfRoutePathsMetrixCache;

        public ListOfRoutePath[,] ListOfRoutePathsMetrixCache
        {
            get { return _listOfRoutePathsMetrixCache; }
            private set { _listOfRoutePathsMetrixCache = value; }
        }

        private bool _isCacheMatrixSet = false;

        public bool IsCacheMatrixSet
        {
            get { return _isCacheMatrixSet; }
            private set { _isCacheMatrixSet = value; }
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
            private set => allNode = value;
        }

        private Topology(int topo_size)
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

        /// <summary>
        /// Tìm tất cả đường đi từ node "from" tới "to" dựa trên Cost
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ListOfRoutePath RoutePathLeastCost(int from, int to)
        {
            ListOfRoutePath allPath = ListOfRoutePath.CreateRoutePathsStorage();
            AllNode[from].PathToFromTopologyLeastCost(allPathStorage: ref allPath
                , destinationNode: AllNode[to]);
            return allPath;
        }
        /// <summary>
        /// Tìm tất cả đường đi từ node "from" tới "to" dựa trên numHop
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ListOfRoutePath RoutePathMinimumHop(int from, int to)
        {
            ListOfRoutePath allPath = ListOfRoutePath.CreateRoutePathsStorage();
            AllNode[from].PathToFromTopologyMinimumHop(allPathStorage: ref allPath
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
            int[,] metrix = new int[TopologySize, TopologySize];
            metrix.Initialize();
            for (int x = 0; x < TopologySize; x++)
            {
                Dictionary<Router, Router> _routeTable = new Dictionary<Router, Router>();
                for (int y = 0; y < TopologySize; y++)
                {
                    int x_transform = x + 1;
                    int y_transform = y + 1;
                    if (x_transform == y_transform)
                    {
                        metrix[x, y] = 0;
                        continue;
                    }
                    ListOfRoutePath listPath;

                    listPath = mode == Mode.LeastCost
                        ? RoutePathLeastCost(x_transform, y_transform)
                        :
                        RoutePathMinimumHop(x_transform, y_transform);

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

        /// <summary>
        /// Tính toán bản định tuyến toàn Node
        /// sử dụng xữ lý song song
        /// </summary>
        /// <param name="mode">Chế độ tìm đường</param>
        /// <returns></returns>
        public int[,] MetrixCaculateParallel(Mode mode = Mode.LeastCost)
        {
            int[,] metrix = new int[TopologySize, TopologySize];
            metrix.Initialize();
            Parallel.For(0, TopologySize, x =>
            {
                Dictionary<Router, Router> _routeTable = new Dictionary<Router, Router>();
                ParallelLoopResult r = Parallel.For(0, TopologySize, y =>
                {
                    int x_transform = x + 1;
                    int y_transform = y + 1;
                    if (x_transform == y_transform)
                    {
                        metrix[x, y] = 0;
                        return;
                    }
                    ListOfRoutePath listPath;

                    listPath = mode == Mode.LeastCost
                        ? RoutePathLeastCost(x_transform, y_transform)
                        :
                        RoutePathMinimumHop(x_transform, y_transform);

                    if (listPath.CantRoute)
                    {
                        metrix[x, y] = -1;
                        return;
                    }
                    metrix[x, y] = mode == Mode.LeastCost ?
                        listPath.GetLeastCostPath().NextHop().HostID
                        :
                        listPath.GetMinimunHopPath().NextHop().HostID;
                    _routeTable.Add(AllNode[y_transform], AllNode[metrix[x, y]]);
                });
                AllNode[x + 1].ImportRouteTable(_routeTable);
            });
            return metrix;
        }

        public Router this[int RID]
        {
            get
            {
                if (AllNode.ContainsKey(RID))
                {
                    return AllNode[RID];
                }
                else
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
        }
    }
}
