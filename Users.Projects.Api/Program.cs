using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Users.Projects.Api.Data;
using Users.Projects.Api.Data.Configuration;
using Users.Projects.Api.Services.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UsersProjectsDbConnectionString");

builder.Services.AddDbContext<UsersProjectsDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(sg =>
{
    sg.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Users.Projects.Api",
        Description = "The API for the Users.Projects App."
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersProjectsDbContext>();
        await DatabaseInitializer.InitializeDatabase(dbContext);
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => 
    x.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.MapControllers();

app.Run();
