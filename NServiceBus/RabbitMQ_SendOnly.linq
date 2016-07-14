<Query Kind="Program">
  <NuGetReference>NServiceBus</NuGetReference>
  <NuGetReference>NServiceBus.RabbitMQ</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>NServiceBus</Namespace>
</Query>

void Main()
{
	NServiceBusBootstrapper();

	//TODO: Add a reference your Message library

	//Endpoint.Send("queuename", new ???{});
}

// Configuration
string RabbitMQConnectionString = Environment.GetEnvironmentVariable("RabbitMQTransport.ConnectionString");
const string EndpointName = "LINQPad";
const string ErrorQueue = "error";
string PathToLogDirectory = System.IO.Path.GetTempPath();
NServiceBus.Logging.LogLevel LogLevel = NServiceBus.Logging.LogLevel.Warn;

#region NServiceBus Setup
ISendOnlyBus Endpoint = null;
void NServiceBusBootstrapper()
{
	var defaultFactory = NServiceBus.Logging.LogManager.Use<NServiceBus.Logging.DefaultFactory>();
	defaultFactory.Directory(PathToLogDirectory);
	defaultFactory.Level(LogLevel);
	
	var config = new BusConfiguration();
	config.UseTransport<RabbitMQTransport>().ConnectionString(RabbitMQConnectionString);
	config.AssembliesToScan(GetType().Assembly, typeof(NServiceBus.Transports.RabbitMQ.IManageRabbitMqConnections).Assembly);
	config.UseSerialization<JsonSerializer>();
	
	config.EndpointName(EndpointName);
	config.EnableInstallers();
	
	config.Conventions()
		.DefiningCommandsAs(t => true);
	
	Endpoint = NServiceBus.Bus.CreateSendOnly(config);	
}

class ConfigErrorQueue : NServiceBus.Config.ConfigurationSource.IProvideConfiguration<NServiceBus.Config.MessageForwardingInCaseOfFaultConfig>
{
	public NServiceBus.Config.MessageForwardingInCaseOfFaultConfig GetConfiguration()
	{
		return new NServiceBus.Config.MessageForwardingInCaseOfFaultConfig
		{
			ErrorQueue = ErrorQueue
		};
	}
}
#endregion