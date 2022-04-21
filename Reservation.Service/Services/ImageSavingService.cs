using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reservation.Models.Common;
using Reservation.Service.Interfaces;
using SixLabors.ImageSharp;

namespace Reservation.Service.Services
{
    public class ImageSavingService : IImageSavingService
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

        public async Task<KeyValuePair<bool, string>> SaveImageAsync(SaveImageClientModel model)
        {
            return await SendRequestAsync<SaveImageClientModel, KeyValuePair<bool, string>>(_client, HttpMethod.Post, "Reservation/SaveImage", model, CancellationToken.None);
        }

        private async Task<TResult> SendRequestAsync<T, TResult>(HttpClient client, HttpMethod method, string uri, T data, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(method, uri);
            
            string dataString = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(dataString, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
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