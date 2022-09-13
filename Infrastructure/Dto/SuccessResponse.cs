namespace dotnet_2.Infrastructure.Dto
{
    public class SuccessResponse<T>
{
    public T? data {get; set;}
    public bool? success { get; set; }
    public string? message { get; set; }
    public string? origin { get; set; }
}
}