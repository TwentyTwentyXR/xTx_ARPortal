Shader "Custom/Color_StencilFilter"
{
    Properties
    {
        _myValue("WorldValue", Range(1,255)) = 1
        _Color ("Color", Color) = (1,1,1,1)
        //[Enum(Equal,3,NotEqual,6)] _StencilTest ("Stencil Test", int) = 6
    }
    SubShader
    {
        Color [_Color]

        Stencil{
            Ref[_myValue]
            Comp [_StencilTest]
        }

        Pass
        {
            
        }
    }
}
