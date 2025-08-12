using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);

// Load API base URL from configuration
var apiBase = builder.Configuration["Api:BaseUrl"];
if (string.IsNullOrWhiteSpace(apiBase))
{
    throw new InvalidOperationException("Missing Api:BaseUrl in appsettings.json");
}


builder.Services
    .AddHttpClient<IQuizServiceProxy, QuizServiceProxy>(client =>
    {
        client.BaseAddress = new Uri(apiBase);
    });

builder.Services
    .AddSingleton<IQuizService, QuizService>();

builder.Services
    .AddHttpClient<ICourseServiceProxy, CourseServiceProxy>(client =>
    {
        client.BaseAddress = new Uri(apiBase);
    });

builder.Services
    .AddHttpClient<IExerciseServiceProxy, ExerciseServiceProxy>(c =>
    {
        c.BaseAddress = new Uri(apiBase);
    });

builder.Services
    .AddHttpClient<IUserServiceProxy, UserServiceProxy>(client =>
    {
        client.BaseAddress = new Uri(apiBase);
    });

builder.Services
    .AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<ICoinsServiceProxy, CoinsServiceProxy>(); 
builder.Services.AddScoped<ICoinsService, CoinsService>(); 

builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllersWithViews();

builder.Services.Configure<RazorViewEngineOptions>(opts =>
{
    opts.ViewLocationFormats.Add("/Views/Exercise/{0}.cshtml");
    opts.ViewLocationFormats.Add("~/Views/Exercise/{0}.cshtml");
});

builder.Services.AddRazorPages();

// ✅ CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:7037")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials()
                     .WithExposedHeaders("Content-Type", "Accept");
    });
});

// Register HTTP clients for proxies
builder.Services.AddHttpClient<IQuizServiceProxy, QuizServiceProxy>(client =>
{
    client.BaseAddress = new Uri(apiBase);
});

builder.Services.AddHttpClient<ICourseServiceProxy, CourseServiceProxy>(client =>
{
    client.BaseAddress = new Uri(apiBase);
});

builder.Services.AddHttpClient<IExerciseServiceProxy, ExerciseServiceProxy>(client =>
{
    client.BaseAddress = new Uri(apiBase);
});

builder.Services.AddHttpClient<RoadmapServiceProxy>();
builder.Services.AddScoped<IRoadmapService, RoadmapService>();
builder.Services.AddScoped<IRoadmapServiceProxy, RoadmapServiceProxy>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISectionServiceProxy, SectionServiceProxy>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<IUserServiceProxy, UserServiceProxy>(client =>
{
    client.BaseAddress = new Uri(apiBase);
});


// Register services
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ISectionService, SectionService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "quiz",
    pattern: "Quiz/{action}/{id}",
    defaults: new { controller = "Quiz" });

app.MapControllerRoute(
    name: "exam",
    pattern: "Exam/{action}/{id}",
    defaults: new { controller = "Exam" });

app.MapControllerRoute(
    name: "courses",
    pattern: "Course/{action=ViewCourses}/{id?}");

app.MapControllerRoute(
    name: "exercises",
    pattern: "Exercise/{action=Index}/{id?}",
    defaults: new { controller = "Exercise" });

app.MapControllerRoute(
    name: "coursePreview",
    pattern: "Course/{id?}",
    defaults: new { controller = "Course", action = "CoursePreview" });

app.MapControllerRoute(
    name: "module",
    pattern: "Module/{id:int}",
    defaults: new { controller = "Module", action = "Details" });

    
app.Run();
