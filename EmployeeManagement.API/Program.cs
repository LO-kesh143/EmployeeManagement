using EmployeeManagement.API.Profiles;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Repository.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Connection String Setup
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ✅ Add DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAutoMapper(typeof(EmployeeProfile).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ✅ Register Controllers
builder.Services.AddControllers();

// ✅ Configure CORS if needed (optional, example shown)
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// });

// ✅ Register Business Services, Repositories via Dependency Injection
// builder.Services.AddScoped<IEmployeeService, EmployeeService>();
// builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
