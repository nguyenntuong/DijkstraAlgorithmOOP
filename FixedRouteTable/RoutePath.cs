﻿using System.Collections.Generic;

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


        private RoutePath(List<Router> path)
        {
            Path = path;
            CaculateCost();
        }

        private int CaculateCost()
        {
            for (int i = 1; i < Path.Count; i++)
            {
                Cost += Path[i - 1].DirectedRouters[Path[i]];
            }
            return Cost;
        }

        public Router NextHop() => IsNotValid ? null : Path[1];
    }
}
