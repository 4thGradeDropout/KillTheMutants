using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathToCommandConverter
{
    public List<ElemCommand> PathToCommands(List<Vector2> Path, Navigator host)
    {
        List<ElemCommand> result = new List<ElemCommand>();
        Path.ForEach(p => result.Add(new ElemCommand_MOVE(host, p)));
        return result;
    }
}
