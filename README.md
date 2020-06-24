# DataStax Desktop - C# Netflix example
An introduction to using the Cassandra database with well-defined steps to optimize your learning.  Using a Netflix dataset for sample data, your locally running Cassandra database will contain a minimal set of show data for you to customize and experiment with.

Contributors:
* [Jeff Banks](https://github.com/jeffbanks)

## Objectives
* Leverage DataStax driver APIs for interaction with a local running Cassandra database.
* Set up a Cassandra Query Language (CQL) session and perform operations such as creating, reading, and writing.
* Visualize how the CQL is used with builder patterns provided by the DataStax driver APIs.
* Use the Netflix show dataset as example information across three differently constructed tables.
* Observe how the partition key along with clustering keys produce an optimized experience.

## Project Layout

* [Program.cs](/Netflix/Program.cs) - C# application file demonstrating the capabilities of Cassandra with Netflix examples.
* [Netflix.csproj](/Netflix/Netflix.csproj) - Visual Studio .NET C# Project file

## How this works
To get started, read the `main()` method of the Program class and follow the steps for interacting with your own Cassandra database.
The methods invoked by the `main()` method are created to provide
more flexibility for modifications as you learn.

## Setup and running

### Prerequisites

* A running instance of [Apache Cassandra®](http://cassandra.apache.org/download/) 1.2+
* [Microsoft Visual Studio](https://visualstudio.microsoft.com/vs/) with .NET Core 2.1+
* IDE Alternatives: [Visual Studio Code](https://code.visualstudio.com/) or [Rider](https://www.jetbrains.com/rider/)

If running [DataStax Desktop](https://www.datastax.com/blog/2020/05/learn-cassandra-datastax-desktop), no prerequisites are required. The Cassandra instance is provided with the DataStax
Desktop stack as part of container provisioning.

If not using DataStax Desktop, spin up your own local instance of Cassandra exposing its address and
port to align with settings in the `main()` method of the file `Netflix/Program.cs`.

### Running
Verify your Cassandra database is running in your local container.

Run the C# `main()` method inside the Netflix Program class. View the console output for steps executed and check for the following output:

```
Creating Primary Table
Creating Titles By Rating Table
Creating Titles By Rating Table
Primary insert query: INSERT INTO demo.netflix_master (title, show_id, cast, country, date_added, description, director, duration, listed_in, rating, release_year, type) VALUES (?,?,?,?,?,?,?,?,?,?,?,?)
Inserting into Primary Table for 'Life of Jimmy'
Inserting into Primary Table for 'Pulp Fiction'
TitlesByDate insert query: INSERT INTO demo.netflix_titles_by_date (title, show_id, release_year, date_added) VALUES (?,?,?,?)
Inserting into TitlesByDate Table for 'Life of Jimmy'
Inserting into TitlesByDate Table for 'Pulp Fiction'
TitlesByRating insert query: INSERT INTO demo.netflix_titles_by_rating (title, show_id, rating) VALUES (?,?,?)
Inserting into TitlesByRating Table for 'Life of Jimmy'
Inserting into TitlesByRating Table for 'Pulp Fiction'
ReadAll From: netflix_master

RECORD: Life of Jimmy, 100000000,  [ Jimmy ],  [ United States ], 2020-06-01, Experiences of a guitar playing DataStax developer,  [ Franky J ], 42 min,  [ Action ], TV-18, 2020, Movie

RECORD: Pulp Fiction, 100000001,  [ John Travolta, Samuel L. Jackson, Uma Thurman, Harvey Keitel, Tim Roth, Amanda Plummer, Maria de Medeiros, Ving Rhames, Eric Stoltz, Rosanna Arquette, Christopher Walken, Bruce Willis ],  [ United States ], 2019-01-19, This
stylized crime caper weaves together stories ...,  [ Quentin Tarantino ], 154 min,  [ Classic Movies, Cult Movies, Dramas ], R, 1994, Movie

-----
ReadAll From: netflix_titles_by_date

RECORD: 2020, 2020-06-01, 100000000, Life of Jimmy

RECORD: 2020, 2020-06-01, 100000001, Pulp Fiction

RECORD: 1994, 2019-01-19, 100000001, Pulp Fiction

-----
ReadAll From: netflix_titles_by_rating

RECORD: TV-18, 100000000, Life of Jimmy

RECORD: R, 100000001, Pulp Fiction

-----
ReadAll from Primary, Filtering by Title: 'Pulp Fiction'

RECORD: Pulp Fiction, 100000001,  [ John Travolta, Samuel L. Jackson, Uma Thurman, Harvey Keitel, Tim Roth, Amanda Plummer, Maria de Medeiros, Ving Rhames, Eric Stoltz, Rosanna Arquette, Christopher Walken, Bruce Willis ],  [ United States ], 2019-01-19, This
stylized crime caper weaves together stories ...,  [ Quentin Tarantino ], 154 min,  [ Classic Movies, Cult Movies, Dramas ], R, 1994, Movie

-----

RECORD:  [ Quentin Tarantino ]

-----
Update of Director in Primary by Show Id: 100000001 and Title: 'Pulp Fiction'

RECORD:  [ Quentin Jerome Tarantino ]

```

## Having trouble?
Are you getting errors reported but can't figure out what to do next?  Copy your log output, document any details, and head
over to the [DataStax Community](https://community.datastax.com/spaces/131/datastax-desktop.html) to get some assistance.


## Questions or comments?
If you have any questions or want to post a feature request, visit the [Desktop space at DataStax Community](https://community.datastax.com/spaces/131/datastax-desktop.html)
