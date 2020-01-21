using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// Creates a test bar chart
public class TestChart : ViewComponent {
    public async Task<IViewComponentResult> InvokeAsync(string message) {
        return await Task.Run(() => View("Default", message));
    }
}