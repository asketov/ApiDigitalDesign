using DAL;

namespace ApiDigitalDesign.Services
{
    public class SubscribeService
    {
        private readonly DataContext _db;
        public SubscribeService(DataContext db)
        {
            _db = db;
        }
        
    }
}
