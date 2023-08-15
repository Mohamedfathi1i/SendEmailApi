using SendEmailApi.Extension.Settings;
using SendEmailApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IMailingService, MailingServices>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseStaticFiles();

//app.UseAuthorization();

app.MapControllers();

app.Run();
