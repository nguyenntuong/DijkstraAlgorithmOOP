using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedRouteTable
{
    class LeastCostRoutePath : BestRoutePathBase
    {
        public static BestRoutePathBase CreateRoutePathStorage()
        {
            return new LeastCostRoutePath();
        }


        public override void CompareAndReplace(RoutePath routePath)
        {
            if (BestRoutePath == null)
            {
                BestRoutePath = routePath;
            }
            else
            {
                if (BestRoutePath.Cost == routePath.Cost)
                {
                    if (BestRoutePath.NumHop > routePath.NumHop)
                        BestRoutePath = routePath;
                }
                else if (BestRoutePath.Cost > routePath.Cost)
                {
                    BestRoutePath = routePath;
                }
            }
        }

        public override bool BreakCondition(List<Router> routePath)
        {
            return BestRoutePath != null
                && RoutePath.CaculateCost(routePath) > BestRoutePath.Cost;
        }

        public override bool BreakCondition(RoutePath routePath)
        {
            return BestRoutePath != null
                && routePath.Cost > BestRoutePath.Cost;
        }
    }
}
