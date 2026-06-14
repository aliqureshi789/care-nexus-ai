using CareNexus.Api.Services;
using CareNexus.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Dependency Injection
builder.Services.AddScoped<IAgentOrchestrator, AgentOrchestrator>();
builder.Services.AddScoped<IWorkContextService, WorkContextService>();
builder.Services.AddScoped<GraphAuthService>();
builder.Services.AddScoped<SharePointService>();

builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();
builder.Services.AddScoped<IReasoningService, ReasoningService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    service = "CARE-NEXUS AI Backend",
    timestamp = DateTime.UtcNow
}));

app.Run();