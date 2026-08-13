namespace ATaraxiaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemplateController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TemplateController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    [HttpGet("GetItem")]
    public async Task<ActionResult<Template>> GetItem(Guid id)
    {


        var Item = await _unitOfWork.Templates.GetByIdAsync(id, new[] { "UserLikes" });


        if (Item == null)
        {
            return NotFound();
        }
        else
        {

            return Ok(Item);
        }


    }
    [HttpGet("GetItems")]
    public async Task<ActionResult<IEnumerable<Template>>> GetItems()
    {
        var items = await _unitOfWork.Templates.GetAllAsync(new[] { "UserLikes" });
        if (items == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(items);
        }
    }

    [HttpGet("GetByName")]
    public async Task<ActionResult<Template>> GetByName(string name)
    {

        var items = await _unitOfWork.Templates.FindAsync(b => b.Title == name, new[] { "UserLikes" });

        if (items == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(items);
        }

    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Template>> Create(Template template)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _unitOfWork.Templates.CreateAsync(template);

        await _unitOfWork.CompleteAsync();

        return Ok(template);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, Template template)
    {
        if (id != template.TemplateId)
        {
            return BadRequest();
        }
        var existingTemplate = await _unitOfWork.Templates.GetByIdAsync(id, new[] { "UserLikes" });
        if (existingTemplate == null)
        {
            return NotFound();
        }

        existingTemplate.Title = template.Title;
        existingTemplate.FileUrl = template.FileUrl;
        existingTemplate.Picture = template.Picture;
        existingTemplate.File = template.File;
        existingTemplate.IsVideo = template.IsVideo;
        existingTemplate.UserLikes = template.UserLikes;


        await _unitOfWork.CompleteAsync();

        return Ok(existingTemplate);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var template = await _unitOfWork.Templates.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }
        await _unitOfWork.Templates.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }




}