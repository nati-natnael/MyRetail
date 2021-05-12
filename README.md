# MyRetail RESTful Service
MyRetail is a RESTful API for retrieving product information.

## Endpoints
This API is hosted and can be accessed by the links below:
| | |
|-|-|
|__Get Product by ID__| https://<span></span>my-retail-service.azurewebsites.net/product/{id} |
|__Update Product Price by ID__| https://<span></span>my-retail-service.azurewebsites.net/product/{id}/price/{price} |
|__Swagger UI__| https://<span></span>my-retail-service.azurewebsites.net/swagger |


## MyRetail API Examples (From Hosted App)
The following are examples of MyRetail app retrieving product information and updating pricing.

### Get Product Info by ID
* https://my-retail-service.azurewebsites.net/product/13860428
* https://my-retail-service.azurewebsites.net/product/54456119

### Update Product Price by ID
Since this is a `PUT` request, it's best done from the swagger page or applications like `Postman`.

*NOTE - MyRetail is hosted on cold start function. The first request will be slow to respond.*

Update Price From Swagger
1. Navigate to the following link: https://my-retail-service.azurewebsites.net/swagger
2. From the Product dropdown click on `/product/{id}/price/{price}`
3. Click `Try it out`
4. Enter `13860428` in the `id` field
5. Enter `10.00` in the `price` field
6. Click the `Execute` button

The above steps will send a `PUT` request to MyRetail API in the following form:
```
PUT /product/13860428/price/10.00 HTTP/1.1
...
```

## Run MyRetail RESTful Service Locally
You can also run this system locally. Please follow the following steps for local API setup.

### System Requirements
* __Git__<br>
    Required for pulling source code from Github.

* __.NET 5.0__<br>
    This is required for running the MyRetail service app locally. Follow the link below to download the appropriate library for your operating system.<br>
    > Download and install: https://dotnet.microsoft.com/download
    
    If you have having issues installing .NET 5.0 on Linux, try the link below:
    > Linux install: https://docs.microsoft.com/en-us/dotnet/core/install/linux-snap

* __MongoDB__<br>
    Pricing information of products is stored in the Mongo DB collection called `Products`. Please follow the instruction to install the Mongo DB server and run it locally.
    > https://docs.mongodb.com/manual/installation/

* __MongoDB Shell__<br>
    required for interacting with DB from the command line.
    > Install MongDB Shell: https://docs.mongodb.com/mongodb-shell/install/#std-label-mdb-shell-install

### Setup Local MyRetailDB
Pre-requisites:
* `MongoDB Server` must be installed and is running locally. Only required if app uses local mongoDB.
* `MongoDB Shell` must be installed.

1. Paste the lines below to file `~/MyRetail/db/InitMyRetailMongoDB.js`. If you have setup username and password, update credentials.
    ```
    const protocol = "mongodb";
    const dbAddress = "locahost:27017";
    const credentials = "USERNAME:PASSWORD@";
    ```
2. Execute `~/MyRetail/db/InitMyRetailMongoDB.js` script to populate database with test data. Run the following command.
    ```
    cd ~/MyRetail
    mongosh --nodb db/InitMyRetailMongoDB.js
    ```
3. Update `~MyRetail/src/appsettings.json` so the app can connect to the local database. The `DBConnectionStrings` must reflect new database connection, credentials, and db and collection name created by `~/MyRetail/db/InitMyRetailMongoDB.js`.
    ```
    "DBConnectionStrings": {
        "Protocol": "mongodb",
        "Address": "localhost:27017",
        "Username": "USERNAME",
        "Password": "PASSWORD",
        "Name": "MyRetailDB",
        "CollectionName": "Products"
    },
    ```

*CAUTION - `InitMyRetailMongoDB.js` will drop MyRetailDB when run.*

### Setup MyRetail API

Pre-requisites:
* `.NET 5.0` must be installed
* MongoDB  server must be running

1. Clone MyRetail Repository

    ```
    git clone https://github.com/nati-natnael/MyRetail.git
    ```
2. Go to MyRetail Directory

    ```
    cd MyRetail
    ```
3. Run MyRetail API

    ```
    dotnet run --project src/MyRetail.csproj
    ```
4. Run MyRetail Unit tests

    ```
    dotnet test tests/MyRetail.UnitTest/MyRetail.UnitTest.csproj
    ```
4. Run MyRetail Acceptance tests

    ```
    dotnet test tests/MyRetail.AcceptanceTest/MyRetail.AcceptanceTest.csproj
    ```



## Run Tests Locally
MyRetail is cover by unit and acceptance tests.

*NOTE - MyRetail acceptance tests use hosted version of the app.*

Follow these steps to run all unit tests:

1. Navigate to `~/MyRetail/src/`.

    ```
    cd ~/MyRetail/src/
    ```
2. Run the following command

    ```
    dotnet test src/
    ```
