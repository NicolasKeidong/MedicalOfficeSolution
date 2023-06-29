# Medical Office Management
Medical Office API is a basic Web API application for a medical office. It is a good start to have a better understanding on how to create a web API.
This project also includes the use of Data Transfer Objects (DTOs)[^1], the Models are:

![Models](https://github.com/NicolasKeidong/MedicalOfficeSolution/assets/122652469/f7948aaf-255e-420a-952e-7064f4e4f83d)

![api](https://github.com/NicolasKeidong/MedicalOfficeSolution/assets/122652469/eeacd0dc-8d9f-44eb-a3e1-909a788f51c8)


![Swagger](https://github.com/NicolasKeidong/MedicalOfficeSolution/assets/122652469/ef5709ad-fd6d-44ba-8687-a39e27898f19)

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
