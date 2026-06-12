using Microsoft.AspNetCore.Mvc;

// [ApiController] enables automatic multipart form binding, automatic 400 responses
// for missing/invalid input, and other web API conveniences.
// Without it, [FromForm] won't bind the uploaded file correctly.
[ApiController]

// [controller] is a placeholder — resolves to the class name minus "Controller".
// So this becomes the route: POST /api/hotdog
[Route("api/[controller]")]
public class HotdogController : ControllerBase
{
    private readonly HotdogClassifierService _classifier;

    // ASP.NET reads this constructor and sees it needs a HotdogClassifierService.
    // It looks it up in the DI container (registered in Program.cs as a singleton)
    // and injects it automatically — you never call new HotdogController() yourself.
    // This is equivalent to importing a module in Node, but wired up by the framework.
    public HotdogController(HotdogClassifierService classifier)
    {
        _classifier = classifier;
    }

    // [HttpPost] maps this method to POST requests on /api/hotdog.
    // IActionResult is the return type that lets you return different HTTP responses
    // (Ok, BadRequest, NotFound, etc.) from the same method.
    [HttpPost]
    public IActionResult Classify([FromForm] IFormFile image)
    {
        // [FromForm] tells ASP.NET the file comes from a multipart/form-data body —
        // the same format curl -F and HTML <input type="file"> both send.
        if (image is null || image.Length == 0)
            return BadRequest("No file uploaded."); // → HTTP 400

        var isHotdog = _classifier.Classify(image);

        // Ok() serialises the object to JSON and sends it with HTTP 200.
        // new { isHotdog } is an anonymous object — shorthand for new { isHotdog = isHotdog }.
        // The client receives: { "isHotdog": true } or { "isHotdog": false }
        return Ok(new { isHotdog });
    }
}