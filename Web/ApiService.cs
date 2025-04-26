namespace Web;

public class ApiService(HttpClient httpClient)
{
    public async Task<Dictionary<string, bool>> GetFeaturesAsync()
    {
        var response = await httpClient.GetAsync("/features");
        response.EnsureSuccessStatusCode();
        var features = await response.Content.ReadFromJsonAsync<Dictionary<string, bool>>();
        return features ?? new Dictionary<string, bool>();
    }

    public async Task<int?> GetCounterAsync()
    {
        var response = await httpClient.GetAsync("/counter");
        response.EnsureSuccessStatusCode();
        var count = await response.Content.ReadFromJsonAsync<int>();
        return count;
    }

    public async Task<int?> IncrementCounterAsync()
    {
        var response = await httpClient.PostAsync("/counter/increment", null);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var newCount = await response.Content.ReadFromJsonAsync<int>();
        return newCount;
    }
}