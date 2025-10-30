namespace RainbowProject.Models.Dto
{
    public class ColorItemDtoRequest
    {
        public string Hex { get; set; } = "#000000";
        public Guid UserId { get; set; }
    }
}