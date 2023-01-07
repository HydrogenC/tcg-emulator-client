using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Character
{
    public string Name { get; set; }
    public int Hp { get; set; }

    public Character(string name, int hp = 10)
    {
        Name = name;
        Hp = hp;
    }
}
