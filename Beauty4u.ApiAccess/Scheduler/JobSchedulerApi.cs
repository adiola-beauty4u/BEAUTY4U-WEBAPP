using Beauty4u.Interfaces.Api.Scheduler;
using Beauty4u.Models.Api.Scheduler;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Beauty4u.ApiAccess.Scheduler
{
    public class JobSchedulerApi : IJobSchedulerApi
    {
        private readonly HttpClient _httpClient;
        readonly string jobSchedulerEndpoint = "/JobScheduler";
        private readonly IConfiguration _config;

        public JobSchedulerApi(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task CreateForToday(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.GetAsync($"{_config["SchedulerApi"]}{jobSchedulerEndpoint}/create-for-today");

            response.EnsureSuccessStatusCode();

            var output = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            if (output != null && output.TryGetValue("message", out var message))
            {
                Console.WriteLine(message);
            }
        }

        public async Task<List<IQueuedJob>> GetQueuedJobs(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.GetAsync($"{_config["SchedulerApi"]}{jobSchedulerEndpoint}/queued-jobs");

            response.EnsureSuccessStatusCode();

            var output = await response.Content.ReadFromJsonAsync<List<QueuedJob>>();

            return output.ToList<IQueuedJob>();
        }
        public async Task CancelAllJobs(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.DeleteAsync($"{_config["SchedulerApi"]}{jobSchedulerEndpoint}/all");

            response.EnsureSuccessStatusCode();

            var output = await response.Content.ReadAsStringAsync();
        }
        public async Task CreateExecuteJob(string jwtToken, ICreateJobSchedule createJobSchedule)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);
                var schedulerApi = _config["SchedulerApi"];

                var response = await _httpClient.PostAsJsonAsync(
                    $"{schedulerApi}{jobSchedulerEndpoint}/create-execute-job",
                    createJobSchedule
                );

                var raw = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw response: {raw}");

                response.EnsureSuccessStatusCode();

                var output = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (output != null && output.TryGetValue("message", out var message))
                {
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                throw;
            }
        }

    }
}
