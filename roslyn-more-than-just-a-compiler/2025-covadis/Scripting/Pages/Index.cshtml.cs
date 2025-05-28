using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Scripting.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Code { get; set; } = "1 + 2";

    public object? Output { get; set; }
    
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Output = await CSharpScript.EvaluateAsync<object?>(Code);
        return Page();
    }
}