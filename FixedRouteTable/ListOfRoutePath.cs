/*
 * Author & Creator : Nguyễn Nhựt Tường
 * Date: 16-3
 * Modifer-Date: 18-3
 * */
using System.Collections.Generic;
using System.Linq;

namespace FixedRouteTable
{
    public class ListOfRoutePath
    {
        public static ListOfRoutePath CreateRoutePathsStorage()
        {
            return new ListOfRoutePath();
        }

        /// <summary>
        /// Danh sách tất cả các tuyến đường đi được
        /// điều kiện LeastCost
        /// </summary>
        private RoutePath _routePathsWithLeastCost;

        public RoutePath RoutePathsWithLeastCost
        {
            get => _routePathsWithLeastCost;
            private set => _routePathsWithLeastCost = value;
        }
        /// <summary>
        /// Danh sách tất cả các tuyến đường đi được
        /// điều kiện MinimumHop
        /// </summary>
        private RoutePath _routePathsWithMinimumHop;

        public RoutePath RoutePathsWithMinimumHop
        {
            get { return _routePathsWithMinimumHop; }
            set { _routePathsWithMinimumHop = value; }
        }

        /// <summary>
        /// Có thể đến đích hay không
        /// </summary>
        public bool CanRoute => RoutePathsWithLeastCost!=null;

        /// <summary>
        /// Không thể đến được đích phải không
        /// </summary>
        public bool CantRoute => RoutePathsWithLeastCost == null;

        private ListOfRoutePath()
        {
            RoutePathsWithLeastCost = null;
            RoutePathsWithMinimumHop = null;
        }

        public void Add(RoutePath routePath)
        {
            if (RoutePathsWithMinimumHop == null)
            {
                RoutePathsWithMinimumHop = routePath;
            }
            else
            {
                if (RoutePathsWithMinimumHop.NumHop > routePath.NumHop)
                {
                    RoutePathsWithMinimumHop = routePath;
                }
            }
            if (RoutePathsWithLeastCost == null)
            {
                RoutePathsWithLeastCost = routePath;
            }
            else
            {
                if (RoutePathsWithLeastCost.Cost > routePath.Cost)
                {
                    RoutePathsWithLeastCost = routePath;
                }
            }
        }

        /// <summary>
        /// Lấy ra tuyến đường có đường đi qua ít Router nhất
        /// </summary>
        /// <returns></returns>
        public RoutePath GetMinimunHopPath()
        {
            return RoutePathsWithMinimumHop;
        }

        /// <summary>
        /// Lấy ra tuyến đường có chi phí thấp nhất
        /// </summary>
        /// <returns></returns>
        public RoutePath GetLeastCostPath()
        {
            return RoutePathsWithLeastCost;
        }
    }
}
