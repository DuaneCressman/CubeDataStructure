using System;
using System.Collections.Generic;
using System.Text;
using static CubeOrientation.Notation;

namespace CubeOrientation
{
    public struct CubeOrientation
    {
        public FaceColours Front;
        public FaceColours Top;

        public CubeOrientation(FaceColours front, FaceColours top)
        {
            Front = front;
            Top = top;
        }
    }
}
