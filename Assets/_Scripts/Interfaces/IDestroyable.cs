

using System.Collections.Generic;

public interface IDestroyable
{
    public List<IDestroyable> GetDestructArea();

    public void Destroy();
}