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

## Project Dependencies ##
Here is the list of dependencies for the project:

* Json.net
* Newtonsoft.Json
* Microsoft.EntityFrameworkCore
* Pomelo.EntityFrameworkCore.MySql
* Microsoft.EntityFrameworkCore.Design
* JetBrains.Annotations
* Microsoft.AspNetCore.Mvc.DataAnnotations