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
        var FindItem = _valheim.Items.FirstOrDefault(i => i.ItemId == item.ItemId);
        if (FindItem == null)
        {
            throw new InvalidOperationException("This item already exists.");
        }
        _valheim.Items.Add(item);
        _valheim.SaveChanges();
        return item;
    }

    public IEnumerable<Item> GetItems()
    {
        return _valheim.Items.ToList();
    }

    public void DeleteItem(int itemId)
    {
        var FindItemById = _valheim.Items.Find(itemId);
        if (FindItemById == null)
        {
            throw new KeyNotFoundException("Item not found. ");
        }
        _valheim.Items.Remove(FindItemById);
        _valheim.SaveChanges();

    }
}