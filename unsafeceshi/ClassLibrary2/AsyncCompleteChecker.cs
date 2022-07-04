namespace ClassLibrary2;

public class AsyncCompleteChecker
{
    public static bool IsCompleted(object obj)
    {
        if (obj == null)
            return true;

        var op = obj as AsyncOp;
        if (op != null)
            return op.Completed;

        return true;
    }
}