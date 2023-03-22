

using System.Collections.Generic;

public interface IDestroyable
{
    public List<Tile> GetDestructArea();

    public void Destroy();
}