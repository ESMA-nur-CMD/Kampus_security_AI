using KampusGuvenlikAI.Core.Models;
using KampusGuvenlikAI.Core.Services;
using Microsoft.Data.Sqlite;

namespace KampusGuvenlikAI.Core.Data;

public class IncidentReportRepository
{
    private readonly string _databasePath;
    private readonly string _connectionString;

    public IncidentReportRepository(string databasePath)
    {
        _databasePath = databasePath;
        _connectionString = $"Data Source={databasePath}";
    }

    public void InitializeDatabase()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_databasePath)!);

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                """
                CREATE TABLE IF NOT EXISTS IncidentReports (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreatedAt TEXT NOT NULL,
                    ReportText TEXT NOT NULL,
                    HumanLabel TEXT NOT NULL,
                    AiCategory TEXT NOT NULL,
                    Urgency TEXT NOT NULL,
                    Step1 TEXT NOT NULL,
                    Step2 TEXT NOT NULL,
                    Step3 TEXT NOT NULL
                );
                """;
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            throw;
        }
    }

    public List<IncidentReport> GetAll()
    {
        var reports = new List<IncidentReport>();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM IncidentReports ORDER BY datetime(CreatedAt) DESC;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                reports.Add(Map(reader));
            }
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            throw;
        }
        finally
        {
            Logger.LogMessage("Rapor listeleme islemi tamamlandi.");
        }

        return reports;
    }

    public void Add(IncidentReport report)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                """
                INSERT INTO IncidentReports
                (CreatedAt, ReportText, HumanLabel, AiCategory, Urgency, Step1, Step2, Step3)
                VALUES
                ($CreatedAt, $ReportText, $HumanLabel, $AiCategory, $Urgency, $Step1, $Step2, $Step3);
                """;
            AddParameters(command, report);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            throw;
        }
        finally
        {
            Logger.LogMessage("Rapor ekleme islemi tamamlandi.");
        }
    }

    public void Update(IncidentReport report)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                """
                UPDATE IncidentReports SET
                    CreatedAt = $CreatedAt,
                    ReportText = $ReportText,
                    HumanLabel = $HumanLabel,
                    AiCategory = $AiCategory,
                    Urgency = $Urgency,
                    Step1 = $Step1,
                    Step2 = $Step2,
                    Step3 = $Step3
                WHERE Id = $Id;
                """;
            command.Parameters.AddWithValue("$Id", report.Id);
            AddParameters(command, report);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            throw;
        }
        finally
        {
            Logger.LogMessage("Rapor guncelleme islemi tamamlandi.");
        }
    }

    public void Delete(int id)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM IncidentReports WHERE Id = $Id;";
            command.Parameters.AddWithValue("$Id", id);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            throw;
        }
        finally
        {
            Logger.LogMessage("Rapor silme islemi tamamlandi.");
        }
    }

    private static void AddParameters(SqliteCommand command, IncidentReport report)
    {
        command.Parameters.AddWithValue("$CreatedAt", report.CreatedAt.ToString("O"));
        command.Parameters.AddWithValue("$ReportText", report.ReportText);
        command.Parameters.AddWithValue("$HumanLabel", report.HumanLabel);
        command.Parameters.AddWithValue("$AiCategory", report.AiCategory);
        command.Parameters.AddWithValue("$Urgency", report.Urgency);
        command.Parameters.AddWithValue("$Step1", report.Step1);
        command.Parameters.AddWithValue("$Step2", report.Step2);
        command.Parameters.AddWithValue("$Step3", report.Step3);
    }

    private static IncidentReport Map(SqliteDataReader reader)
    {
        return new IncidentReport
        {
            Id = Convert.ToInt32(reader["Id"]),
            CreatedAt = DateTime.Parse(Convert.ToString(reader["CreatedAt"])!),
            ReportText = Convert.ToString(reader["ReportText"]) ?? string.Empty,
            HumanLabel = Convert.ToString(reader["HumanLabel"]) ?? string.Empty,
            AiCategory = Convert.ToString(reader["AiCategory"]) ?? string.Empty,
            Urgency = Convert.ToString(reader["Urgency"]) ?? string.Empty,
            Step1 = Convert.ToString(reader["Step1"]) ?? string.Empty,
            Step2 = Convert.ToString(reader["Step2"]) ?? string.Empty,
            Step3 = Convert.ToString(reader["Step3"]) ?? string.Empty
        };
    }
}
