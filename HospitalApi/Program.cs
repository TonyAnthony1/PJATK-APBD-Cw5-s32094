using System.Text.Json.Serialization;
using HospitalApi.Data;
using HospitalApi.Repositories;
using HospitalApi.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPatientRepo, PatientRepo>();
builder.Services.AddScoped<IBedAssignmentRepo, BedAssignmentRepo>();


builder.Services.AddScoped<IPatientService, PatientServ>();
builder.Services.AddScoped<IBedAssignmentServ, BedAssignmentService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
    app.MapOpenApi();
    
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();