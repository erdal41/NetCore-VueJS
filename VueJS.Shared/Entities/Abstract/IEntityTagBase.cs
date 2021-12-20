namespace VueJS.Shared.Entities.Abstract
{
    public interface IEntityTagBase
    {
        int Id { get; set; }
        string Guid { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int? ParentId { get; set; }
    }
}