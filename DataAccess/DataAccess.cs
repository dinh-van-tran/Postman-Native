using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        public static void InitializeDatabase()
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=postman.db"))
            {
                db.Open();

                String addRequestTable = "CREATE TABLE IF NOT EXISTS REQUEST(" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "method VARCHAR(10) NOT NULL, " +
                    "url TEXT NOT NULL" +
                    ");";

                SqliteCommand createTable = new SqliteCommand(addRequestTable, db);
                createTable.ExecuteReader();

                String addRequestQueryTable = "CREATE TABLE IF NOT EXISTS QUERY_PARAMETER(" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "request_id INTEGER NOT NULL, " +
                    "name TEXT NOT NULL, " +
                    "value TEXT, " +
                    "FOREIGN KEY(request_id) REFERENCES REQUEST(id)" +
                    ");";
                createTable = new SqliteCommand(addRequestQueryTable, db);
                createTable.ExecuteReader();

                String addRequestHeaderTable = "CREATE TABLE IF NOT EXISTS HEADER(" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "request_id INTEGER NOT NULL, " +
                    "name TEXT NOT NULL, " +
                    "value TEXT, " +
                    "FOREIGN KEY(request_id) REFERENCES REQUEST(id) ON UPDATE CASCADE ON DELETE CASCADE" +
                    ");";
                createTable = new SqliteCommand(addRequestHeaderTable, db);
                createTable.ExecuteReader();

                db.Close();
            }
        }

        public static List<Request> getAllRequest()
        {
            List<Request> requests = new List<Request>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=postman.db"))
            {
                db.Open();

                SqliteCommand selectRequestCommand = new SqliteCommand("SELECT id, method, url FROM REQUEST;", db);

                SqliteDataReader query = selectRequestCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    string method = query.GetString(1);
                    string url = query.GetString(2);

                    var request = new Request(id, method, url);
                    request.QueryParameters = getQueryParameters(id, db);
                    request.Headers = getHeaders(id, db);

                    requests.Add(request);
                }

                db.Close();
            }

            return requests;
        }

        public static Request getRequestById(int id)
        {
            Request request = null;

            using (SqliteConnection db =
                new SqliteConnection("Filename=postman.db"))
            {
                db.Open();

                SqliteCommand selectRequestCommand = new SqliteCommand("SELECT id, method, url FROM REQUEST WHERE id = " + id + ";", db);

                SqliteDataReader query = selectRequestCommand.ExecuteReader();

                if (query.Read())
                {
                    string method = query.GetString(1);
                    string url = query.GetString(2);
                    request.QueryParameters = getQueryParameters(id, db);
                    request.Headers = getHeaders(id, db);

                    request = new Request(id, method, url);
                }

                db.Close();
            }

            return request;
        }

        public static int saveRequest(Request request)
        {
            if (request == null)
            {
                return -1;
            }

            if (request.Id == -1)
            {
                return insertRequest(request);
            }

            return updateRequest(request);
        }

        private static int insertRequest(Request request)
        {
            int newId = -1;
            using (SqliteConnection db =
                new SqliteConnection("Filename=postman.db"))
            {
                db.Open();

                SqliteCommand command = new SqliteCommand();
                command.Connection = db;
                command.CommandText = "INSERT INTO REQUEST(method, url) VALUES(@method, @url);";

                command.Parameters.AddWithValue("@method", request.Method);
                command.Parameters.AddWithValue("@url", request.Url);

                command.ExecuteReader();
                newId = getLastRowId(db);

                insertHeaders(request, db);
                insertQueryParametes(request, db);

                db.Close();
            }

            return newId;
        }

        private static int updateRequest(Request request)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=postman.db"))
            {
                db.Open();

                SqliteCommand command = new SqliteCommand();
                command.Connection = db;
                command.CommandText = "UPDATE REQUEST SET method = @method, url = @url where id = @id;";

                command.Parameters.AddWithValue("@id", request.Id);
                command.Parameters.AddWithValue("@method", request.Method);
                command.Parameters.AddWithValue("@url", request.Url);

                command.ExecuteReader();

                deleteHeaders(request.Id, db);
                insertHeaders(request, db);

                deleteQueryParameters(request.Id, db);
                insertQueryParametes(request, db);

                db.Close();
            }

            return request.Id;
        }

        private static void deleteQueryParameters(int requestId, SqliteConnection db)
        {
            SqliteCommand deleteCommand = new SqliteCommand("DELETE FROM QUERY_PARAMETER WHERE request_id = " + requestId + ";", db);
            deleteCommand.ExecuteScalar();
        }

        private static void insertQueryParametes(Request request, SqliteConnection db)
        {
            if (request.QueryParameters == null || request.QueryParameters.Count == 0)
            {
                return;
            }

            foreach (var parameter in request.QueryParameters)
            {
                SqliteCommand addCommand = new SqliteCommand("INSERT INTO QUERY_PARAMETER(request_id, name, value) VALUES (@requestId, @name, @value)", db);

                addCommand.Parameters.AddWithValue("@requestId", request.Id);
                addCommand.Parameters.AddWithValue("@name", parameter.Name);
                addCommand.Parameters.AddWithValue("@value", parameter.Value);

                addCommand.ExecuteScalar();
            }
        }

        private static void deleteHeaders(int requestId, SqliteConnection db)
        {
            SqliteCommand deleteCommand = new SqliteCommand("DELETE FROM HEADER WHERE request_id = " + requestId + ";", db);
            deleteCommand.ExecuteScalar();
        }

        private static void insertHeaders(Request request, SqliteConnection db)
        {
            if (request.Headers == null || request.Headers.Count == 0)
            {
                return;
            }

            foreach (var header in request.Headers)
            {
                SqliteCommand addCommand = new SqliteCommand("INSERT INTO HEADER(request_id, name, value) VALUES (@requestId, @name, @value)", db);

                addCommand.Parameters.AddWithValue("@requestId", request.Id);
                addCommand.Parameters.AddWithValue("@name", header.Name);
                addCommand.Parameters.AddWithValue("@value", header.Value);

                addCommand.ExecuteScalar();
            }
        }

        private static List<Parameter> getQueryParameters(int requestId, SqliteConnection db)
        {
            List<Parameter> parameters = new List<Parameter>();
            SqliteCommand selectRequestCommand = new SqliteCommand ("SELECT id, name, value FROM QUERY_PARAMETER WHERE request_id = " + requestId + ";", db);
            SqliteDataReader query = selectRequestCommand.ExecuteReader();

            while (query.Read())
            {
                int id = query.GetInt32(0);
                string name = query.GetString(1);
                string value = query.GetString(2);
                parameters.Add(new Parameter(id, name, value));
            }

            return parameters;
        }

        private static List<Parameter> getHeaders(int requestId, SqliteConnection db)
        {
            List<Parameter> headers = new List<Parameter>();
            SqliteCommand selectRequestCommand = new SqliteCommand("SELECT id, name, value FROM HEADER WHERE request_id = " + requestId + ";", db);
            SqliteDataReader query = selectRequestCommand.ExecuteReader();

            while (query.Read())
            {
                int id = query.GetInt32(0);
                string name = query.GetString(1);
                string value = query.GetString(2);
                headers.Add(new Parameter(id, name, value));
            }

            return headers;
        }

        public static int getLastRowId(SqliteConnection db)
        {
            SqliteCommand command = new SqliteCommand("select last_insert_rowid();", db);
            Int64 LastRowID64 = (Int64)command.ExecuteScalar();
            return (int)LastRowID64;
        }
    }
}
