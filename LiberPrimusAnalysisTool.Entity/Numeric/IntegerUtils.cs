namespace LiberPrimusAnalysisTool.Entity.Numeric;

public static class IntegerUtils
{
    public static IntBits DetectIntegerType(string text)
    {
        if (byte.TryParse(text, out _))
        {
            return IntBits.Byte;
        }
        else if (ushort.TryParse(text, out _) )
        {
            return IntBits.Short;
        }
        else if (uint.TryParse(text, out _))
        {
            return IntBits.Int;
        }
        else if (ulong.TryParse(text, out _))
        {
            return IntBits.Long;
        }
        else
        {
            return IntBits.Error;
        }
    }

    public static string[] GetIntBitsForList()
    {
        string[] retval = ["Byte", "Short", "Int32", "Int64"];
        return retval;
    }

    public static int ConvertToIntBitValue(IntBits intType)
    {
        switch (intType)
        {
            case IntBits.Byte:
                return 8;
            case IntBits.Short:
                return 16;
            case IntBits.Int:
                return 32;
            case IntBits.Long:
                return 64;
            default:
                return 0;
        }
    }
    
    public static IntBits ConvertToEnum(string text)
    {
        return text switch
        {
            "Byte" => IntBits.Byte,
            "Short" => IntBits.Short,
            "Int32" => IntBits.Int,
            "Int64" => IntBits.Long,
            _ => IntBits.Error
        };
    }
}