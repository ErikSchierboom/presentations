using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Scripting.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Code { get; set; } = "1 + 2";

    public ScriptState<object?>? State { get; set; }
    
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        State = await CSharpScript.RunAsync(Code);
        return Page();
    }
}