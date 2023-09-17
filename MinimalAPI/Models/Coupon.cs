using MinimalAPI.Data;

namespace MinimalAPI.Models
{
    public class Coupon
    {
        public Coupon()
        {
            Created = DateTime.Now;
        }
        public int Id { get; set; } 
        public string? Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; private set; }
        public DateTime? LastUpdated { get; set; }
    }
}
