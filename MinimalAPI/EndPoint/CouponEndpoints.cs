using AutoMapper;
using FluentValidation;
using MinimalAPI.Data;
using MinimalAPI.Models.DTO;
using MinimalAPI.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MinimalAPI.EndPoint
{
    public static class CouponEndpoints
    {
        public static void ConfigureCouponEndPoints(this WebApplication app)
        {
            app.MapGet("/api/coupons", (ILogger<Program> _logger) =>
            {
                //_logger.Log(LogLevel.Information,  "Hello World");
                APIResponse response = new();
                response.Result = CouponStore.CouponList;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("GetCoupons").Produces<IEnumerable<APIResponse>>(200);

            app.MapGet("/api/coupon/{id:int}", (int id) =>
            {
                APIResponse response = new();
                response.Result = CouponStore.CouponList.FirstOrDefault(x => x.Id == id);
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("GetCoupon").Produces<APIResponse>(200);

            app.MapPost("/api/coupon", async ([FromBody] CouponCreateDto createCouponDTO, IMapper mapper, IValidator<CouponCreateDto> _validation) =>
            {
                APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

                var validateResult = await _validation.ValidateAsync(createCouponDTO);

                if (!validateResult.IsValid
                    || string.IsNullOrEmpty(createCouponDTO.Name)
                    || CouponStore.CouponList.FirstOrDefault(x => x?.Name?.ToLower() == createCouponDTO.Name.ToLower()) != null)
                {
                    response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ErrorMessage.ToString());
                    return Results.BadRequest(response);
                }

                Coupon coupon = mapper.Map<Coupon>(createCouponDTO);
                coupon.Id = CouponStore.CouponList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;

                CouponDto couponDTO = mapper.Map<CouponDto>(coupon);


                CouponStore.CouponList.Add(coupon);

                response.Result = couponDTO;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.Created;
                return Results.Ok(response);

                //return Results.Ok(coupon);
                //return Results.Created($"/api/coupon/{coupon.Id}", coupon);
                //return Results.CreatedAtRoute($"GetCoupon", new { id= couponDTO.Id}, couponDTO);
            }).WithName("CreateCoupon").Accepts<CouponCreateDto>("application/json").Produces<APIResponse>(201).Produces(400);

            app.MapPut("/api/coupon", async ([FromBody] CouponUpdateDto couponUpdateDto, IMapper mapper, IValidator<CouponUpdateDto> _validation) =>
            {
                APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

                var validateResult = await _validation.ValidateAsync(couponUpdateDto);

                if (!validateResult.IsValid)
                {
                    response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ErrorMessage.ToString());
                    return Results.BadRequest(response);
                }

                Coupon couponFromStore = CouponStore.CouponList.FirstOrDefault(x => x.Id == couponUpdateDto.Id);

                couponFromStore.IsActive = couponUpdateDto.IsActive;
                couponFromStore.Name = couponUpdateDto.Name;
                couponFromStore.Percent = couponUpdateDto.Percent;
                couponFromStore.LastUpdated = DateTime.Now;


                CouponDto couponDto = mapper.Map<CouponDto>(couponFromStore);

                response.Result = couponDto;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);

            }).WithName("UpdateCoupon").Accepts<CouponUpdateDto>("application/json").Produces<APIResponse>(200).Produces(400); ;

            app.MapDelete("/api/coupon/{id:int}", (int id) =>
            {
                APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
                Coupon coupon = CouponStore.CouponList.FirstOrDefault(x => x.Id == id);

                if (coupon == null)
                {
                    return Results.BadRequest(response);
                }

                CouponStore.CouponList.Remove(coupon);


                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
                return Results.Ok(response);

            }).WithName("DeleteCoupon").Produces<APIResponse>(204).Produces<APIResponse>(400);
        }
    }
}
