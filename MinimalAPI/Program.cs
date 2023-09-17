using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Data;
using MinimalAPI.Models;
using MinimalAPI.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
    _logger.Log(LogLevel.Information,  "Hello World");
    return Results.Ok(CouponStore.CouponList);
}).WithName("GetCoupons").Produces<IEnumerable<Coupon>>(200);

app.MapGet("/api/coupon/{id:int}", (int id) =>
{
    return Results.Ok(CouponStore.CouponList.FirstOrDefault(x => x.Id == id));
}).WithName("GetCoupon").Produces<Coupon>(200);

app.MapPost("/api/coupon", ([FromBody] CouponCreateDto createCouponDTO, IMapper mapper) => 
{
    if (string.IsNullOrEmpty(createCouponDTO.Name))
        return Results.BadRequest("Invalid Id or coupon name");

    if (CouponStore.CouponList.FirstOrDefault(x => x?.Name?.ToLower() == createCouponDTO.Name.ToLower()) != null)
        return Results.BadRequest("Coupon name exists");

    Coupon coupon = mapper.Map<Coupon>(createCouponDTO);
    coupon.Id = CouponStore.CouponList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;

    CouponDto couponDTO = mapper.Map<CouponDto>(coupon);


    CouponStore.CouponList.Add(coupon);

    //return Results.Ok(coupon);
    //return Results.Created($"/api/coupon/{coupon.Id}", coupon);
    return Results.CreatedAtRoute($"GetCoupon", new { id= couponDTO.Id}, couponDTO);
}).WithName("CreateCoupon").Accepts<CouponCreateDto>("application/json").Produces<Coupon>(201).Produces(400);

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