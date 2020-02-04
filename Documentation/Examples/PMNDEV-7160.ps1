Add-Type -AssemblyName System.Net.Http

$rootURL = "Set-The-Root-Domain-Here"
$userID = "Set-The-User-Id-Here"
$userName = "Set-The-UserName-Here"
$password = "Set-The-Password-Here"

#Function to read the Response Body and return the Results Expected
function ReadResponse($responseBody){
    $responseContent = $responseBody.Content.ReadAsStringAsync().Result | ConvertFrom-Json 
    return $responseContent[0].results
}

#Function to read and format the Response Error
function ReadError($responseBody){
    $body = $responseBody.Content.ReadAsStringAsync().Result

    $errorMessage = [string]::Format("Status code {0}. Reason {1}. Server reported the following message: {2}.", $responseBody.StatusCode, $responseBody.ReasonPhrase, $body)
}



#Setting up the HTTP Client
[Net.ServicePointManager]::SecurityProtocol = 'tls12';
$client = New-Object -TypeName System.Net.Http.Httpclient
$client.BaseAddress = $rootURL
$client.DefaultRequestHeaders.Authorization =  [System.Net.Http.Headers.AuthenticationHeaderValue]::new("Basic",
        [System.Convert]::ToBase64String(
            [System.Text.Encoding]::UTF8.GetBytes([string]::Format("{0}:{1}", $userName, $password))));

#Make a Request to the Users List Endpoint 
$UsersListResponse = $client.GetAsync('/Users/List').Result

if($UsersListResponse.IsSuccessStatusCode)
{
    $userList = ReadResponse $UsersListResponse
    $userList
}
else
{
    #Read the Error Code.
    $errorMessage = ReadError $UsersListResponse
    $errorMessage
} 

#Make a Request to the Users Get Endpoint
$UsersGetResponse = $client.GetAsync('/Users/Get?&ID='+$userID).Result
if($UsersGetResponse.IsSuccessStatusCode)
{
    $getResponse = ReadResponse $UsersGetResponse
    $UsersGet = $getResponse[0]

    #Change a property on the User Object.
    $UsersGet.Active = $true

    #IF one was is setting the User from Not Active to Active, Check to verify the User has their password already set or else the Update Endpoint can fail.
    $hasPasswordResponse = $client.GetAsync('/Users/HasPassword?&userID='+$userID).Result
    if($hasPasswordResponse.IsSuccessStatusCode)
    {
        $hasPasswordContent = ReadResponse $hasPasswordResponse
        $hasPassword = $hasPasswordContent[0]

        if($hasPassword -eq $true)
        {
            #Need to check to see if any properties are null or string.empty.If they are null then remove the property.
            #Reason is cause Powershell has a weird bug where $null is treated as string.empty and then the Conversion to json will incorrect set as empty string rather than null.

            $UsersGet.PSObject.Properties |
		    Where-Object { -not $_.value } |
		    ForEach-Object {
			    $UsersGet.psobject.Properties.Remove($_.name)
		    }
            
            #Convert the object to json
            $json = $UsersGet | ConvertTo-Json
            #Set the json as an Array and then set the httpContent.
            $httpContent = [System.Net.Http.StringContent]::new('['+ $json + ']', [System.Text.Encoding]::UTF8, "application/json")
            #Send the httpContent to the Update Endpoint.
            $UpdateResponse = $client.PutAsync('/Users/Update', $httpContent).Result

            if($UpdateResponse.IsSuccessStatusCode){ Write-Host "User Updated" }
            else
            { 
                $errorMessage = ReadError $UpdateResponse
                $errorMessage
            }
        }

    }
}
else
{
    #Read the Error Code.
    $errorMessage = ReadError $UsersGetResponse
    $errorMessage
} 

#Make a Request to the Users Delete Endpoint. 
$UsersDeleteResponse = $client.DeleteAsync('/Users/Delete?&ID=' + $userID).Result
if($UsersDeleteResponse.IsSuccessStatusCode)
{
    #If the Response was Status Code 200 then the Delete was successfull
    Write-Host "Delete Success"
}
else
{
    #Read the Error Code.
    $errorMessage = ReadError $UsersDeleteResponse
    $errorMessage
} 