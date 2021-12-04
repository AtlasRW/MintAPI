namespace Mint.API.DTOs
{
    public class MessageFiltersDTO
	{

		public List<int> Categories { get; set; }
		public bool? IsCurrent { get; set; }
		public bool? IsDraft { get; set; }

	}
}