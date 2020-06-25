using Cassandra;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;

/**
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specifi c language governing permissions and
 * limitations under the License.
 *
 * <p>
 * DataStax Quickstart C# with Netflix
 * <p>
 * Utilizes the Netflix dataset containing data from all Netflix
 * titles and movies as of 2019.
 * <p>
 * Source:https://www.kaggle.com/shivamb/netflix-shows
 * License:Creative Commons:Public Domain
 */
namespace Netflix
{
    class Program
    {
        private static String TABLE_NETFLIX_PRIMARY = "netflix_master";
        private static String TABLE_NETFLIX_TITLES_BY_DATE = "netflix_titles_by_date";
        private static String TABLE_NETFLIX_TITLES_BY_RATING = "netflix_titles_by_rating";

        private static String TITLE_PULP_FICTION = "Pulp Fiction";
        private static String TITLE_LIFE_OF_JIMMY = "Life of Jimmy";
        private static int SHOW_ID_LIFE_OF_JIMMY = 100000000;
        private static int SHOW_ID_PULP_FICTION = 100000001;

        private static String KEYSPACE_NAME = "demo";

        static void Main(string[] args)
        {
            // 1. Define cluster with keyspace and connect to keyspace
            Cluster cluster = Cluster.Builder().WithDefaultKeyspace(KEYSPACE_NAME).AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();

            // 2. Create the foundation Netflix tables.
            CreatePrimaryTable(session);
            CreateTitlesByDateTable(session);
            CreateTitlesByRatingTable(session);

            // 3. Write Netflix data into the three newly created tables.
            // Inserting two records into each.
            InsertPrimary(session);
            InsertTitlesByDate(session);
            InsertTitlesByRating(session);

            // 4. Read from the new Netflix tables.
            Print(ReadAll(session, TABLE_NETFLIX_PRIMARY));
            Print(ReadAll(session, TABLE_NETFLIX_TITLES_BY_DATE));
            Print(ReadAll(session, TABLE_NETFLIX_TITLES_BY_RATING));

            // 5. Read all from primary table and read using Pulp Fiction title
            Print(ReadAllInPrimaryByTitle(session, TITLE_PULP_FICTION));
            Print(ReadDirectorInPrimaryByTitle(session, TITLE_PULP_FICTION));

            // 6. Update the Pulp Fiction movie to have the director's full name.
            // Then, show the updated record.
            var updatedDirectorList = new List<String>() { "Quentin Jerome Tarantino" };

            UpdateDirectorInPrimary(session,
                    SHOW_ID_PULP_FICTION, TITLE_PULP_FICTION, updatedDirectorList);

            Print(ReadDirectorInPrimaryByTitle(session, TITLE_PULP_FICTION));

            // 7. Cleanup the cluster
            cluster.Dispose();
        }

        private static void CreatePrimaryTable(ISession session)
        {
            Console.WriteLine("Creating Primary Table");
            String query = String.Format("CREATE TABLE IF NOT EXISTS {0}.{1} (show_id int, cast list<text>, country list<text>, " +
                "date_added date, description text, director list< text >," +
                "duration text, listed_in list<text>, rating text, release_year int, title text, type text, " +
                "PRIMARY KEY((title), show_id))", KEYSPACE_NAME, TABLE_NETFLIX_PRIMARY);
            session.Execute(query);

        }

        private static void CreateTitlesByDateTable(ISession session)
        {
            Console.WriteLine("Creating Titles By Rating Table");
            String query = String.Format("CREATE TABLE IF NOT EXISTS {0}.{1} (show_id int," +
                " date_added date, release_year int, title text, PRIMARY KEY ((release_year), date_added, show_id)) " +
                " WITH CLUSTERING ORDER BY (date_added DESC)", KEYSPACE_NAME, TABLE_NETFLIX_TITLES_BY_DATE);
            session.Execute(query);

        }

