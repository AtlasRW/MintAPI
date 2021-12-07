using System.Data;
using Mint.API.Interfaces;
using System.Data.SqlClient;

namespace Mint.API.Models
{
    public class Message : Model
    {

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public bool IsDraft { get; set; }
        public string? Header { get; set; }
        public string? Body { get; set; }
        public string? Background { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Icon { get; set; }
        public int? Action { get; set; }
        public List<int> Categories { get; set; } = new List<int>();

        public bool Read(SqlDataReader pReader)
        {
            if (pReader == null || !pReader.Read()) return false;

            Id          = pReader.GetFieldValue<int>("MESSAGE_id");
            Title       = pReader.GetFieldValue<string>("MESSAGE_title");
            IsDraft     = pReader.GetFieldValue<bool>("MESSAGE_is_draft");
            Header      = pReader.IsDBNull("MESSAGE_header") ? null : pReader.GetFieldValue<string>("MESSAGE_header");
            Body        = pReader.IsDBNull("MESSAGE_body") ? null : pReader.GetFieldValue<string>("MESSAGE_body");
            Background  = pReader.IsDBNull("MESSAGE_background") ? null : pReader.GetFieldValue<string>("MESSAGE_background");
            StartDate   = pReader.IsDBNull("MESSAGE_start_date") ? null : pReader.GetFieldValue<DateTime>("MESSAGE_start_date");
            EndDate     = pReader.IsDBNull("MESSAGE_end_date") ? null : pReader.GetFieldValue<DateTime>("MESSAGE_end_date");
            Icon        = pReader.IsDBNull("MESSAGE_icon") ? null : pReader.GetFieldValue<int>("MESSAGE_icon");
            Action      = pReader.IsDBNull("MESSAGE_action") ? null : pReader.GetFieldValue<int>("MESSAGE_action");
            Categories  = pReader.IsDBNull("MESSAGE_categories")
                ? new List<int>()
                : Array.ConvertAll(pReader.GetFieldValue<string>("MESSAGE_categories").Split(','), int.Parse).ToList();

            return true;
        }

    }
}