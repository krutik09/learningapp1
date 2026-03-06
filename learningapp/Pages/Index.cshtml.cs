using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace learningapp.Pages;

public class IndexModel : PageModel
{
    public List<Courses> Courses = new List<Courses>();
    private readonly ILogger<IndexModel> _logger;
    private IConfiguration _configuration;
    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {

        string connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        var sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();

        var sqlcommand = new SqlCommand(
        "SELECT Id,Name,Rating FROM Courses;", sqlConnection);
        using (SqlDataReader sqlDatareader = sqlcommand.ExecuteReader())
        {
            while (sqlDatareader.Read())
            {
                Courses.Add(new Courses()
                {
                    Id = Int32.Parse(sqlDatareader["Id"].ToString()),
                    Name = sqlDatareader["Name"].ToString(),
                    Rating = Decimal.Parse(sqlDatareader["Rating"].ToString())
                });
            }
        }
    }
}
