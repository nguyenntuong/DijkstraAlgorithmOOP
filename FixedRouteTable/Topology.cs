/*
 * Author & Creator : Nguyễn Nhựt Tường
 * Date: 16-3
 * Modifer-Date: 18-3
 * */
using System.Collections.Generic;
using System.Diagnostics;
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
        private Dictionary<int, Router> _router;

        public Dictionary<int, Router> Routers
        {
            get => _router;
            private set => _router = value;
        }

        private Topology(int topo_size)
        {
            TopologySize = topo_size;
            InitAllRouters(topo_size);
        }

        /// <summary>
        /// Thêm chi phí liên kết giữa các Router
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void AddRelative(int from, int to, int cost)
        {
            Routers[from].AddDirectedRouterWithCost(Routers[to], cost);
        }

        /// <summary>
        /// Gở bỏ kết nối giữa 2 NODE
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void RemoveRelative(int from, int to)
        {
            Routers[from].RemoveDirectedRouter(Routers[to]);
        }

        /// <summary>
        /// Cập nhật Cost outgoing
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void UpdateRelative(int from, int to, int cost)
        {
            Routers[from].UpdateCost(Routers[to], cost);
        }

        /// <summary>
        /// Check 2 Node có kết nối với nhau không
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public bool HasLinkConnect(int node1, int node2)
        {
            return Routers[node1].HasLinkConnect(Routers[node2])
                    &&
                    Routers[node2].HasLinkConnect(Routers[node1]);
        }

        /// <summary>
        /// Khởi tạo tất cả các Node trong mô hình với thông số mặc định
        /// </summary>
        /// <param name="topoSize"></param>
        private void InitAllRouters(int topoSize)
        {
            Routers = new Dictionary<int, Router>(topoSize);
            for (int i = 1; i <= topoSize; i++)
            {
                Router node = Router.CreateRouter(i);
                Routers.Add(i, node);
            }
        }

        /// <summary>
        /// Tìm tất cả đường đi từ node "from" tới "to" dựa trên Cost
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private BestRoutePathBase RoutePathLeastCost(int from, int to)
        {
            BestRoutePathBase bestPath = LeastCostRoutePath.CreateRoutePathStorage();
            Routers[from].PathToFromTopology(pathStorage: ref bestPath
                , destinationNode: Routers[to]);
            return bestPath;
        }

        /// <summary>
        /// Tìm tất cả đường đi từ node "from" tới "to" dựa trên numHop
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private BestRoutePathBase RoutePathMinimumHop(int from, int to)
        {
            BestRoutePathBase bestPath = MinimumHopRoutePath.CreateRoutePathStorage();
            Routers[from].PathToFromTopology(pathStorage: ref bestPath
                , destinationNode: Routers[to]);
            return bestPath;
        }

        /// <summary>
        /// Tính toán bản định tuyến toàn Node
        /// </summary>
        /// <param name="mode">Chế độ tìm đường</param>
        /// <returns></returns>
        public int[,] MetrixCaculate(Mode mode = Mode.LeastCost)
        {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
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
                    BestRoutePathBase bestPath;

                    bestPath = mode == Mode.LeastCost
                        ?
                        RoutePathLeastCost(x_transform, y_transform)
                        :
                        RoutePathMinimumHop(x_transform, y_transform);

                    if (bestPath.CantRoute)
                    {
                        metrix[x, y] = -1;
                        continue;
                    }
                    metrix[x, y] = bestPath.GetBestPath().NextHop().HostID;
                    _routeTable.Add(Routers[y_transform], Routers[metrix[x, y]]);
                }
                Routers[x + 1].ImportRouteTable(_routeTable);
            }
#if DEBUG
            stopwatch.Stop();
            System.Console.WriteLine($"Sync {stopwatch.ElapsedMilliseconds} miliseconds");
#endif
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
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
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
                    BestRoutePathBase bestPath;

                    bestPath = mode == Mode.LeastCost
                        ?
                        RoutePathLeastCost(x_transform, y_transform)
                        :
                        RoutePathMinimumHop(x_transform, y_transform);

                    if (bestPath.CantRoute)
                    {
                        metrix[x, y] = -1;
                        return;
                    }
                    metrix[x, y] = bestPath.GetBestPath().NextHop().HostID;
                    _routeTable.Add(Routers[y_transform], Routers[metrix[x, y]]);
                });
                Routers[x + 1].ImportRouteTable(_routeTable);
            });
#if DEBUG
            stopwatch.Stop();
            System.Console.WriteLine($"Parallel {stopwatch.ElapsedMilliseconds} miliseconds");
#endif
            return metrix;
        }

        public Router this[int RID]
        {
            get
            {
                if (Routers.ContainsKey(RID))
                {
                    return Routers[RID];
                }
                else
                {
                    throw new System.Exception($"Không tồn tại Router {RID} trong Topology");
                }
            }
        }
    }
}
