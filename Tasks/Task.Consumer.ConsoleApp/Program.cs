using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "turkey-01.rmq.cloudamqp.com",
    UserName = "yjmqzbrm",
    Password = "OnB9sGfUhnQi5tT7L3J9qtM_OSGF5lmv",
    VirtualHost= "yjmqzbrm"
};

var queue = "task-queue";
var exchange = "task-publish";
var bind = "task";

var connection = factory.CreateConnection();
//Here we create channel with session and model
using
var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
channel.QueueDeclare(queue, exclusive: false);
channel.QueueBind(queue, exchange, bind);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Task message received: {message}");
};

channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
Console.ReadKey();