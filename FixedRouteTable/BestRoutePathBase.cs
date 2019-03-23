using System.Collections.Generic;

namespace FixedRouteTable
{
    public abstract class BestRoutePathBase
    {
        /// <summary>
        /// Danh sách tất cả các tuyến đường đi được
        /// điều kiện
        /// </summary>
        private RoutePath _bestRoutePath = null;

        public RoutePath BestRoutePath
        {
            get { return _bestRoutePath; }
            protected set { _bestRoutePath = value; }
        }

        /// <summary>
        /// Có thể đến đích hay không
        /// </summary>
        public bool CanRoute => BestRoutePath != null && BestRoutePath.IsValid;

        /// <summary>
        /// Không thể đến được đích phải không
        /// </summary>
        public bool CantRoute => BestRoutePath == null || BestRoutePath.IsNotValid;

        public abstract void CompareAndReplace(RoutePath routePath);

        /// <summary>
        /// Điều kiện dừng tối ưu khi chạy đệ quy
        /// </summary>
        /// <param name="routePath"></param>
        /// <returns></returns>
        public abstract bool BreakCondition(RoutePath routePath);

        public abstract bool BreakCondition(List<Router> routePath);

        public RoutePath GetBestPath()
        {
            return BestRoutePath;
        }
    }
}
