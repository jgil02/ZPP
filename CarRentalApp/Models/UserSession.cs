using CarRentalApp.Models;

namespace CarRentalApp
{
    public static class UserSession
    {
        
        public static Client? CurrentClient { get; set; }

       
        public static Worker? CurrentWorker { get; set; }
    }
}