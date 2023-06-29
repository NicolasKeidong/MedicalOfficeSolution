# Medical Office Management
Medical Office API is a basic Web API application for a medical office. It is a good start to have a better understanding of how to create a web API.
This project also includes the use of Data Transfer Objects (DTOs)[^1], the Models are:

![models](https://github.com/NicolasKeidong/MedicalOfficeSolution/assets/122652469/f0b78f3a-9f11-4fcb-b2c7-274016a4b65a)

![json](https://github.com/NicolasKeidong/MedicalOfficeSolution/assets/122652469/c375b910-b488-41f4-aff3-c614bf2e09c1)

![swagger](https://github.com/NicolasKeidong/MedicalOfficeSolution/assets/122652469/12d2af4e-715a-4d35-8ced-1031c2bfea19)


## Installation
- Clone this repository.
- Add your scripts.
- Have fun. :smiley:

## Usage
Go to Properties/launchSettings.json and modify launchUrl if you want to open it with Swagger, other than that you are ready to go.  

``` JS
 "profiles": {
    "MedicalOfficeWebApi": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "api/patients", // to  "launchUrl": "swagger" if you want to open it with Swagger
      ...
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "api/patients",  // to  "launchUrl": "swagger" if you want to open it with Swagger
      ...

```

## Additional notes
At the moment, I don't have future plans for this project which is why there is no guide on how to contribute to this project, but if I change my mind I will update this file, thank you for your understanding.

## References
[^1]: [Create Data Transfer Objects (DTOs).](https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5)
