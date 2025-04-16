using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Components.Interfaces
{
    public interface IBehaviour
    {
        float Evaluate();
        void Behave();
    }
}
