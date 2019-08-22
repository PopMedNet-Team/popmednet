Add-Type -AssemblyName System.Net.Http

$rootURL = "Set-The-Root-Domain-Here"
$requestID = "Set-The-Request-Id-Here"
$userName = "Set-The-UserName-Here"
$password = "Set-The-Password-Here"

#Setting up the HTTP Client
[Net.ServicePointManager]::SecurityProtocol = 'tls12';
$client = New-Object -TypeName System.Net.Http.Httpclient
$client.BaseAddress = $rootURL
$client.DefaultRequestHeaders.Authorization =  [System.Net.Http.Headers.AuthenticationHeaderValue]::new("Basic",
        [System.Convert]::ToBase64String(
            [System.Text.Encoding]::UTF8.GetBytes([string]::Format("{0}:{1}", $userName, $password))));

#Making the Request To RequestDatamarts to get the RequestId
$RequestDataMartsResponse = $client.GetAsync('/Requests/RequestDataMarts?requestID=' + $requestID).Result
if($RequestDataMartsResponse.IsSuccessStatusCode)
{
    #Parse the Response and Get to the RequestDataMarts List
    $RequestDataMartsContentBody = $RequestDataMartsResponse.Content.ReadAsStringAsync().Result | ConvertFrom-Json 
    $requestDatamarts = $RequestDataMartsContentBody[0].results
    $requestDatamarts
}
else
{
    $responseBody = $RequestDataMartsResponse.Content.ReadAsStringAsync().Result

    $errorMessage = [string]::Format("Status code {0}. Reason {1}. Server reported the following message: {2}.", $RequestDataMartsResponse.StatusCode, $RequestDataMartsResponse.ReasonPhrase, $responseBody)

    $errorMessage
} 

$RequestDataMartsResponse = $client.GetAsync('/Requests/RequestsByRoute').Result
if($RequestDataMartsResponse.IsSuccessStatusCode)
{
    #Parse the Response and Get to the RequestDataMarts List
    $RequestDataMartsContentBody = $RequestDataMartsResponse.Content.ReadAsStringAsync().Result | ConvertFrom-Json 
    $requestDatamarts = $RequestDataMartsContentBody[0].results
    $requestDatamarts
}
else
{
    $responseBody = $RequestDataMartsResponse.Content.ReadAsStringAsync().Result

    $errorMessage = [string]::Format("Status code {0}. Reason {1}. Server reported the following message: {2}.", $RequestDataMartsResponse.StatusCode, $RequestDataMartsResponse.ReasonPhrase, $responseBody)

    $errorMessage
}

