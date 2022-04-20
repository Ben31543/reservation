using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reservation.Models.Common;
using Reservation.Service.Interfaces;

namespace Reservation.Service.Services
{
    public class ImageSavingService: IImageSavingService
    {
        private readonly ILogger<ImageSavingService> _logger;
        private HttpClient _client;

        public ImageSavingService(
            ILogger<ImageSavingService> logger,
            HttpClient client)
        {
            _logger = logger;
            _client = client;
        }
        
        public async Task<string> SaveImageAsync(IFormFile file, string resourceHost, string imagePath)
        {
            var data = new SaveImageClientModel
            {
                File = file,
                ImagePath = imagePath,
                ResourceHost = resourceHost
            };

            var result = await SendRequestAsync<SaveImageClientModel, string>(_client, HttpMethod.Post, "Reservation/SaveImage", data, CancellationToken.None);
            return result;
        }
        
        private async Task<TResult> SendRequestAsync<T, TResult>(HttpClient client, HttpMethod method, string uri, T data, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(method, uri);

            if (data != null)
            {
                string dataString = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(dataString, Encoding.UTF8, "application/json");
            }

            var response = await client.SendAsync(request, cancellationToken);
            var tResult = response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error ({(int)response.StatusCode}[{response.StatusCode}]) at requesting {response.RequestMessage?.Method.Method} {uri}", response.RequestMessage);
            }

            var result = await tResult;
            if (!string.IsNullOrWhiteSpace(result))
            {
                try
                {
                    return JsonConvert.DeserializeObject<TResult>(result);
                }
                catch (JsonReaderException ex)
                {
                    _logger.LogError($"Error parsing {response.RequestMessage?.Method.Method} {uri} response json", ex);
                }
            }

            return default;
        }
    }
}