        private static void CreateTitlesByRatingTable(ISession session)
        {
            Console.WriteLine("Creating Titles By Rating Table");
            String query = String.Format("CREATE TABLE IF NOT EXISTS {0}.{1} (show_id int, rating text, title text," +
                    " PRIMARY KEY ((rating), show_id))", KEYSPACE_NAME, TABLE_NETFLIX_TITLES_BY_RATING);
            session.Execute(query);
        }


        private static void InsertPrimary(ISession session)
        {

            String query = String.Format("INSERT INTO {0}.{1}", KEYSPACE_NAME, TABLE_NETFLIX_PRIMARY) +
                    " (title, show_id, cast, country, date_added, " +
                    "description, director, duration, listed_in, rating, release_year, type) " +
                    "VALUES (?,?,?,?,?,?,?,?,?,?,?,?)";

            Console.WriteLine("Primary insert query: {0}", query);
            Console.WriteLine("Inserting into Primary Table for '{0}' ", TITLE_LIFE_OF_JIMMY);
            var listedInLifeOfJimmy = new List<String>() { "Action" };
            var countriesLifeOfJimmy = new List<String>() { "United States" };
            var castLifeOfJimmy = new List<String>() { "Jimmy" };
            var directorsLifeOfJimmy = new List<String>() { "Franky J" };

            var preparedStatmentLifeOfJimmy = session.Prepare(query);
            var statementLifeOfJimmy = preparedStatmentLifeOfJimmy.Bind(
                TITLE_LIFE_OF_JIMMY, SHOW_ID_LIFE_OF_JIMMY, castLifeOfJimmy, countriesLifeOfJimmy,
                            new LocalDate(2020, 6, 1), "Experiences of a guitar playing DataStax developer", directorsLifeOfJimmy,
                            "42 min", listedInLifeOfJimmy, "TV-18", 2020, "Movie");
            session.Execute(statementLifeOfJimmy);

            Console.WriteLine("Inserting into Primary Table for '{0}' ", TITLE_PULP_FICTION);
            var listedInPulpFiction = new List<String>() { "Classic Movies", "Cult Movies", "Dramas" };
            var countriesPulpFiction = new List<String>() { "United States" };
            var castPulpFiction = new List<String>() {"John Travolta", "Samuel L. Jackson", "Uma Thurman", "Harvey Keitel", "Tim Roth",
                            "Amanda Plummer", "Maria de Medeiros", "Ving Rhames", "Eric Stoltz", "Rosanna Arquette", "Christopher Walken",
                            "Bruce Willis"};

            var directorsPulpFiction = new List<String>();
            directorsPulpFiction.Add("Quentin Tarantino");

            var preparedStatmentPulpFiction = session.Prepare(query);
            var statementLifeOfPulpFiction = preparedStatmentPulpFiction.Bind(
                TITLE_PULP_FICTION, SHOW_ID_PULP_FICTION, castPulpFiction, countriesPulpFiction, new LocalDate(2019, 1, 19),
                            "This stylized crime caper weaves together stories ...", directorsPulpFiction,
                            "154 min", listedInPulpFiction, "R", 1994, "Movie");
            session.Execute(statementLifeOfPulpFiction);

        }

        private static void InsertTitlesByDate(ISession session)
        {

            String query = String.Format("INSERT INTO {0}.{1} (title, show_id, release_year, date_added) VALUES (?,?,?,?)", KEYSPACE_NAME, TABLE_NETFLIX_TITLES_BY_DATE);
            Console.WriteLine("TitlesByDate insert query: {0}", query);

            Console.WriteLine("Inserting into TitlesByDate Table for '{0}' ", TITLE_LIFE_OF_JIMMY);
            var preparedStatmentLifeOfJimmy = session.Prepare(query);
            var statementLifeOfJimmy = preparedStatmentLifeOfJimmy.Bind(TITLE_LIFE_OF_JIMMY, SHOW_ID_LIFE_OF_JIMMY, 2020, new LocalDate(2020, 6, 1));
            session.Execute(statementLifeOfJimmy);


            Console.WriteLine("Inserting into TitlesByDate Table for '{0}' ", TITLE_PULP_FICTION);
            var preparedStatmentPulpFiction = session.Prepare(query);
            var statementLifeOfPulpFiction = preparedStatmentPulpFiction.Bind(TITLE_PULP_FICTION, SHOW_ID_PULP_FICTION, 1994, new LocalDate(2019, 1, 19));
            session.Execute(statementLifeOfPulpFiction);

        }

