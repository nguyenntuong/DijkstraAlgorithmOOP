/*
 * Author & Creator : Nguyễn Nhựt Tường
 * Date: 16-3
 * Modifer-Date: 18-3
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixedRouteTable
{
    public class Router
    {

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        public static Router CreateNode(int iD) => new Router(iD);

        /// <summary>
        /// Danh sách các Router kết nối trực tiếp kết hợp với phí kết nối (Cost)
        /// Chỉ sử dụng phí cổng ra
        /// </summary>
        private readonly Dictionary<Router, int> _directedRoutersWithCost = new Dictionary<Router, int>();

        public Dictionary<Router, int> DirectedRoutersWithCost => _directedRoutersWithCost;

        /// <summary>
        /// ID of Router
        /// Định danh Router
        /// </summary>
        private int _routerID;

        public int HostID
        {
            get => _routerID;
            private set => _routerID = value;
        }

        /// <summary>
        /// Bảng định tuyến
        /// </summary>
        private Dictionary<Router, Router> _routeTable;

        public Dictionary<Router, Router> RouteTable
        {
            get => _routeTable;
            private set => _routeTable = value;
        }


        private Router(int id) => HostID = id;

        public void ImportRouteTable(Dictionary<Router, Router> routeTable) => RouteTable = routeTable;

        /// <summary>
        /// Thêm phí liên kết giữa Router và các Router kề
        /// </summary>
        /// <param name="directedNode">Router kề</param>
        /// <param name="cost">Phí</param>
        /// <returns></returns>
        public bool RelativeNode(Router directedNode, int cost)
        {
            if (DirectedRoutersWithCost.ContainsKey(directedNode))
                return false;
            DirectedRoutersWithCost.Add(directedNode, cost);
            return true;
        }

        /// <summary>
        /// Tìm tất tuyến đường từ chính Router này đến một Router khác trong 
        /// Topology chứa nó dựa vào chi phí đường đi
        /// </summary>
        /// <param name="pathStorage">Chứa tất cả các tuyến đường có thể đi</param>
        /// <param name="destinationNode">Đích đến</param>
        /// <param name="sourceNode">Nguồn</param>
        /// <param name="pathTracking">Đánh dấu đường đi</param>
        public void PathToFromTopologyLeastCost(ref ListOfRoutePath pathStorage
            , Router destinationNode
            , List<Router> pathTracking = null)
        {
            pathTracking = (pathTracking ?? new List<Router>()).ToList();
            if (pathTracking.Exists(o => o.Equals(this)))
            {
                return;
            }
            pathTracking.Add(this);
            if (pathStorage.GetLeastCostPath() != null 
                &&RoutePath.CaculateCost(pathTracking)>pathStorage.GetLeastCostPath().Cost)
            {
                return;
            }
            if (DirectedRoutersWithCost.ContainsKey(destinationNode))
            {
                pathTracking.Add(destinationNode);
                pathStorage.Add(RoutePath.CreatePath(pathTracking));
                return;
            }
            else
            {
                var directedRouter = DirectedRoutersWithCost
                    .Where(co=>!pathTracking.Exists(po => po.Equals(co.Key))
                    );
                foreach (KeyValuePair<Router, int> item in directedRouter)
                {
                    item.Key.PathToFromTopologyLeastCost(pathStorage: ref pathStorage
                        , destinationNode: destinationNode
                        , pathTracking: pathTracking);
                }
            }
        }

        /// <summary>
        /// Tìm tất tuyến đường từ chính Router này đến một Router khác trong 
        /// Topology chứa nó dựa vào số Hop ít nhất
        /// </summary>
        /// <param name="pathStorage">Chứa tất cả các tuyến đường có thể đi</param>
        /// <param name="destinationNode">Đích đến</param>
        /// <param name="sourceNode">Nguồn</param>
        /// <param name="pathTracking">Đánh dấu đường đi</param>
        public void PathToFromTopologyMinimumHop(ref ListOfRoutePath pathStorage
            , Router destinationNode
            , List<Router> pathTracking = null)
        {
            pathTracking = (pathTracking ?? new List<Router>()).ToList();
            if (pathTracking.Exists(o => o.Equals(this)))
            {
                return;
            }
            pathTracking.Add(this);
            if (pathStorage.GetMinimunHopPath() != null
                &&                
                pathTracking.Count > pathStorage.GetMinimunHopPath().NumHop)
            {
                return;
            }
            if (DirectedRoutersWithCost.ContainsKey(destinationNode))
            {
                pathTracking.Add(destinationNode);
                pathStorage.Add(RoutePath.CreatePath(pathTracking));
                return;
            }
            else
            {
                var directedRouter = DirectedRoutersWithCost
                    .Where(co => !pathTracking.Exists(po => po.Equals(co.Key))
                    );
                foreach (KeyValuePair<Router, int> item in directedRouter)
                {
                    item.Key.PathToFromTopologyMinimumHop(pathStorage: ref pathStorage
                        , destinationNode: destinationNode
                        , pathTracking: pathTracking);
                }
            }
        }
        /// <summary>
        /// Tìm đường đi dựa vào database
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public RoutePath GetRoutePathFromRoutingTable(Router destination)
        {
            List<Router> path = new List<Router>();
            Router current = this;
            while (current.RouteTable.ContainsKey(destination))
            {
                path.Add(current);
                current = current.RouteTable[destination];
            }
            path.Add(destination);
            return RoutePath.CreatePath(path);
        }

        public override string ToString()
        {
            string present = $"Router: {HostID} - DirectedNode: {DirectedRoutersWithCost.Count} => {{";

            foreach (KeyValuePair<Router, int> item in DirectedRoutersWithCost)
            {
                present += " " + item.Key.HostID.ToString();
            }
            present += " }";
            return present;
        }

        public override bool Equals(object obj)
        {
            Router router = obj as Router;
            return router != null &&
                   HostID == router.HostID;
        }

        public override int GetHashCode()
        {
            return -1649111662 + HostID.GetHashCode();
        }

        public static bool operator ==(Router router1, Router router2)
        {
            return EqualityComparer<Router>.Default.Equals(router1, router2);
        }

        public static bool operator !=(Router router1, Router router2)
        {
            return !(router1 == router2);
        }
    }
}
