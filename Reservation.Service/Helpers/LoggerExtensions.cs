using Microsoft.Extensions.Logging;

namespace Reservation.Service.Helpers
{
	public static class LoggerExtensions
	{
		public static void LogRequest(this ILogger logger, string url, object data)
		{
			logger.LogWarning("request: " + url + ", data: {@data}", data);
		}

		public static void LogResponse(this ILogger logger, string url, object data)
		{
			logger.LogWarning("response: " + url + ", data: {@data}", data);
		}
	}
}
