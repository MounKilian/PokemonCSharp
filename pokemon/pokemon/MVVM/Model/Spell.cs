using System;
using System.Collections.Generic;

namespace pokemon.Model;

public partial class Spell
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Damage { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Monster> Monsters { get; set; } = new List<Monster>();
}
