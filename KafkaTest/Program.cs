using KafkaTest.Kafka.Consumers;
using KafkaTest.Kafka.Producers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IKafkaProducer>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var bootstrapServers = configuration["KafkaProducerConfig:bootstrapServers"];

    return new KafkaProducer(bootstrapServers!);
});

builder.Services.AddSingleton<IKafkaConsumer>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var bootstrapServers = configuration["KafkaConsumerConfig:BootstrapServers"];
    var groupId = configuration["KafkaConsumerConfig:GroupId"];

    return new KafkaConsumer(bootstrapServers!, groupId!);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/generateOdities", ([FromServices] IKafkaProducer kafkaProducer, int number) =>
{
    dynamic obj = new System.Dynamic.ExpandoObject();
    obj.A = number;

    if ((obj.A & 1) == 1)
    {
        obj.B = "It is odd, isn't it?";
    }

    var topic = "oddities";
    var key = "Key_1";
    kafkaProducer.SendMessage(topic, key, obj);

    return obj;
});

app.MapGet("/getOddities", ([FromServices] IKafkaConsumer kafkaConsumer) =>
{
    var topic = "oddities";

    kafkaConsumer.StartConsuming(topic);
});


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
