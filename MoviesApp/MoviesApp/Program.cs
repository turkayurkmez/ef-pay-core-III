using Microsoft.EntityFrameworkCore;
using Movies.DataApplication.Data;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Eğer bütün select sorguları NoTracking davranışı ile çalışacaksa UseQueryTrackingBehavior fonksiyonu ile konfigure edilebilir.
//Query Splittting: Birbirlerinden bağımsız iki ilişkili data 
//(Mesela hem yönetmen hem de yorumlar gibi) çekiyorsanız; kartezyen patlama denen satırların tekrar etmesi durumunu önlemek için
//Sorgu bölünebilir ve bu davranış tüm db modeline yansıtılabilir:
var connectionString = builder.Configuration.GetConnectionString("db");
builder.Services.AddDbContext<MoviesDbContext>(opt => opt.UseLazyLoadingProxies(false)
                                                         .UseSqlServer(connectionString,
                                                                       opt => opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                                                         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)


);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
