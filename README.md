# strategy-and-polymorphic-pattern-sample
A sample of **Strategy Pattern** along with **Polymorphic Pattern** applied to MongoDB a NoSQL database.

The app domain is called JobAllocation, where a job applicant formulary is received to determine the proper allocation type.
The allocation type will be defined by the application of a policy, or company preference, when a policy cannot be applied.

## First steps

In order to run the application locally you can use `docker-compose` from [Docker](https://www.docker.com/).
> Important: If you are using Windows OS we recommend to install Docker via WSL!

**Steps:**

1. With Docker installed just run:
    ```shell
    # Start the app dependencies
    docker compose up -d
    ```
2. Then connect to the MongoDB database via browser on `localhost:8081` or via any Mongo client (Ex: Studio3T, etc) on `localhost:27017`.
3. Once connected to MongoDB, run the script below to insert data for the app database
    <details>
        <summary>Script for MongoDB insert</summary>

           // Use the file PopulateDbScript.js

    </details>
4. Now run the app via your IDE ou via CLI using: `dotnet run --project src/JobAllocation.HttpService/JobAllocation.HttpService.csproj`
5. Once the app is running access via browser `http://localhost:5058/swagger`

### Useful commands

```shell
# Start app dependencies
docker compose up -d
# Stop app dependencies
docker compose down
```

## Examples

Using the provided data we can select the Allocation Type from two ways, from applied policies or via company preference.

1. Example of request to apply a policy
```curl
curl -X 'POST' \
  'http://localhost:5058/define-allocation' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "disabilityCode": 0,
  "degreeCode": 7,
  "birthday": "1500-04-04T21:04:05.751Z",
  "companyCode": "1",
  "state": "TO",
  "salary": 500
}'
```
2. Example of request to select via company preference
```curl
curl -X 'POST' \
  'http://localhost:5058/define-allocation' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "disabilityCode": 0,
  "degreeCode": 2,
  "birthday": "1992-04-04T21:04:05.751Z",
  "companyCode": "1",
  "state": "RS",
  "salary": 500
}'
```

## References

- [Strategy Pattern](https://refactoring.guru/design-patterns/strategy)
- [Polymorphic Pattern](https://www.mongodb.com/developer/products/mongodb/polymorphic-pattern/)