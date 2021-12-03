namespace Mint.API.DTOs
{
    public interface MessageFiltersDTO
	{

		List<int> Categories { get; set; }
		bool? IsCurrent { get; set; }
		bool? IsDraft { get; set; }

	}
}