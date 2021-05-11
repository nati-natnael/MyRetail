# MyRetail RESTful Service
MyRetail is a RESTful API for retrieving product information.

## Endpoints
This API is hosted and can be accessed by the links below:
| | |
|-|-|
|__Get Product by ID__| https://my-retail-service.azurewebsites.net/product/{id} |
|__Update Product Price by ID__| https://my-retail-service.azurewebsites.net/product/{id}/price/{price} |
|__Swagger UI__| https://my-retail-service.azurewebsites.net/swagger |

## Setup and Run MyRetail RESTful Service Locally
You can also run this system locally. Please follow the following steps for local API setup.

### System Requirements
* __Git__<br>
    Required for pulling source code from Github.

* __.NET 5.0__<br>
    This is required for running the MyRetail service app locally. Follow the link below to download the appropriate library for your operating system.<br>
    > Download and install: https://dotnet.microsoft.com/download

* __MongoDB__<br>
    Pricing information of products is stored in the Mongo DB collection called `Products`. Please follow the instruction to install the Mongo DB server and run it locally.
    > https://docs.mongodb.com/guides/server/install/

* __MongoDB Shell__<br>
    required for interacting with DB from the command line.
    > Download MongDB Shell: https://www.mongodb.com/try/download/shell

### Setup Local MyRetailDB
Pre-requisites:
* `MongoDB Server` must be installed and is running locally.
* `MongoDB Shell` must be installed.

There is a JS script in the `~/MyRetail/db` directory called `InitMyRetailMongoDB.js`. This script is designed to seed the database with product test data.<br>

---
**CAUTION** - `InitMyRetailMongoDB.js` will drop MyRetailDB when run.

---

The script contains connection names and credentials. These lines need to be updated to reflect the local database values. These values must match `DBConnectionStrings` in appsettings.json file used by MyRetail app to connect to MongoDB.
```
const dbName = "MyRetailDB"; 
const dbAddress = "cluster0.88sdm.mongodb.net";
const dbUsername = "MyRetail";
const dbPassword = "myretail";
const collectionName = "Products";
```

Run code below to execute the script:
```
> cd ~/MyRetail
> mongosh --nodb db/InitMyRetailMongoDB.js
```

### Setup MyRetail API
Pre-requisites:
* `.NET 5.0` must be installed.
* MyRetail repository must cloned.
* Target MongoDB must be running

1. Clone MyRetail Repository

    ```
    > git clone https://github.com/nati-natnael/MyRetail.git
    ```
2. Go to MyRetail Directory

    ```
    > cd MyRetail
    ```
3. Run MyRetail API

    ```
    > dotnet run src/MyRetail.csproj
    ```
4. Run MyRetail tests

    ```
    > dotnet run tests/MyRetail.UnitTest/MyRetail.UnitTest.csproj
    ```

## MyRetail API Examples (From Hosted APP)
The following are examples of MyRetail app retrieving product information and updating pricing.

### Get Product Info by ID
* https://my-retail-service.azurewebsites.net/product/13860428
* https://my-retail-service.azurewebsites.net/product/54456119

### Update Product Price by ID
Since this is a `PUT` request, it's best done from the swagger page or applications like `Postman`.

---
**NOTE** - MyRetail is hosted on cold start function. The first request will be slow to respond.

---

Update Price From Swagger
1. Navigate to the following link: https://my-retail-service.azurewebsites.net/swagger
2. From the Product dropdown click on `/product/{id}/price/{price}`
3. Click `Try it out`
4. Enter `13860428` in the `id` field
5. Entry `10.00` in the `price` field
6. Click the `Execute` button

The above steps will send a `PUT` request to MyRetail API in the following form:
```
PUT /product/13860428/price/10.00 HTTP/1.1
...
```
## Run Tests Locally
MyRetail is cover by unit and acceptance tests. 

---
**NOTE** - MyRetail acceptance tests use hosted version of the app.

---

Follow these steps to run all unit tests:

1. Navigate to `~/MyRetail/src/`.

    ```
    > cd ~/MyRetail/src/
    ```
2. Run the following command

    ```
    > dotnet test
    ```