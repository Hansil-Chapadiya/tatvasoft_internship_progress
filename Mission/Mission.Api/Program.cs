using Microsoft.EntityFrameworkCore;
using Mission.Entities.context;
using Mission.Entities.Entities;
using Mission.Repositories.IRepositories;
using Mission.Repositories.Repositories;
using Mission.Services.IServices;
using Mission.Services.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<MissionDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MissionDbContext>();

    if (!db.Users.Any(u => u.EmailAddress == "admin@tatvasoft.com"))
    {
        db.Users.Add(new User
        {
            FirstName = "Tatva",
            LastName = "Admin",
            EmailAddress = "admin@tatvasoft.com",
            Password = "Tatva@123",
            UserType = "admin",
            PhoneNumber = "9876543210",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false,
            UserImage = ""
        });
        db.SaveChanges();
    }
}


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


