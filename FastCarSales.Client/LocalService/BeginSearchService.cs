using FastCarSales.Client.Events;

namespace FastCarSales.Client.LocalService
{
    public class BeginSearchService
    {
        private readonly EventAggregator EvntAggregator = null!;

        public BeginSearchService(EventAggregator evntAggregator)
        {
            EvntAggregator = evntAggregator;
        }

        //public void BeginSearch() { 
        //    EvntAggregator.publish(new BeginSearchArgs(new post))
            
        //    }


    }
}
