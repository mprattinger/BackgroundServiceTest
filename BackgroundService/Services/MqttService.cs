using BackgroundService.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using System.Text;
using System.Text.Json;
using Hosting = Microsoft.Extensions.Hosting;

namespace BackgroundService.Services;

public class MqttService : Hosting.BackgroundService
{
    private readonly AppState _appState;
    private readonly MqttFactory _mqttFactory;
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _mqttClientOptions;

    public MqttService(AppState appState)
    {
        _appState = appState;

        _mqttFactory = new MqttFactory();
        _mqttClient = _mqttFactory.CreateMqttClient();

        _mqttClientOptions = new MqttClientOptionsBuilder()
               .WithTcpServer("homeassistant")
               .WithCredentials("mprattinger", "Fl61291420")
               .Build();

        _mqttClient.ApplicationMessageReceivedAsync +=  async e =>
        {           
            if (e == null)
            {
                Console.WriteLine("Error: Event is null");
                return;
            }
            if (e.ApplicationMessage == null)
            {
                Console.WriteLine("Error: ApplicationMessage is null");
                return;
            }

            var topic = e.ApplicationMessage.Topic;
            var msg = e.ApplicationMessage!.Payload;
            var temp = Encoding.UTF8.GetString(msg);

            Console.WriteLine($"Received application message for topic {topic}: {temp}");

            var data = JsonSerializer.Deserialize<TempData>(temp);
            if(data == null)
            {
                Console.WriteLine("Error: TempData is null");
                return;
            }

            await _appState.NewTempReceived(data);
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _mqttClient.ConnectAsync(_mqttClientOptions, stoppingToken);

        var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
               .WithTopicFilter(
                   f =>
                   {
                       f.WithTopic("env/#");
                   })
        .Build();

        await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        Console.WriteLine("MQTT client subscribed to topic.");

    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder().WithReason(MqttClientDisconnectReason.NormalDisconnection).Build());
    }
}
