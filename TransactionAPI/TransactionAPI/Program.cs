using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TransactionAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IPaymentService, PaymentService>();

// Setting the JWT authentication  
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "financial-system",
            ValidAudience = "transaction-api",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3ss4-s3r4-4-m1nh4-s3cr3t-k3y-t0-us3-w1th-th1s-4pp"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Just important thing, the order matter in this case
// The correct way is to use the UseHttpsRedirection first to make the security protocol available to the application
// after that UseAuthentication before UseAuthorization to certified that app ever have the user authentication and your roles
// and, more importantly, MapControllers after configuring all these things to make sure that this application is configured correctly
// with Authentication and Authorization on all your endpoints.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
