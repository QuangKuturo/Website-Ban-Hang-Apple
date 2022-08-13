﻿using Microsoft.EntityFrameworkCore;
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

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
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IDistrictsRepository, DistrictsRepository>();
builder.Services.AddScoped<IWardsRepository, WardsRepository>();
builder.Services.AddScoped<IFeeRepository, FeeRepository>();


builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IContractsService, ContractsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IReviewsService, ReviewsService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IDistrictsService, DistrictsService>();
builder.Services.AddScoped<IWardsService, WardsService>();
builder.Services.AddScoped<IFeeService, FeeService>();

//builder.Services.AddMemoryCache();

// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

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
      name: "UserDetail",
      areaName: "Admin",
      pattern: "admin/thong-tin-ca-nhan",
      defaults: new { controller = "AppUsers", action = "ViewDetail" });

    endpoints.MapAreaControllerRoute(
      name: "ChangePass",
      areaName: "Admin",
      pattern: "admin/doi-mat-khau",
      defaults: new { controller = "AppUsers", action = "ChangePassWord" });

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
     name: "ThemMoiBaiViet",
     areaName: "Admin",
     pattern: "admin/tao-moi-bai-viet",
     defaults: new { controller = "Post", action = "AddView" });

    endpoints.MapAreaControllerRoute(
      name: "DanhSachBaiViet",
      areaName: "Admin",
      pattern: "admin/quan-ly-bai-viet",
      defaults: new { controller = "Post", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-don-hang",
      defaults: new { controller = "Orders", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-danh-gia",
      defaults: new { controller = "Reviews", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-phi-van-chuyen",
      defaults: new { controller = "Fee", action = "Index" });


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
       name: "user",
       pattern: "/dang-ky",
       defaults: new { controller = "Users", action = "Register" });

    endpoints.MapControllerRoute(
       name: "userlogin",
       pattern: "/dang-nhap",
       defaults: new { controller = "Users", action = "Login" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


});


//app.MapControllerRoute(

//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
