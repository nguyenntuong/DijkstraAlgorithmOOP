using System.Collections.Generic;

namespace FixedRouteTable
{
    class MinimumHopRoutePath : BestRoutePathBase
    {
        public static BestRoutePathBase CreateRoutePathStorage()
        {
            return new MinimumHopRoutePath();
        }

        public override void CompareAndReplace(RoutePath routePath)
        {
            if (BestRoutePath == null)
            {
                BestRoutePath = routePath;
            }
            else
            {
                if (BestRoutePath.NumHop == routePath.NumHop)
                {
                    if (BestRoutePath.Cost > routePath.Cost)
                        BestRoutePath = routePath;
                }
                else if (BestRoutePath.NumHop > routePath.NumHop)
                {
                    BestRoutePath = routePath;
                }
            }
        }

        public override bool BreakCondition(List<Router> routePath)
        {
            return BestRoutePath != null
                &&
                routePath.Count > (BestRoutePath.NumHop + 1);
        }

        public override bool BreakCondition(RoutePath routePath)
        {
            return BestRoutePath != null
                &&
                routePath.NumHop > BestRoutePath.NumHop;
        }
    }
}
