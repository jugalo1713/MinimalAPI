using MinimalAPI.Models;

namespace MinimalAPI.Data
{
    public static class CouponStore
    {
        public static List<Coupon> CopuponList = new List<Coupon>
        {
            new Coupon { Id= 1, Name = "10OFF", Percent= 10, IsActive= true },
            new Coupon { Id= 1, Name = "20OFF", Percent= 20, IsActive= false },
            new Coupon { Id= 1, Name = "30OFF", Percent= 30, IsActive= true }
        };
    }
}
