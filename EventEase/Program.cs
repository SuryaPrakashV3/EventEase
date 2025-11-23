using EventEase.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register event service
builder.Services.AddSingleton<EventBase.Services.IEventService, EventBase.Services.InMemoryEventService>();
// Register user session (scoped per connection)
builder.Services.AddScoped<EventBase.Services.UserSessionService>();
// Register attendance service (in-memory)
builder.Services.AddSingleton<EventBase.Services.IAttendanceService, EventBase.Services.InMemoryAttendanceService>();
// Register file-backed user store
builder.Services.AddScoped<EventBase.Services.IUserStoreService, EventBase.Services.FileUserStoreService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
