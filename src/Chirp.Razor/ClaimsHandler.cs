namespace ClaimsHandler;

public class ClaimsHandler {

    private static HttpClient client = new () 
    {
        BaseAddress = new Uri("https://https://bdsagroup4chirprazor.azurewebsites.net/"),
    };

    public static async Task<String> GetClaimsAsync() 
    {
        //Including the Client; Defined  in HTTPClient
        //Response from Azure With Claim
        using HttpResponseMessage response = await client.GetAsync(".auth/me");
        //Insuring that the Response is valid
        
        response.EnsureSuccessStatusCode();

        string responseBody = response.Headers.ToString();
        // Above three lines can be replaced with new helper method below
        // string responseBody = await client.GetStringAsync(uri);

        return responseBody;
    }
}