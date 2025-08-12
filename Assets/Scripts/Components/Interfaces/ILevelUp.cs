using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Components.Interfaces
{
    internal interface ILevelUp
    {
        void LevelUp(CharacterData data, int level);
        public int minLevel {  get; set; }
    }
}
