using Michalski.ComputerPheripherals.BL;

var builder = WebApplication.CreateBuilder(args);

// Add BLC as a service
string daoLibrary = builder.Configuration["DaoLibrary"] ?? "Michalski.ComputerPheripherals.DAO.dll";
builder.Services.AddSingleton<BLC>(new BLC(daoLibrary));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
