# MyRetail RESTful Service
MyRetail is a RESTful API for retieving product infomation.

## Endpoints
This API is hosted and can be accessed by the links below:
| | | |
|-|-|-|
|__Raw__| https://my-retail-service.azurewebsites.net/ |
|__Swagger__| https://my-retail-service.azurewebsites.net/swagger |
| | | |

## Run Locally
You can also run this system locally. Please follow the following steps for local API setup.

### Requirements
* __Git__<br>
    Required for pulling source code from github.

* __.NET 5.0__<br>
    This is required for running MyRetail service app locally. Follow the link below to download the appropriate library for you operating system.<br>
    > Download and install: https://dotnet.microsoft.com/download

* __MongoDB__<br>
    Pricing information of products is stored in mongo db collection called `Products`. Please follow the instruction to install mongo db server and run it locally.
    > https://docs.mongodb.com/guides/server/install/

* __MongoDB Shell__<br>
    required for interacting with DB from the command line.
    > Download MongDB Shell - https://www.mongodb.com/try/download/shell

### Setup MyRetail RESTful API
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
    dotnet run src/MyRetail.csproj
    ```
4. Run MyRetail tests
    ```
    dotnet run tests/MyRetail.UnitTest/MyRetail.UnitTest.csproj
    ```

### Setup Local MyRetailDB
Pre-requist:
* MongoDB has been installed and is running locally.
* MongoDB Shell has been installed

There is a JS script in `~/MyRetail/db` directory called `InitMyRetailMongoDB.js`. This script is designed to seed the database with products test data.<br>

---
**CAUTION** - `InitMyRetailMongoDB.js` will drop MyRetailDB when run.

---

The script contains connection and credentials. These lines need to be updated to reflect the local database values. 
```
const dbName = "MyRetailDB"; 
const dbAddress = "cluster0.88sdm.mongodb.net";
const dbUsername = "MyRetail";
const dbPassword = "myretail";
```

Run code below to excute script:
```
> cd ~/MyRetail
> mongosh --nodb db/InitMyRetailMongoDB.js
```

## MyRetail API Examples (From Hosted APP)
The following are examples of MyRetail app retrieving product information and updating pricing.

### Get Product Info by ID
* https://my-retail-service.azurewebsites.net/product/13860428
* https://my-retail-service.azurewebsites.net/product/54456119

### Update Product Price by ID
Since this is a `PUT` request, its best done from the swagger page or applications like `Postman`.

* Update Price From Swagger
    1. Navigate to following link: https://my-retail-service.azurewebsites.net/swagger
    2. From the Product dropdown click on `/product/{id}/price/{price}`
    3. Click `Try it out`
    4. Enter `13860428` in the `id` field
    5. Entry `10.00` in the `price` field
    6. Click `Execute` button

The above steps will send a `PUT` request to MyRetail API in the following form with:
```
PUT /product/13860428/price/10.00 HTTP/1.1
...
```
    
