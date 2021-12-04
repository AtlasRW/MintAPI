namespace Mint.API.DTOs
{
    public class MessageDTO
	{

		public int? Id { get; set; }
		public string Title { get; set; }
		public bool IsDraft { get; set; }
		public string? Header { get; set; }
		public string? Body { get; set; }
		public string? Background { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? Icon { get; set; }
		public int? Action { get; set; }
		public List<int> Categories { get; set; }

	}
}