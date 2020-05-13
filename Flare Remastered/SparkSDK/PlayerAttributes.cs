﻿using Flare_Remastered.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare_Remastered.SparkSDK
{
    public class PlayerAttributes : SDKObj
    {
        public PlayerAttributes(ulong addr) : base(addr)
        {
        }

        public float playerSpeed
        {
            get
            {
                return MCM.readFloat(addr + 0x9C);
            }
            set
            {
                MCM.writeFloat(addr + 0x9C, value);
            }
        }
    }
}