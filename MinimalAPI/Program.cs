using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI;
using MinimalAPI.Data;
using MinimalAPI.Models;
using MinimalAPI.Models.DTO;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/coupons", (ILogger<Program> _logger) =>
{
    //_logger.Log(LogLevel.Information,  "Hello World");
    APIResponse response = new();
    response.Result = CouponStore.CouponList;
    response.IsSuccess = true;
    response.StatusCode =HttpStatusCode.OK;

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

app.MapPost("/api/coupon", async ([FromBody] CouponCreateDto createCouponDTO, IMapper mapper, IValidator<CouponCreateDto> _validation ) => 
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest};


    var validateResult = await _validation.ValidateAsync(createCouponDTO);

    if (!validateResult.IsValid
        || string.IsNullOrEmpty(createCouponDTO.Name)
        || CouponStore.CouponList.FirstOrDefault(x => x?.Name?.ToLower() == createCouponDTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ErrorMessage.ToString());
        return Results.BadRequest(response);
    }
        

    //if (string.IsNullOrEmpty(createCouponDTO.Name))
    //{
    //    response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ToString());
    //    return Results.BadRequest(response);
    //}
        

    //if (CouponStore.CouponList.FirstOrDefault(x => x?.Name?.ToLower() == createCouponDTO.Name.ToLower()) != null)
    //{
    //    response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ToString());
    //    return Results.BadRequest(response);
    //}

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

app.MapPut("/api/coupon", () =>
{

});

app.MapDelete("/api/coupon/{id:int}", (int id) =>
{

});


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}