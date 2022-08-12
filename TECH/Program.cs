using Microsoft.EntityFrameworkCore;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
});
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataBaseEntityContext>(options =>
{
    // Đọc chuỗi kết nối
    string connectstring = builder.Configuration.GetConnectionString("AppDbContext");
    options.UseSqlServer(connectstring);
});
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(EFUnitOfWork));
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EFRepository<,>));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IContractsRepository, ContractsRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IContractsService, ContractsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductsService, ProductsService>();

//builder.Services.AddMemoryCache();

// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{

    endpoints.MapAreaControllerRoute(
       name: "TaiKhoan",
       areaName: "Admin",
       pattern: "admin/quan-ly-tai-khoan",
       defaults: new { controller = "AppUsers", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "LienHe",
       areaName: "Admin",
       pattern: "admin/quan-ly-lien-he-tu-van",
       defaults: new { controller = "Contract", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "DanhMuc",
       areaName: "Admin",
       pattern: "admin/quan-ly-danh-muc",
       defaults: new { controller = "Category", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "ThemMoiSanPham",
       areaName: "Admin",
       pattern: "admin/tao-moi-san-pham",
       defaults: new { controller = "Product", action = "AddView" });

    endpoints.MapAreaControllerRoute(
      name: "DanhSachSanPham",
      areaName: "Admin",
      pattern: "admin/quan-ly-san-pham",
      defaults: new { controller = "Product", action = "Index" });


    endpoints.MapAreaControllerRoute(
       name: "admin",
       areaName: "Admin",
       pattern: "admin/{controller=Home}/{action=Index}/{id?}");

    


    //// Topic List
    //routes.MapControllerRoute(
    //    name: "topic_listing",
    //    pattern: "bo-suu-tap-du-an",
    //    defaults: new { controller = "Topic", action = "List" });


    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

});


//app.MapControllerRoute(

//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
