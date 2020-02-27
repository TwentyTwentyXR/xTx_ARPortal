Shader "Custom/PortalWindow"
{
    Properties
    {
        _myValue("WorldValue", Range(1,255)) = 1
    }

    SubShader
    {

        Zwrite off
        ColorMask 0
        Cull off

        Stencil{
            Ref[_myValue]
            Pass replace
        }

        Pass
        {

        }
    }
}