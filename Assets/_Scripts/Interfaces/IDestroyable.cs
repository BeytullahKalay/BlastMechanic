

using System.Collections.Generic;

public interface IDestroyable
{
    public List<ITile> GetDestructArea();

    public void Destroy();
}