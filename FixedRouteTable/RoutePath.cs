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
        public static RoutePath CreatePath(List<Router> path, bool ismore1000path = false)
        {
            return new RoutePath(path,ismore1000path);
        }

        private bool _morethan1000Path;

        public bool IsMoreThan1000Path
        {
            get { return _morethan1000Path; }
            private set { _morethan1000Path = value; }
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
        private int _cost;

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


        private RoutePath(List<Router> path,bool ismorethan1000path)
        {
            IsMoreThan1000Path = ismorethan1000path;
            Path = path;
            CaculateCost();
        }

        private int CaculateCost()
        {
            for (int i = 1; i < Path.Count; i++)
            {
                Cost += Path[i - 1].DirectedRoutersWithCost[Path[i]];
            }
            return Cost;
        }

        public Router NextHop() => IsNotValid ? null : Path[1];
    }
}
