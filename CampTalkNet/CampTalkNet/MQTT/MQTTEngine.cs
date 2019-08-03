using System;
using System.Collections.Generic;
using System.Text;
using CampTalkNet.Service.Validator;
using MQTTnet;
using MQTTnet.Server;

namespace CampTalkNet.MQTT
{
    internal class MqttEngine
    {
        public static MqttEngine GetInstance() => MqttEngine.syncEngine ?? (MqttEngine.syncEngine = new MqttEngine());

        public async void Init()
        {
            if (this.IsInit)
            {
                return;
            }
            var mqttServer = new MqttFactory().CreateMqttServer();
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionValidator(c => AuthValidator.Auth(c.ClientId, c.Username, c.Password))
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1803);
            await mqttServer.StartAsync(optionsBuilder.Build());
            this.IsInit = true;
        }

        private MqttEngine() { }

        public bool IsInit { get; private set; } = false;

        private static MqttEngine syncEngine = null;
    }
}
