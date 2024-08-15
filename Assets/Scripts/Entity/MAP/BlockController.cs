using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public List<Block> blocks = new List<Block>();

   public void Control()
    {
        foreach (var block in blocks)
        {
            block.Switch();
        }
    }
}
