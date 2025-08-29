namespace UserManagement.Models;

public class OpenWindowInput(Page page, string title, Action onClose, int? height, int width = 500)
{
    public Page Page { get; private set; } = page;
    public string Title { get; private set; } = title;     
    public int Width { get; private set; } = width;      
    public int? Height { get; private set; } = height;     
    public Action OnClose { get; private set; } = onClose; 
}

