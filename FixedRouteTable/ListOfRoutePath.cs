/*
 * Author & Creator : Nguyễn Nhựt Tường
 * Date: 16-3
 * Modifer: 18-3
 * */
using System.Collections.Generic;
using System.Linq;

namespace FixedRouteTable
{
    public class ListOfRoutePath
    {
        public static ListOfRoutePath CreateCacheListPath()
        {
            return new ListOfRoutePath();
        }

        /// <summary>
        /// Danh sách tất cả các tuyến đường đi được
        /// </summary>
        private List<RoutePath> _routePaths;

        public List<RoutePath> RoutePaths
        {
            get => _routePaths;
            private set => _routePaths = value;
        }

        /// <summary>
        /// Có thể đến đích hay không
        /// </summary>
        public bool CanRoute => RoutePaths.Count > 0;

        /// <summary>
        /// Không thể đến được đích phải không
        /// </summary>
        public bool CantRoute => RoutePaths.Count == 0;

        private ListOfRoutePath()
        {
            RoutePaths = new List<RoutePath>();
        }

        public void Add(RoutePath routePath)
        {
            RoutePaths.Add(routePath);
        }

        /// <summary>
        /// Lấy ra tuyến đường có đường đi qua ít Router nhất
        /// </summary>
        /// <returns></returns>
        public RoutePath GetMinimunHopPath()
        {
            return RoutePaths
                .OrderBy(orb => orb.Cost)
                .Where(o => o.NumHop == RoutePaths.Min(i => i.NumHop))
                .First();
        }

        /// <summary>
        /// Lấy ra tuyến đường có chi phí thấp nhất
        /// </summary>
        /// <returns></returns>
        public RoutePath GetLeastCostPath()
        {
            return RoutePaths
                .OrderBy(orb => orb.NumHop)
                .Where(o => o.Cost == RoutePaths.Min(i => i.Cost))
                .First();
        }
    }
}