        private static void InsertTitlesByRating(ISession session)
        {
            String query = String.Format("INSERT INTO {0}.{1} (title, show_id, rating) VALUES (?,?,?)", KEYSPACE_NAME, TABLE_NETFLIX_TITLES_BY_RATING);
            Console.WriteLine("TitlesByRating insert query: {0}", query);

            Console.WriteLine("Inserting into TitlesByRating Table for '{0}' ", TITLE_LIFE_OF_JIMMY);
            var preparedStatmentLifeOfJimmy = session.Prepare(query);
            var statementLifeOfJimmy = preparedStatmentLifeOfJimmy.Bind(TITLE_LIFE_OF_JIMMY, SHOW_ID_LIFE_OF_JIMMY, "TV-18");
            session.Execute(statementLifeOfJimmy);

            Console.WriteLine("Inserting into TitlesByRating Table for '{0}' ", TITLE_PULP_FICTION);
            var preparedStatmentPulpFiction = session.Prepare(query);
            var statementPulpFiction = preparedStatmentPulpFiction.Bind(TITLE_PULP_FICTION, SHOW_ID_PULP_FICTION, "R");
            session.Execute(statementPulpFiction);
        }

        private static RowSet ReadAll(ISession session, String tableName)
        {
            Console.WriteLine("ReadAll From: {0}", tableName);
            var query = String.Format("SELECT * FROM {0}.{1}", KEYSPACE_NAME, tableName);
            return session.Execute(query);
        }

        private static RowSet ReadAllInPrimaryByTitle(ISession session, String title)
        {
            Console.WriteLine("ReadAll from Primary, Filtering by Title: '{0}'", title);
            var query = String.Format("SELECT * FROM {0}.{1} WHERE title = ?", KEYSPACE_NAME, TABLE_NETFLIX_PRIMARY);
            var preparedStatment = session.Prepare(query);
            var statement = preparedStatment.Bind(title);
            return session.Execute(statement);
        }

        private static RowSet ReadDirectorInPrimaryByTitle(ISession session, String title)
        {
            var query = String.Format("SELECT director FROM {0}.{1} WHERE title = ?", KEYSPACE_NAME, TABLE_NETFLIX_PRIMARY);
            var preparedStatment = session.Prepare(query);
            var statement = preparedStatment.Bind(title);
            return session.Execute(statement);
        }

        private static void UpdateDirectorInPrimary(ISession session, Int32 showId,
                                                   String title,
                                                   List<String> directors)
        {
            Console.WriteLine("Update of Director in Primary by Show Id: {0} and Title: '{1}'", showId, title);
            var query = String.Format("UPDATE {0}.{1} SET director = ? WHERE show_id = ? and title = ? ", KEYSPACE_NAME, TABLE_NETFLIX_PRIMARY);
            var preparedStatment = session.Prepare(query);
            var statement = preparedStatment.Bind(directors, showId, title);
            session.Execute(statement);
        }

        private static void Print(RowSet rowSet)
        {
            Console.WriteLine();
            foreach (var row in rowSet)
            {
                StringBuilder rb = new StringBuilder("RECORD: ");
                foreach (var col in row)
                {
                    if (col.GetType().IsArray)
                    {
                        rb.AppendFormat(" [ {0} ]", string.Join(", ", col as String[]));
                    }
                    else
                    {
                        rb.Append(col);
                    }
                    rb.Append(", ");
                }
                rb.Remove(rb.Length - 2, 1).AppendLine();
                Console.WriteLine(rb.ToString());
            }
            Console.WriteLine("-----");
        }
    }
}
