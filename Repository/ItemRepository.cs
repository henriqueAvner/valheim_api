using api_valheim.models;

namespace api_valheim.Repository;


public class ItemRepository : IItemRepository
{
    protected readonly ValheimContext _valheim;

    public ItemRepository(ValheimContext context)
    {
        _valheim = context;
    }

    public Item AddItem(Item item)
    {
        _valheim.Items.Add(item);
        _valheim.SaveChanges();
        return item;
    }

    public IEnumerable<Item> GetItems()
    {
        return _valheim.Items;
    }
}