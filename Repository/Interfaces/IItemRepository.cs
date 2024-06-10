using api_valheim.models;

namespace api_valheim.Repository;

public interface IItemRepository
{
    Item AddItem(Item item);
    IEnumerable<Item> GetItems();

    void DeleteItem(int id);
}