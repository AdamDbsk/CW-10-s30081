﻿using System;
using System.Collections.Generic;

namespace CW_10_s30081.Models;

public partial class Country
{
    public int IdCountry { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Trip> Trips { get; set; }
}
