namespace VueJS.Shared.Entities.Abstract
{
    public interface IEntityPostBase
    {
        int Id { get; set; }
        string Guid { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        string Picture { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        int UserId { get; set; }
    }
}