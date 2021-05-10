const dbName = "MyRetailDB";
const dbAddress = "cluster0.88sdm.mongodb.net";
const dbUsername = "MyRetail";
const dbPassword = "myretail";

print("Connecting to db ... ", dbName);
const db = connect(`mongodb+srv://${dbUsername}:${dbPassword}@${dbAddress}/${dbName}`);
print("Successfully connected to ", db);

print(`Dropping DB ${dbName} ...`);
db.Products.drop();
print(`Successfully dropped DB ${dbName}`);

print("Inserting to collection 'Products' ...");
const docArray = [
    {
        "id": 13860428,
        // "name": "The Big Lebowski (Blu-ray) (Widescreen)",
        "current_price": {
            "value": 14.99,
            "currency_code": "USD"
        }
    },
    {
        "id": 54456119,
        // "name": "Creamy Peanut Butter 40oz - Good &#38; Gather&#8482;",
        "current_price": {
            "value": 5.99,
            "currency_code": "USD"
        }
    },
    {
        "id": 13264003,
        // "name": "Jif Natural Creamy Peanut Butter - 40oz",
        "current_price": {
            "value": 5.99,
            "currency_code": "USD"
        }
    },
    {
        "id": 12954218,
        // "name": "Kraft Macaroni &#38; Cheese Dinner Original - 7.25oz",
        "current_price": {
            "value": 0.99,
            "currency_code": "USD"
        }
    }
    // *** Add another Document here ***
];

db.Products.insertMany(docArray);
print("Successfully inserted to collection 'Products'");

print("Verify document insertion to collection 'Products' ...");
for (const doc of docArray) {
    const dbDoc = db.Products.find({ "id": doc["id"] });
    if (dbDoc){
        print("\tDocument ID - ", doc["id"], " found");
    } else {
        print("\tDocument ID - ", doc["id"], " not found");
        print("Verification failed");
        quit();
    }
}
print("Verification successfully completed");