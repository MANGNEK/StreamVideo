namespace StreamVideo.models;

public class UploadVideoRequest
{
    public IFormFile File { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
}
