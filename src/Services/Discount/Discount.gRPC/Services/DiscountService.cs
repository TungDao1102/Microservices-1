using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services
{
    public class DiscountService(DiscountContext dbContext) : DiscountProtoService.DiscountProtoServiceBase
    {
        public async override Task<ListCouponModelResponse> GetAllDiscount(GetAllDiscountRequest request, ServerCallContext context)
        {
            var coupons = await dbContext.Coupons.ToListAsync();
            var couponModels = coupons.Adapt<IEnumerable<CouponModel>>();
            var couponResponse = new ListCouponModelResponse();
            couponResponse.Coupon.AddRange(couponModels);
            return couponResponse;
        }

        public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            coupon ??= new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount"
            };

            return coupon.Adapt<CouponModel>();
        }

        public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            await dbContext.AddAsync(coupon);
            await dbContext.SaveChangesAsync();
            return coupon.Adapt<CouponModel>();
        }

        public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Not found"));
            }
            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            return new DeleteDiscountResponse { Success = true };
        }

        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            dbContext.Update(coupon);
            await dbContext.SaveChangesAsync();
            return coupon.Adapt<CouponModel>();
        }
    }
}
