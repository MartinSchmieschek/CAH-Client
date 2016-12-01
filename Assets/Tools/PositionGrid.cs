using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Tools
{
    class PositionGrid
    {
        float CulumSpacing = 1f;
        float RowSpacing = 1f;
        Vector3 Offset = new Vector3(0, 0, 0);

        public PositionGrid(float culumSpacing,float rowSpacing,Vector3 offset)
        {
            CulumSpacing = culumSpacing;
            RowSpacing = rowSpacing;
            Offset = offset;
        }

        public Vector3 GetAtXY(int collum,int row)
        {
            return new Vector3(Offset.x + (CulumSpacing * collum), Offset.y + (RowSpacing * row),Offset.z);
        }

        public Vector3 GetAtXZ(int collum, int row)
        {
            return new Vector3(Offset.x + (CulumSpacing * collum), Offset.z, Offset.z + (RowSpacing * row));
        }

        public Vector3 GetAtYZ(int collum, int row)
        {
            return new Vector3(Offset.x, Offset.z + (RowSpacing * row), Offset.z + (CulumSpacing * collum));
        }
    }
}
