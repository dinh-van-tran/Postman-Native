using System;
using System.Collections.Generic;
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
                    "name TEXT NOT NULL, " +
                    "method VARCHAR(10) NOT NULL, " +
                    "url TEXT NOT NULL, " +
                    "body_parameter_type VARCHAR(10) NOT NULL DEFAULT 'TEXT', " +
                    "text_parameter TEXT NOT NULL DEFAULT '' " +
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

                String addRequestFormTable = "CREATE TABLE IF NOT EXISTS FORM_PARAMETER(" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "request_id INTEGER NOT NULL, " +
                    "name TEXT NOT NULL, " +
                    "value TEXT, " +
                    "FOREIGN KEY(request_id) REFERENCES REQUEST(id)" +
                    ");";
                createTable = new SqliteCommand(addRequestFormTable, db);
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

                SqliteCommand selectRequestCommand = new SqliteCommand("SELECT id, name, method, url, body_parameter_type, text_parameter FROM REQUEST;", db);

                SqliteDataReader query = selectRequestCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    string name = query.GetString(1);
                    string method = query.GetString(2);
                    string url = query.GetString(3);
                    string bodyParameterType = query.GetString(4);
                    string textParameter = query.GetString(5);

                    var request = new Request(id, name, method, url, bodyParameterType, textParameter);
                    request.QueryParameters = getQueryParameters(id, db);
                    request.FormParameters = getFormParameters(id, db);
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

                SqliteCommand selectRequestCommand = new SqliteCommand("SELECT name, method, url, body_parameter_type, text_parameter FROM REQUEST WHERE id = " + id + ";", db);

                SqliteDataReader query = selectRequestCommand.ExecuteReader();

                if (query.Read())
                {
                    string name = query.GetString(0);
                    string method = query.GetString(1);
                    string url = query.GetString(2);
                    string bodyParameterType = query.GetString(3);
                    string textParameter = query.GetString(4);

                    request.QueryParameters = getQueryParameters(id, db);
                    request.FormParameters = getFormParameters(id, db);
                    request.Headers = getHeaders(id, db);

                    request = new Request(id, name, method, url);
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
                command.CommandText = "INSERT INTO REQUEST(name, method, url, body_parameter_type, text_parameter) VALUES(@name, @method, @url, @bodyParameter, @textParameter);";

                command.Parameters.AddWithValue("@name", request.Name);
                command.Parameters.AddWithValue("@method", request.Method);
                command.Parameters.AddWithValue("@url", request.Url);
                command.Parameters.AddWithValue("@bodyParameter", request.BodyParameterType);
                command.Parameters.AddWithValue("@textParameter", request.TextParameter);

                command.ExecuteReader();
                newId = getLastRowId(db);

                insertHeaders(request, newId, db);
                insertQueryParameters(request, newId, db);
                insertFormParameters(request, newId, db);

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
                command.CommandText = "UPDATE REQUEST SET method = @method, url = @url, body_parameter_type = @bodyParameter, text_parameter = @textParameter WHERE id = @id;";

                command.Parameters.AddWithValue("@id", request.Id);
                command.Parameters.AddWithValue("@name", request.Name);
                command.Parameters.AddWithValue("@method", request.Method);
                command.Parameters.AddWithValue("@url", request.Url);
                command.Parameters.AddWithValue("@bodyParameter", request.BodyParameterType);
                command.Parameters.AddWithValue("@textParameter", request.TextParameter);

                command.ExecuteReader();

                deleteHeaders(request.Id, db);
                insertHeaders(request, request.Id, db);

                deleteQueryParameters(request.Id, db);
                insertQueryParameters(request, request.Id, db);

                deleteFormParameters(request.Id, db);
                insertFormParameters(request, request.Id, db);

                db.Close();
            }

            return request.Id;
        }


        private static void insertQueryParameters(Request request, int requestId, SqliteConnection db)
        {
            insertParameters("QUERY_PARAMETER", requestId, request.QueryParameters, db);
        }

        private static void insertFormParameters(Request request, int requestId, SqliteConnection db)
        {
            insertParameters("FORM_PARAMETER", requestId, request.FormParameters, db);
        }

        private static void insertHeaders(Request request, int requestId, SqliteConnection db)
        {

            insertParameters("HEADER", requestId, request.Headers, db);
        }

        private static List<Parameter> getQueryParameters(int requestId, SqliteConnection db)
        {
            return getParameters("QUERY_PARAMETER", requestId, db);
        }

        private static List<Parameter> getFormParameters(int requestId, SqliteConnection db)
        {
            return getParameters("FORM_PARAMETER", requestId, db);
        }

        private static List<Parameter> getHeaders(int requestId, SqliteConnection db)
        {
            return getParameters("HEADER", requestId, db);
        }

        private static List<Parameter> getParameters(string tableName, int requestId, SqliteConnection db)
        {
            string queryString = string.Format("SELECT id, name, value FROM {0} WHERE request_id = {1};", tableName, requestId);
            SqliteCommand selectRequestCommand = new SqliteCommand(queryString, db);
            List<Parameter> parameterList = new List<Parameter>();

            SqliteDataReader query = selectRequestCommand.ExecuteReader();

            while (query.Read())
            {
                int id = query.GetInt32(0);
                string name = query.GetString(1);
                string value = query.GetString(2);
                parameterList.Add(new Parameter(id, name, value));
            }

            return parameterList;
        }

        private static void insertParameters(string tableName, int requestId, List<Parameter> parameterList, SqliteConnection db)
        {
            if (parameterList == null || parameterList.Count == 0)
            {
                return;
            }

            string queryString = string.Format("INSERT INTO {0}(request_id, name, value) VALUES (@requestId, @name, @value)", tableName);
            foreach (var parameter in parameterList)
            {
                if (parameter.Name == null || parameter.Name.Trim().Length == 0)
                {
                    continue;
                }

                SqliteCommand addCommand = new SqliteCommand(queryString, db);

                addCommand.Parameters.AddWithValue("@requestId", requestId);
                addCommand.Parameters.AddWithValue("@name", parameter.Name);
                addCommand.Parameters.AddWithValue("@value", parameter.Value);

                addCommand.ExecuteScalar();
            }
        }

        private static void deleteHeaders(int requestId, SqliteConnection db)
        {
            deleteParameters("HEADER", requestId, db);
        }

        private static void deleteQueryParameters(int requestId, SqliteConnection db)
        {
            deleteParameters("QUERY_PARAMETER", requestId, db);
        }

        private static void deleteFormParameters(int requestId, SqliteConnection db)
        {
            deleteParameters("FORM_PARAMETER", requestId, db);
        }

        private static void deleteParameters(string tableName, int requestId, SqliteConnection db)
        {
            string queryString = string.Format("DELETE FROM {0} WHERE request_id = {1}", tableName, requestId);
            SqliteCommand deleteCommand = new SqliteCommand(queryString, db);
            deleteCommand.ExecuteScalar();
        }

        public static int getLastRowId(SqliteConnection db)
        {
            SqliteCommand command = new SqliteCommand("select last_insert_rowid();", db);
            Int64 LastRowID64 = (Int64)command.ExecuteScalar();
            return (int)LastRowID64;
        }
    }
}
