/*
 * Author & Creator : Nguyễn Nhựt Tường
 * Date: 16-3
 * Modifer-Date: 18-3
 * */
using System.Collections.Generic;

namespace FixedRouteTable
{
    public class RoutePath
    {
        public static RoutePath CreatePath(List<Router> path)
        {
            return new RoutePath(path);
        }


        /// <summary>
        /// Đường đi
        /// </summary>
        private List<Router> _path;

        public List<Router> Path
        {
            get => _path;
            private set => _path = value;
        }

        /// <summary>
        /// Tổng phí của đường này
        /// </summary>
        private int _cost = 0;

        public int Cost
        {
            get => _cost;
            private set => _cost = value;
        }

        /// <summary>
        /// Số Router phải đi qua, không tính Router source
        /// </summary>
        public int NumHop => Path.Count - 1;

        /// <summary>
        /// Xác định đây có phải là route hợp lệ không
        /// </summary>
        public bool IsValid => Path.Count > 1;
        public bool IsNotValid => Path.Count < 2;


        private RoutePath(List<Router> path)
        {
            Path = path;
            CaculateCost();
        }

        private void CaculateCost()
        {
            for (int i = 1; i < Path.Count; i++)
            {
                Cost += Path[i - 1].DirectedRoutersWithCost[Path[i]];
            }
        }
        static public int CaculateCost(List<Router> path)
        {
            int cost = 0;
            for (int i = 1; i < path.Count; i++)
            {
                cost += path[i - 1].DirectedRoutersWithCost[path[i]];
            }
            return cost;
        }

        public Router NextHop() => IsNotValid ? null : Path[1];
    }
}
