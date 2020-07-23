﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Menus
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public IList<MenuChildItem> Children { get; set; }
    }
}
