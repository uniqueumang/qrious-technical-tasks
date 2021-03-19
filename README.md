# Qrious Technical Challenge

## Interview Code Review Test

### Test 1
* Name the class properly. Something like `CustomerAddressRepository`.
* Method name should be more clear i.e., GetAddressesByStatus
* Why return type is `IEnumberable<Address>`? Recommendation to be more concreate. I am assuming it is a distinct set of addresses so may be ISet & return HashSet<Address>. You will need to implement Equals and GetHashCode in Address Class if implementing Hashset.
* Can status argument be an enum rather than string? 
* Implement a constructor in `CustomerAddressRepository` such that it takes `IDBConnection`. 
* Create `IReadCustomerAddressRepository` with `GetAddressesByStatus` method in it & `CustomerAddressRepository` should interherit this. 
* Missing Logger & logging in general
* If Status cannot be enum, atleast check it is not null empty or whitespace. 
* SQL Select query should be constant string field. It should also check if CustomerAddress is null or empty. Status (if string) should also handle case insensitivity. Also paramatise to avoid SQL injection
    `SELECT CustomerAddress FROM dbo.Customer WHERE CustomerAddress <> '' AND Status LIKE @Status`
* `retreiveAddressSqlCommand` instead of cmd.  Make it clear. 
* Create a private method 'GetRawAddressesFromDatabase' which returns a `List<string>`. Role of this method to connect to DB & query the DB. Call this method from `CustomerAddressRepository`. 
* Wrap connection in using statement. Also close the connection in the using statement.Wrap it in try-finally. Something like this 
``` C#
List<string> GetRawAddressesFromDatabase(string status)
{
    try{
        using (_dbConnection)
        {
            ...
            _dbConnection.Open();
             do work ...
             _dbConnection.Close();
        }
    } finally
    {
        if (_dbConnection.State != Closed){
            _dbConnection.Close();
        }
    }
}
```
* It is not right to `throw ex`. Just do a thow or throw a new but different exception.  If you're doing just a throw than why even have try catch block? . Throw ex will cause loss of stack trace. 
* Once you have the result from Database, then you should convert the result to Address object. 
* finally make it Async. We may be doing expensive network operation.

Here is my pseudo code of the summary of above: 
``` C#
interface IReadCustomerRepository
{
    Task<ISet<Address>> GetAddressesByStatusAsync(string status);
}

class CustomerRepository : IReadCustomerRepository
{
    readonly IDBConnection _dbConnection;
    const string RetreiveAddressSqlCommand = "Select .... ";
    # Add Some Logger 

    CustomerRepository(IDBConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }


     async Task<ISet<Address>> GetAddressesByStatusAsync(string status)
     {
         if (status is null empty or whitespace)
         {
             return null; // this is subjective. I dont know what layer above is doing. Can throw Validation exception instead. 
         }
        ListWithRawAddresses = null;
         try
         {
            ListWithRawAddresses = await GetRawAddressesFromDatabaseAsync(status);
         }catch(Exception ex){
             throw new ProviderException("Failed to retreive customer address from DB", ex); //Some kind of exception throw the issue is with database and not with code
         }

         
        if (ListWithRawAddresses == null || ListWithRawAddresses.Length ==0){
            return null;
        }
    
        return ListWithRawAddresses.Select(StringToAddress).ToHashSet();
    }

    static Address StringToAddress(string addressString)
		{
            try{
			return new Address(addressString);
            }catch(Exception ex){
                # Log the input and the exception thrown
                throw
            }
		}
}
```

Notes: 
+ It is hard for me to tell how much result can return from database. Should there be pagination? 
+ This is a webapi, I am asumming DI mechanism exists. 

### Test 2
* some of the good stuff is already mentioned above related DI, SQL Injection, naming artifacts.
* Name of the method should be along the lines of `AddCustomerPurchaseToPurchaseAndRewardDb`.
* Use TransactionScope to manage the transaction across database.
* Make it async 

here is the pseudo code:
```C#
Task<Result> AddCustomerPurchaseToPurchaseAndRewardDb(Purchase purchase)
{
    using (var transactionScope =  new TransactionScope())
    {
        using (_purchaseDbConnection){

            await insertPurchaseToPurchaseDb(purchase);
            using(_rewardsConnection)
            {
                await insertPurchaseToRewardsDb(purchase);
            }
        }

        transactionScope.Complete();

        return Result.Success(); 
    }
   
   return Result.Failed();
}
```
Note: My Preference would be to use Unit Of Work Pattern but similar logic

### Test 3
* I have no clue what is class even doing. I see it is a queue of email which can also send emails in an asyncronous pattern.
* My biggest issue here is thread exhausion. There is no Join but we keep on creating new threads. General preference would be to use Task. 
* Use `Interlocked.Increment()` to increment failed and send count. Both of this need to be ulong. 

MY asuumption is this class is a consumer which has reference to all queues. Someother oblect is constantly calling `SendNextEmail` like an infinite while loop. 


## Ubiquity Technical Challenge

Answer:
12 digit Number: 123334444567 Prime Numbers: 313 563 811 863

On my computer it takes 19ms to get that answer. Spec: AMD Ryzen 5 1600 & 32Gb of Ram. 

Algorithm: 
1) Find all 12-digit numbers which follows given rules. I use binary tree to generate such number. I found 8000 such numbers. 
2) Find prime factors of given 12 digit numbers. I only considered prime factors between 100 & 999. 
