# README #

## Running on Command Line ##
Prior to running the project, a few environment variables must be set: 
*DK_API_USERNAME, DK_API_PASSWORD, DK_API_CERT_PATH,* and *DK_API_CERT_PASS*

To keep the information private, it is recommended to set these variables using a script stored in the `private` directory, which is ignored by git. 

Here's an example of such a script:
```
export DK_API_USERNAME="username"
export DK_API_PASSWORD="password"
export DK_API_CERT_PASS="certificate_password"
export DK_API_CERT_PATH="relative_path_to_certificate"
```
Then one could run the following to set the environment variables:
```
chmod +x ./private/my_script
. ./private/my_script
```

The DK_API_CERT_PATH should specify a path to the DirectKey API certificate used to access the API. The path should be either absolute or specified relative to the root project directory, from which the project will be run.

In a Windows environment, the environment variables would be set using the operating system's built-in environment variables user interface.

To run the project, execute the command `dotnet run` in the root directory.

## App Settings ##

The settings for the project can be found in `appsettings.json`. Note that `appsettings.json` is ignored by git, while `appsettings.Development.json` is not. It is important to specify the name of your SQL database in this file when testing locally.

The main setting relevant for changing is `ApplicationDbContext`, a semicolon-separated string of parameter names and values. Here is an example of what the json file may look like: 

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "ApplicationDbContext": "server=localhost;database=directkeydashboard;"
  },
  "AllowedHosts": "*"
}
```

Lastly, the file InformationLibraries/DKApiClient specifies the API base address as a constant (```const string ApiAddress = "https://dkintapi.keytest.net/api/ver6/";```). For development purposes, it points to the DKInt API (testing API). For production, it should be changed to the correct API endpoint.

## Project Dependencies ##
Here is the list of dependencies for the project (make sure to install the relevant Nuget packages):

* Json.net
* Newtonsoft.Json
* Microsoft.EntityFrameworkCore
* Pomelo.EntityFrameworkCore.MySql
* Microsoft.EntityFrameworkCore.Design
* JetBrains.Annotations
* Microsoft.AspNetCore.Mvc.DataAnnotations
