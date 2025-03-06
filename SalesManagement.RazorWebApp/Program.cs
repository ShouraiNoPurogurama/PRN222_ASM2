using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SalesManagement.RazorWebApp.Hubs;
using SalesManagement.Repositories.DBContext;
using SalesManagement.Repository.DBContext.Interceptors;
using SalesManagement.Repository.UoW;
using SalesManagement.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

builder.Services.AddControllers();

builder.Services.AddDbContext<SalesManagementDBContext>((sp, opt) =>
{
    opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ValidationService>();

builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

builder.Services.AddSignalR();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    await next();

    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        // Re-execute the request so the user gets the error page
        string originalPath = ctx.Request.Path.Value;
        ctx.Items["originalPath"] = originalPath;
        ctx.Request.Path = "/NotFound";
        await next();
    }
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<SalesManagementHubs>("/salesManagementHub");


app.Run();