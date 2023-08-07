using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Users.Projects.Api.Data.Configuration
{
    public static class DatabaseInitializer
    {
        const string STORED_PROCEDURE_SCRIPT = @$"
                CREATE PROCEDURE usp_ClearAndPopulateData
                AS
                BEGIN
                    DELETE FROM TimeLogs;
                    DELETE FROM Users;
                    DELETE FROM Projects;

                    INSERT INTO Users (Id, FirstName, LastName, Email, DateAdded)
                    SELECT TOP 100
                        Id = NEWID(),
                        FirstName = F.FirstName,
                        LastName = L.LastName,
                        Email = CONCAT(F.FirstName, '.', L.LastName, '@', D.Domain),
                        DateAdded = GETDATE()
                    FROM 
                        (VALUES
                            ('John'), ('Gringo'), ('Mark'), ('Lisa'), ('Maria'), 
                            ('Sonya'), ('Philip'), ('Jose'), ('Lorenzo'), ('George'), ('Justin')
                        ) AS F(FirstName)
                        CROSS JOIN 
                        (VALUES ('Johnson'), ('Lamas'), ('Jackson'), ('Brown'), ('Mason'), 
                            ('Rodriguez'), ('Roberts'), ('Thomas'), ('Rose'), ('McDonalds')
                        ) AS L(LastName)
                        CROSS JOIN 
                        (VALUES ('hotmail.com'), ('gmail.com'), ('live.com')) AS D(Domain)
                    ORDER BY NEWID(); -- Random sampling

                    INSERT INTO Projects (Id, Name) VALUES (NEWID(), 'My own'), (NEWID(), 'Free Time'), (NEWID(), 'Work');

                    DECLARE @MaxEntries INT = 20;
                    DECLARE @MyCursor CURSOR;
                    DECLARE @UserId UNIQUEIDENTIFIER;
                    DECLARE @EndDate DATE = GETDATE();
                    DECLARE @StartDate DATE = DATEADD(YEAR, -1, @EndDate);
                    DECLARE @DaysBetween INT = (1+DATEDIFF(DAY, @StartDate, @EndDate));

                    BEGIN
                        SET @MyCursor = CURSOR FOR
                        select Id from users

                        OPEN @MyCursor 
                        FETCH NEXT FROM @MyCursor
	                    INTO @UserId;

                        WHILE @@FETCH_STATUS = 0
                        BEGIN
                          DECLARE @NumberOfEntries INT = CAST(RAND() * @MaxEntries + 1 AS INT);

	                      WHILE @NumberOfEntries > 0
	                      BEGIN
		                    DECLARE @RandomProjectId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM projects
									                    ORDER BY NEWID())

							DECLARE @RandomDate DATE = DATEADD(DAY, RAND(CHECKSUM(NEWID())) * @DaysBetween, @StartDate);
							DECLARE @RandomHours DECIMAL = CAST(RAND() * 7.75 + 0.25 AS DECIMAL(4, 2));
							DECLARE @ExistingRecord INT = (SELECT COUNT(*) FROM TimeLogs WHERE UserId = @UserId AND DateAdded = @RandomDate)

							IF @ExistingRecord > 0
							BEGIN
								DECLARE @HoursPerDay INT = (SELECT SUM(HoursWorked) FROM TimeLogs WHERE UserId = @UserId AND DateAdded = @RandomDate);

								IF @HoursPerDay + @RandomHours > 8
								BEGIN
									SET @RandomHours = 8 - @HoursPerDay;
								END;
							END;
			                
							IF @RandomHours > 0
							BEGIN
								INSERT INTO TimeLogs (Id,UserId, ProjectId, DateAdded, HoursWorked) values
								(
                                    NEWID(),
									@UserId, 
									@RandomProjectId, 
									@RandomDate, 
									@RandomHours
								)

								SET @NumberOfEntries = @NumberOfEntries - 1;
							END;

	                      END;

                          FETCH NEXT FROM @MyCursor
	                      INTO @UserId;
                        END; 

                        CLOSE @MyCursor ;
                        DEALLOCATE @MyCursor;
                    END;
                END;";

        public async static Task InitializeDatabase(UsersProjectsDbContext dbContext)
        {
            await CreateInitializationStoredProcedure(dbContext);
        }

        private static async Task CreateInitializationStoredProcedure(UsersProjectsDbContext dbContext)
        {
            using (var connection = dbContext.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT COUNT(*) FROM sys.objects WHERE name = @storedProcedureName AND type = 'P'";


                    var sqlParameter = command.CreateParameter();
                    sqlParameter.ParameterName = "@storedProcedureName";
                    sqlParameter.Value = "usp_ClearAndPopulateData";
                    command.Parameters.Add(sqlParameter);

                    await connection.OpenAsync();

                    var count = (int)await command.ExecuteScalarAsync();

                    if(count == 0)
                    {
                        dbContext.Database.ExecuteSqlRaw(STORED_PROCEDURE_SCRIPT);
                    }
                }
            }
        }
    }
}
