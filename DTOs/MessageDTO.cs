namespace Mint.API.DTOs
{
    public interface MessageDTO
	{

		int? Id { get; set; }
		string Title { get; set; }
		bool IsDraft { get; set; }
		string? Header { get; set; }
		string? Body { get; set; }
		string? Background { get; set; }
		DateTime? StartDate { get; set; }
		DateTime? EndDate { get; set; }
		int? Icon { get; set; }
		int? Action { get; set; }
		List<int> Categories { get; set; }

	}
}