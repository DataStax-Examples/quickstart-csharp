# DataStax C# Driver for Apache Cassandra Quickstart

[![Build Status](https://travis-ci.org/beccam/quickstart-csharp.svg?branch=master)](https://travis-ci.org/beccam/quickstart-csharp)

A basic C#/.NET demo CRUD application using the DataStax C# Driver for Apache Cassandra. 
If you are having trouble, the complete code solution for `Program.cs` can be found [here](https://gist.github.com/beccam/5abab3b4072d5a0027475481f1d5075f).

Contributors: [Rebecca Mills](https://github.com/beccam)

## Objectives

* To demonstrate how to perform basic CRUD operations with the DataStax C# Driver.
* The intent is to help users get up and running quickly with the driver. 

## Project Layout

* [Program.cs](/QuickStart/Program.cs) - Skeleton C# application with space to fill in CRUD operation code
* [QuickStart.csproj](/QuickStart/QuickStart.csproj) - Visual Studio .NET C# Project file
* [users.cql](/QuickStart/users.cql) - Use this file to create the schema

## Prerequisites
  * A running instance of [Apache Cassandra®](http://cassandra.apache.org/download/) 1.2+
  * [Mircosoft Visual Studio](https://visualstudio.microsoft.com/vs/) with .NET Core 2.1+
  * IDE Alternatives: [Visual Studio Code](https://code.visualstudio.com/) or [Rider](https://www.jetbrains.com/rider/)
  
  
## Create the keyspace and table
The `users.cql` file provides the schema used for this project:

```sql
CREATE KEYSPACE demo
    WITH replication = {'class': 'SimpleStrategy', 'replication_factor': '1'};

CREATE TABLE demo.users (
    lastname text PRIMARY KEY,
    age int,
    city text,
    email text,
    firstname text);
```

## Connect to your cluster

All of our code is contained in the `Program` class. 
Note how the main method creates a session to connect to our cluster and runs the CRUD operations against it. 
Replace the default parameter in `Cluster.Builder()` with your own contact point.

```csharp
// TO DO: Fill in your own contact point
Cluster cluster = Cluster.Builder()
                         .AddContactPoint("127.0.0.1")
                         .Build();
ISession session = cluster.Connect("demo");
```

## CRUD Operations
Fill the code in the methods that will add a user, get a user, update a user and delete a user from the table with the driver.

### INSERT a user
```csharp
private static void SetUser(ISession session, String lastname, int age, String city, String email, String firstname) {
    
    //TO DO: execute SimpleStatement that inserts one user into the table
    var statement = new SimpleStatement("INSERT INTO users (lastname, age, city, email, firstname) VALUES (?,?,?,?,?)", lastname, age, city, email, firstname);

    session.Execute(statement);
}
```
### SELECT a user
```csharp
 private static void GetUser(ISession session, String lastname){

      //TO DO: execute SimpleStatement that retrieves one user from the table
      //TO DO: print firstname and age of user
      var statement = new SimpleStatement("SELECT * FROM users WHERE lastname = ?", lastname);
      
      var result = session.Execute(statement).First();
      Console.WriteLine("{0} {1}", result["firstname"], result["age"]);

}
```

### UPDATE a user's age
```csharp
private static void UpdateUser(ISession session, int age, String lastname) {

    //TO DO: execute SimpleStatement that updates the age of one user
    var statement = new SimpleStatement("UPDATE users SET age =? WHERE lastname = ?", age, lastname);

    session.Execute(statement);
}
```   

### DELETE a user
```csharp
private static void DeleteUser(ISession session, String lastname) {

    //TO DO: execute SimpleStatement that deletes one user from the table
    var statement = new SimpleStatement("DELETE FROM users WHERE lastname = ?", lastname);

    session.Execute(statement);
}
```
 ## License
Copyright 2019 Rebecca Mills

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.   



