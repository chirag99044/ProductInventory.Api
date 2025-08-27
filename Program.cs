using Microsoft.EntityFrameworkCore;
using ProductInventory.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Define a CORS policy
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ng", policy =>
    policy.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true)); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting(); // Routing must come before CORS

app.UseCors(myAllowSpecificOrigins); // Use the CORS policy
app.UseHttpsRedirection();
app.UseDeveloperExceptionPage(); // Add this before UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

